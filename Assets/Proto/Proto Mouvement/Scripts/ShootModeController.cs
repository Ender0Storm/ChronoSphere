using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootModeController : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float projectileTime = 5.0f;
    public float aimAdjustment = 0.5f;
    public float projectileHeight = 0.5f;
    public AudioSource gunSound;

    void FixedUpdate()
    {
        MovePlayer();
        RotateAtCursor();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireProjectile();
        }
    }
    
    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.position += movement * speed * Time.deltaTime;
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
        gunSound.Play();
    }
}
