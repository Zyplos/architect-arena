using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RatBehavior : MonoBehaviour
{
    public Transform player;
    public float fleeDistance = 10f;
    public float fleeSpeed = 5f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private NavMeshAgent agent;
    private Vector3 fleeDestination;
    private bool isFleeing = false;
    private float timer;
    private Vector3 randomPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
            enabled = false;
        }
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is within flee distance
        if (distanceToPlayer < fleeDistance)
        {
            isFleeing = true;
            // Calculate a destination away from the player
            fleeDestination = transform.position + (transform.position - player.position).normalized * fleeDistance * 2f;
        }
        else
        {
            isFleeing = false;
        }

        // If fleeing, navigate to the flee destination
        if (isFleeing)
        {
            agent.SetDestination(fleeDestination);
            agent.speed = fleeSpeed;
        }
        else // If not fleeing, wander around
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                randomPosition = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(randomPosition);
                timer = 0;
            }
        }
    }

    // Generate random position within a sphere
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
