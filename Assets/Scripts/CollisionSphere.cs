using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSphere : MonoBehaviour
{
    public List<GameObject> rangeList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)// Adding all objects that are close to player.
    {
        //Debug.Log(other.tag);
        rangeList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)// Removing all objects that are not close to player.
    {
        rangeList.Remove(other.gameObject);
    }
}
