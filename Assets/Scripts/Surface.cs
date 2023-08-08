using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Surface : MonoBehaviour
{
    public NavMeshSurface NavMeshSurface;

    public float AvoidancePredictionTime = 2;
    public int PathfindingIterationsPerFrame = 100;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshSurface = GetComponent<NavMeshSurface>();
        NavMeshSurface.BuildNavMesh();
        NavMesh.avoidancePredictionTime = AvoidancePredictionTime;
        NavMesh.pathfindingIterationsPerFrame = PathfindingIterationsPerFrame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
