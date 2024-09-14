using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IEnemyComponent
{
    [SerializeField] private float _stopOffset = 0.05f;

    private Enemy _enemy;
    public NavMeshAgent NavAgent { get; private set; }
    public bool IsManualMovement { get; private set; }
    public bool IsManualRotation { get; private set; }

    public bool IsArrived => !NavAgent.isPathStale && NavAgent.remainingDistance < NavAgent.stoppingDistance + _stopOffset;

    private Vector3 _lastCornerPoint;
    public void Initialize(Enemy enmey)
    {
        _enemy = enmey;
        NavAgent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetNextPathPoint()
    {
        NavMeshPath path = NavAgent.path;

        if (path.corners.Length < 2)
            return NavAgent.destination;

        for (int i = 0; i < path.corners.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, path.corners[i]);

            if (distance < 1 && i < path.corners.Length - 1)
            {
                _lastCornerPoint = path.corners[i + 1];
                return _lastCornerPoint;
            }
        }

        return _lastCornerPoint;
    }

    public void SetStop(bool isStop) => NavAgent.isStopped = isStop;
    public void SetSpeed(float speed) => NavAgent.speed = speed;
    public void SetDestination(Vector3 destination) => NavAgent.SetDestination(destination);
    public void SetVelocity(Vector3 velocity) => NavAgent.velocity = velocity;

    public void SetManualMovement(bool isManual)=>IsManualMovement = isManual;
    public void SetManualRotation(bool isManual)=>IsManualRotation = isManual;
}
