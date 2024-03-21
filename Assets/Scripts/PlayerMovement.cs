using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerMovement : MonoBehaviour
{
    [Header("General Information")]
    public Animator playerAnimator;
    public List<ChainIKConstraint> legConstraints;
    [Min(0.1f)]
    public float transformSpeed;
    
    //Shoot Mode
    [Header("Shoot Mode")]
    public float accShootMode = 10.0f;
    public float speedShootMode = 5.0f;
    public float turnSpeed = 10.0f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float projectileTime = 5.0f;
    public float aimAdjustment = 0.5f;
    public float projectileHeight = 0.5f;
    public float projectileSpawnDistance = 2.0f;
    //public AudioSource gunSound;
    
    //Ball Mode
    [Header("Ball Mode")]
    public float speedBallMode = 10f;
    public float maxSpeed = 20f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float startSpeed = 5f;
    

    private Rigidbody rb;
    private CapsuleCollider shootCollider;
    private SphereCollider ballCollider;
    private float transformingLerp;

    private bool isBallMode;
    private bool isDashing;
    private bool isTransforming;
    private bool canDash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        shootCollider = GetComponent<CapsuleCollider>();
        ballCollider = GetComponent<SphereCollider>();
        transformingLerp = 1;
        isBallMode = false;
        canDash = true;
    }

    void FixedUpdate()
    {
        if (!isTransforming)
        {
            if (isBallMode)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

                //Met une limite de vitesse quand on est pas entrain de dash
                if (!isDashing)
                {
                    rb.AddForce(movement * speedBallMode);
                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
                }
            }
            else
            {
                MovePlayerShootMode();
                RotateAtCursor(); 
            }
        }
    }
    
    void Update()
    {
        if (!isTransforming)
        {
            if (isBallMode)
            {
                if (Input.GetButtonDown("Dash") && canDash && !isDashing)
                {
                    float moveHorizontal = Input.GetAxis("Horizontal");
                    float moveVertical = Input.GetAxis("Vertical");

                    Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
                    StartCoroutine(Dash(movement));
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    FireProjectile();
                } 
            }
        }
    }

    void MovePlayerShootMode()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        rb.AddForce(movement * accShootMode);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedShootMode);
    }
    
    void RotateAtCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y + aimAdjustment, 0));
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
            Vector3 directionToLook = (pointOfIntersection - transform.position).normalized;
            Quaternion lookDirection = Quaternion.LookRotation(new Vector3(directionToLook.x, 0, directionToLook.z));
            Vector3 turnDirection = (lookDirection * Quaternion.Inverse(transform.rotation)).eulerAngles;
            turnDirection.y -= turnDirection.y > 180 ? 360 : 0;
            
            rb.angularVelocity = turnDirection * turnSpeed * Time.deltaTime;
        }
    }
    
    void FireProjectile()
    {
        //Crée le projectile à l'avant du joueur et à la bonne hauteur
        Vector3 shootDirection = transform.forward;
        Vector3 spawnPosition = transform.position + shootDirection * projectileSpawnDistance;
        spawnPosition.y += projectileHeight;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.useGravity = false;
        projectileRigidbody.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
        Destroy(projectile, projectileTime);
        //gunSound.Play();
    }
    
    //Serait appelé par le game manager
    public void ChangeMode()
    {
        if (!isTransforming)
        {
            StartCoroutine(Transform());
        }
    }
    
    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        canDash = false;
        rb.AddForce(direction * dashSpeed, ForceMode.Impulse);
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        isDashing = false;
        //Ajout d'un délais avant de pouvoir recommencer le dash
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator Transform()
    {
        transformingLerp = 0;
        isTransforming = true;
        Vector3 velocityBefore = rb.velocity;
        Quaternion rotationBefore = transform.rotation;
        Quaternion rotationAfter = Quaternion.Euler(0, rotationBefore.eulerAngles.y, 0);

        if (isBallMode) {
            rb.angularVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        playerAnimator.SetTrigger(isBallMode ? "Open" : "Close");

        while (transformingLerp < 1)
        {
            transformingLerp += Time.fixedDeltaTime * transformSpeed;

            foreach (ChainIKConstraint constraint in legConstraints)
            {
                constraint.weight = isBallMode ? transformingLerp : 1 - transformingLerp;
            }

            if (isBallMode)
            {
                transform.rotation = Quaternion.Lerp(rotationBefore, rotationAfter, transformingLerp);
                rb.velocity = Vector3.Lerp(velocityBefore, Vector3.zero, transformingLerp);
            }
            
            yield return new WaitForFixedUpdate();
        }

        foreach (ChainIKConstraint constraint in legConstraints)
        {
            constraint.weight = isBallMode ? 1 : 0;
        }

        if (isBallMode)
        {
            transform.rotation = rotationAfter;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        yield return new WaitUntil(() => (isBallMode && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Opened"))
                                      || (!isBallMode && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Ball")));

        isBallMode = !isBallMode;

        ((Collider)(isBallMode ? ballCollider : shootCollider)).enabled = true;
        ((Collider)(isBallMode ? shootCollider : ballCollider)).enabled = false;

        if (isBallMode)
        {
            rb.constraints = RigidbodyConstraints.None;

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            
            Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);

            rb.AddForce(direction * startSpeed, ForceMode.Impulse);
        }

        isTransforming = false;
        yield return null;
    }
    
}
