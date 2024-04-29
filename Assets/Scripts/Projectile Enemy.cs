using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Enemy")
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
