using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityEnemyDetector : MonoBehaviour
{
    [SerializeField] private Transform rotatorObject;
    [SerializeField] private float detectionAngle;
    [SerializeField] private float detectionRadius;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    private Transform currentTarget;
    private Vector3 targetLastKnownPosition;

    private Collider[] targetsIndetectionRadius;

    public List<Transform> visiblePlayers = new List<Transform>();

    private void FixedUpdate()
    {
        FindVisibleTargets();
    }

    private void FindVisibleTargets()
    {
        visiblePlayers.Clear();
        targetsIndetectionRadius = Physics.OverlapSphere(rotatorObject.position, detectionRadius, playerMask);


        for (int i = 0; i < targetsIndetectionRadius.Length; i++)
        {
            Transform target = targetsIndetectionRadius[i].transform;
            Vector3 directionToTarget = (target.position - rotatorObject.position).normalized;
            if (Vector3.Angle(rotatorObject.forward, directionToTarget) < detectionAngle / 2)
            {
                float distanceTotarget = Vector3.Distance(rotatorObject.position, target.position);

                if (!Physics.Raycast(rotatorObject.position, directionToTarget, distanceTotarget, obstacleMask))
                {
                    currentTarget = target;
                    return;
                }
            }
        }
    }
}
