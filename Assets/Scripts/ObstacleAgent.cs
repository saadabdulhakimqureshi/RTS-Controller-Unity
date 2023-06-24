using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(NavMeshObstacle))]
public class ObstacleAgent : MonoBehaviour
{
    [SerializeField]
    private float CarvingTime = 0.5f;
    [SerializeField]
    private float CarvingMoveThreshold = 0.1f;

    public NavMeshAgent Agent;
    public NavMeshObstacle Obstacle;

    private float LastMoveTime;
    private Vector3 LastPosition;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Obstacle = GetComponent<NavMeshObstacle>();

        Obstacle.enabled = false;
        Obstacle.carveOnlyStationary = false;
        Obstacle.carving = true;

        LastPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(LastPosition, transform.position) > CarvingMoveThreshold)
        {
            LastMoveTime = Time.time;
            LastPosition = transform.position;
        }
        if (LastMoveTime + CarvingTime < Time.time)
        {
            Agent.enabled = false;
            Obstacle.enabled = true;
        }
    }

    public bool SetDestination(Vector3 Position)
    {
        Obstacle.enabled = false;

        LastMoveTime = Time.time;
        LastPosition = transform.position;

        Agent.enabled = true;
        if (Agent.SetDestination(Position)) return true; ;
        return false;
        
    }


    public bool Reached()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            return true;
        }
        return false;

    }

    public float GetSpeed()
    {
        return Agent.speed;
    }
}