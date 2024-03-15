using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isBallMode = false;
    
    //Shoot Mode
    public float speedShootMode = 5.0f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float projectileTime = 5.0f;
    public float aimAdjustment = 0.5f;
    public float projectileHeight = 0.5f;
    //public AudioSource gunSound;
    
    //Ball Mode
    public float speedBallMode = 10f;
    public float maxSpeed = 20f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float startSpeed = 5f;
    

    private Rigidbody rb;
    private bool isDashing;
    private bool canDash = true;
    void FixedUpdate()
    {
        if (!isBallMode)
        {
            MovePlayerShootMode();
            RotateAtCursor(); 
        }
        
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
        
    }
    
    void Update()
    {
        if (!isBallMode)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                FireProjectile();
            } 
        }

        if (isBallMode)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
            {
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");

                Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
                StartCoroutine(Dash(movement));
            }
        }

    }


    void MovePlayerShootMode()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.position += movement * speedShootMode * Time.deltaTime;
    }
    
    void RotateAtCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y + aimAdjustment, 0));
        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 pointOfIntersection = ray.GetPoint(rayDistance);
            Vector3 directionToLook = (pointOfIntersection - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(new Vector3(directionToLook.x, 0, directionToLook.z));
        }
    }
    
    void FireProjectile()
    {
        //Crée le projectile à l'avant du joueur et à la bonne hauteur
        Vector3 shootDirection = transform.forward;
        Vector3 spawnPosition = transform.position + shootDirection;
        spawnPosition.y += projectileHeight;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.useGravity = false;
        projectileRigidbody.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
        Destroy(projectile, projectileTime);
        //gunSound.Play();
    }
    
    
    //@Todo Faire le changement de mode
    //Serait appelé par le game manager
    public void ChangeMode()
    {
        //@TODO Il faut mettre le code pour l'animation.
        //Permet de passer du mode tire au mode boule
        if (!isBallMode)
        {
            isBallMode = true;
            rb = GetComponent<Rigidbody>();
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);
        
            //StartSpeed permet d'avoir un élan quand on passe du mode tir au mode boule
            rb.AddForce(direction * startSpeed, ForceMode.Impulse);
        }

        else
        {
            isBallMode = false;
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

}
