using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    // Start is called before the first frame update
    public bool occupied;
    public Sphere CoverViewersSphere;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setOccupied(bool occupied)
    {
        this.occupied = occupied;
    }

    public bool isOccupied()
    {
        return occupied;
    }
}
