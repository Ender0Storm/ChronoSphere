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
    public AudioSource explosionSound;
    public AudioSource hitSound;
    public PlayerMovement playerMovement;
    public bool playerIsDead = false;

    public GameObject leg11;
    public GameObject leg12;
    public GameObject leg13;
    public GameObject leg21;
    public GameObject leg22;
    public GameObject leg23;
    public GameObject leg31;
    public GameObject leg32;
    public GameObject leg33;
    public GameObject bottomSphere;
    public GameObject topSphere;
    public GameObject topSphereHead;
    
    private Renderer rendLeg11;
    private Renderer rendLeg12;
    private Renderer rendLeg13;
    private Renderer rendLeg21;
    private Renderer rendLeg22;
    private Renderer rendLeg23;
    private Renderer rendLeg31;
    private Renderer rendLeg32;
    private Renderer rendLeg33;
    private Renderer rendBottomSphere;
    private Renderer rendTopSphere;
    private Renderer rendTopSphereHead;
    
    private void Start()
    {
        health = maxHealth;
        rendLeg11 = leg11.GetComponent<Renderer>();
        rendLeg12 = leg12.GetComponent<Renderer>();
        rendLeg13 = leg13.GetComponent<Renderer>();
        rendLeg21 = leg21.GetComponent<Renderer>();
        rendLeg22 = leg22.GetComponent<Renderer>();
        rendLeg23 = leg23.GetComponent<Renderer>();
        rendLeg31 = leg31.GetComponent<Renderer>();
        rendLeg32 = leg32.GetComponent<Renderer>();
        rendLeg33 = leg33.GetComponent<Renderer>();
        rendBottomSphere = bottomSphere.GetComponent<Renderer>();
        rendTopSphere = topSphere.GetComponent<Renderer>();
        rendTopSphereHead = topSphereHead.GetComponent<Renderer>();
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
        SetRendererEnabled(false);
        // playerBody.SetActive(false);
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
        SetRendererEnabled(true);
        playerMovement.canMove = true;
    }
    
    public void SetRendererEnabled(bool value)
    {
        rendLeg11.enabled = value;
        rendLeg12.enabled = value;
        rendLeg13.enabled = value;
        rendLeg21.enabled = value;
        rendLeg22.enabled = value;
        rendLeg23.enabled = value;
        rendLeg31.enabled = value;
        rendLeg32.enabled = value;
        rendLeg33.enabled = value;
        rendBottomSphere.enabled = value;
        rendTopSphere.enabled = value;
        rendTopSphereHead.enabled = value;
    }
    
}

