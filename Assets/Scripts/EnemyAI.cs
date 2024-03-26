using System.Collections;
using System.Collections.Generic;
using Polybrush;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

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
    private float turnSpeed;
    [SerializeField]
    private float shootRate;
    [SerializeField]
    private List<Vector3> patrolRoute;
    [SerializeField]
    private float pauseTime;

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
    private int currentPatrolPoint;
    private float shootTimer;
    private float untargetTimer;
    private float pauseTimer;
    private NavMeshAgent navigationAgent;
    private bool targetVisible;

    // Start is called before the first frame update
    void Start()
    {
        currentPatrolPoint = 0;
        shootTimer = shootRate;
        pauseTimer = pauseTime;
        untargetTimer = 0;
        navigationAgent = transform.parent.GetComponent<NavMeshAgent>();
        targetVisible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null)
        {
            FindTarget();
            Patrolling();
        }
        else
        {
            UpdateVisibility();
            AimTowardTarget(currentTarget.transform.position);
            ShootTargetIfAble();
            CheckTargetAvailability();
        }

        if (shootTimer < shootRate) shootTimer += Time.deltaTime;
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        if (colliders.Length > 0 && !Physics.Raycast(transform.position, colliders[0].transform.position - transform.position, detectionRange, groundLayer))
        {
            SetCurrentTarget(colliders[0].gameObject);
        }
    }

    void Patrolling()
    {
        if (patrolRoute.Count == 1) navigationAgent.SetDestination(patrolRoute[0]);
        else if (patrolRoute.Count > 1)
        {
            if (navigationAgent.velocity.sqrMagnitude < 0.01 && navigationAgent.remainingDistance < 0.1)
            {
                if (pauseTimer >= pauseTime)
                {
                    currentPatrolPoint = (currentPatrolPoint + 1) % patrolRoute.Count;

                    navigationAgent.SetDestination(patrolRoute[currentPatrolPoint]);
                    pauseTimer = 0;
                }
                else
                {
                    pauseTimer += Time.deltaTime;
                }
            }
        }
    }

    void UpdateVisibility()
    {
        Vector3 lookVector = currentTarget.transform.position - transform.position;
        targetVisible = !Physics.SphereCast(transform.position, projectilePrefab.transform.localScale.x, lookVector, out _, lookVector.magnitude, groundLayer);
    }

    void AimTowardTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, math.min(turnSpeed * Time.deltaTime, 1));
    }

    void ShootTargetIfAble()
    {
        if (targetVisible && shootTimer >= shootRate
            && Quaternion.Angle(Quaternion.LookRotation(currentTarget.transform.position - transform.position), transform.rotation) < 5)
        {
            FireProjectile();
            shootTimer = 0;
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
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= untargetRange && targetVisible)
        {
            untargetTimer = 0;
            if (!navigationAgent.isStopped) navigationAgent.SetDestination(transform.position);
        }
        else
        {
            untargetTimer += Time.deltaTime;

            if (untargetTimer > untargetDelay)
            {
                SetCurrentTarget(null);
                if (!navigationAgent.isStopped) navigationAgent.SetDestination(transform.position);
            }
            else
            {
                navigationAgent.SetDestination(currentTarget.transform.position);
            }
        }
    }

    public void SetCurrentTarget(GameObject target)
    {
        currentTarget = target;
        untargetTimer = 0;
    }

    public void SetPatrolRoute(List<Vector3> route)
    {
        navigationAgent.SetDestination(transform.position);
        currentPatrolPoint = 0;
        patrolRoute = route;
    }
}
