using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10.0f;
    public float projectileTime = 5.0f;
    public float shootingInterval = 2.0f; // Intervalle de temps entre les tirs
    public float shootingChance = 0.1f; // Probabilité de tirer à chaque intervalle
    public float projectileHeight = 0.5f;
    public AudioSource gunSound;

    private float timeSinceLastShot = 0.0f;

    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootingInterval)
        {
            if (Random.value < shootingChance) // Random.value retourne un nombre entre 0.0 et 1.0
            {
                FireProjectile();
            }
            timeSinceLastShot = 0.0f; // Réinitialiser le temps depuis le dernier tir
        }
    }

    void FireProjectile()
    {
        // Crée le projectile à l'avant de l'ennemi et à la bonne hauteur
        Vector3 shootDirection = transform.forward + transform.forward;
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