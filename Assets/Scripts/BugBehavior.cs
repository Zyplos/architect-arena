using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BugBehavior : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location){
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    // Update is called once per frame
    void Update()
    {
        Seek(target.transform.position);


        // Debug.Log("AGENT REMDISTANCE | " + agent.remainingDistance);

        // if distance is less than 2 set animation trigger to true
        if (agent.remainingDistance < 2)
        {
            animator.SetBool("happy", true);
            animator.SetBool("chasing", false);
        }
        else
        {
            animator.SetBool("happy", false);
            animator.SetBool("chasing", true);
        }
    }
}
