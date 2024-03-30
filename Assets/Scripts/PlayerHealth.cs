using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int health;
    public int damageProjectile = 1;
    public GameObject explosionPrefab;
    public GameObject sparksPrefab;
    public GameObject playerBody;
    public AudioSource explosionSound;
    public AudioSource hitSound;
    public PlayerMovement playerMovement;
    public bool playerIsDead = false;
    
    private void Start()
    {
        health = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileEnemy>() != null)
        {
            if (health > 0)
            {
                hitSound.Play();
                sparksPrefab.GetComponent<ParticleSystem>().Play();
            }
            health -= damageProjectile;
            hitSound.pitch = 1.5f - ((float)health / maxHealth); // Modifie le pitch pour qu'il augmente lorsque la santé diminue
        }

        if (health == 0)
        {
            GameOver();
            // playerMovement.canMove = false;
            // explosionSound.Play();
            // playerBody.SetActive(false);
            // explosionPrefab.GetComponent<ParticleSystem>().Play();
            // Invoke("GameOver", 2f);
        }
    }
    
    public void GameOver()
    {
        playerMovement.canMove = false;
        explosionSound.Play();
        playerBody.SetActive(false);
        explosionPrefab.GetComponent<ParticleSystem>().Play();
        Invoke("SetPlayerIsDead", 2f);
    }

    public void SetPlayerIsDead()
    {
        playerIsDead = true;
    }
    
    public void Respawn()
    {
        health = maxHealth;
        playerBody.SetActive(true);
        playerMovement.canMove = true;
    }
    
}

