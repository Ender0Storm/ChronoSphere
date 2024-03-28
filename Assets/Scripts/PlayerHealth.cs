using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int health;
    public int damageProjectile = 1;
    public GameManager gameManager;
    public GameObject explosionPrefab;
    public GameObject playerBody;
    
    private void Start()
    {
        health = maxHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ProjectileEnemy>() != null)
        {
            //hitSound.Play();
            health-=damageProjectile;
        }

        if (health == 0)
        {
            playerBody.SetActive(false);
            explosionPrefab.GetComponent<ParticleSystem>().Play();
            Invoke("GameOver", 2f);
        }
    }
    
    private void GameOver()
    {
        gameManager.GameOver();
    }
    
    
}

