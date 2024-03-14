using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public float maxSpeed = 20f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float startSpeed = 5f;
    

    private Rigidbody rb;
    private bool isDashing;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);
        
        //StartSpeed permet d'avoir un élan quand on passe du mode tir au mode boule
        rb.AddForce(direction * startSpeed, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        //Met une limite de vitesse quand on est pas entrain de dash
        if (!isDashing)
        {
            rb.AddForce(movement * speed);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isDashing)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
            StartCoroutine(Dash(movement));
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