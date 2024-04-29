using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private MeshRenderer meshRenderer;
    private TrailRenderer trail;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        trail = GetComponent<TrailRenderer>();
    }

    //Détruit le projectile quand il rentre en contact avec un objet
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            meshRenderer.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezePosition;

            Invoke(nameof(DestroySelf), trail.time);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
