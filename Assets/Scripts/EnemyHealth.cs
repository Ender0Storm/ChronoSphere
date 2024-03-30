using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int health;
    private Renderer rend;
    public float minSpeedForDamage = 5f;
    public bool isDead = false;

    public int damageCollision = 6;
    public int damageProjectile = 1;

    public AudioSource hitSound;

    //Pas nécessaire dans le projet final mais le fun a garder pour le premier jouable
    public GameObject head;

    private string PLAYER_TAG = "Player";

    private void Start()
    {
        health = maxHealth;
        rend = GetComponentInChildren<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            hitSound.Play();
            health -= damageProjectile;

            GetComponentInChildren<EnemyAI>().SetCurrentTarget(GameObject.FindGameObjectWithTag(PLAYER_TAG));
        }
        else if (collision.gameObject.tag == PLAYER_TAG && collision.relativeVelocity.magnitude > minSpeedForDamage)
        {
            hitSound.Play();
            health -= damageCollision;
        }

        float healthPercentage = (float)health / maxHealth;
        rend.material.color = Color.Lerp(Color.red, Color.yellow, healthPercentage);

        if (health <= 0)
        {
            isDead = true;
            StartCoroutine(DestroyAfterSound());
            StartCoroutine(DelayHeadDetach(0.0f));
        }
    }

    IEnumerator DestroyAfterSound()
    {
        GetComponentInChildren<Renderer>().enabled = false;
        hitSound.Play();
        yield return new WaitForSeconds(hitSound.clip.length);
        Destroy(gameObject);
    }

    
    //Pas nécessaire dans le projet final mais le fun a garder pour le premier jouable
    IEnumerator DelayHeadDetach(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (head != null)
        {
            Rigidbody headRb = head.GetComponent<Rigidbody>();
            if (headRb == null)
            {
                headRb = head.AddComponent<Rigidbody>();
            }
            head.GetComponent<BoxCollider>().enabled = true;
            head.transform.parent = null;
            headRb.isKinematic = false;
            headRb.mass = 0.5f;
            Destroy(head, 1.5f);
        }
    }
}