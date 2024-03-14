using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float patrolRadius = 10.0f; //détermine la distance maximale à laquelle l'ennemi peut se déplacer pour son prochain point de patrouille.
    public float patrolTimer = 4.0f; //détermine le temps d'attente avant de calculer un nouveau point de destination.

    private Transform target;
    private NavMeshAgent agent;
    private float timer;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = patrolTimer; 
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= patrolTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, patrolRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

}