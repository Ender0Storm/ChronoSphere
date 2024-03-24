using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private float detectionRange;
    [SerializeField]
    private float untargetRange;
    [SerializeField]
    private float untargetDelay;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float shootRate;

    [Header("Layers")]
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawn;
    [SerializeField]
    private AudioSource projectileSound;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileDuration;

    private GameObject currentTarget;
    private float shootTimer;
    private float untargetTimer;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 0;
        untargetTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            FindTarget();
        }
        else
        {
            AimTowardTarget(currentTarget.transform.position);
            ShootTargetIfAble();
            CheckTargetAvailability();
        }

        shootTimer += Time.deltaTime;
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        if (colliders.Length > 0 && !Physics.Raycast(transform.position, colliders[0].transform.position, detectionRange, groundLayer))
        {
            currentTarget = colliders[0].gameObject;
        }
    }

    void AimTowardTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, math.min(turnSpeed * Time.deltaTime, 1));
    }

    void ShootTargetIfAble()
    {
        if (shootTimer <= 0 && Quaternion.Angle(Quaternion.LookRotation(currentTarget.transform.position - transform.position), transform.rotation) < 5)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        //Crée le projectile à l'avant du joueur et à la bonne hauteur
        Vector3 shootDirection = transform.forward;
        Vector3 spawnPosition = projectileSpawn.position;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.useGravity = false;
        projectileRigidbody.AddForce(shootDirection * projectileSpeed, ForceMode.VelocityChange);
        Destroy(projectile, projectileDuration);
        projectileSound.Play();
    }

    void CheckTargetAvailability()
    {
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= untargetRange)
        {
            untargetTimer = 0;
        }
        else
        {
            untargetTimer += Time.deltaTime;
            if (untargetTimer > untargetDelay)
            {
                currentTarget = null;
            }
        }
    }
}
