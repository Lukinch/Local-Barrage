using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitEnemyMover : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float walkRaius;

    private void Update()
    {
        if (navMeshAgent != null && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(GetRandomNavMeshLocation());
        }
    }

    private Vector3 GetRandomNavMeshLocation()
    {
        Vector3 finalPosition = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRaius;
        randomPosition += transform.position;
        if (NavMesh.SamplePosition(randomPosition,out NavMeshHit hit, walkRaius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
