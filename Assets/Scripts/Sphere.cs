using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public List<GameObject> objectsInSphere = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        objectsInSphere.Add(other.gameObject);
        CleanList();
    }

    private void OnTriggerExit(Collider other)
    {
        objectsInSphere.Remove(other.gameObject);
        CleanList();
    }

    void CleanList()
    {
        List<GameObject> list = new List<GameObject>(objectsInSphere);
        foreach (GameObject obj in objectsInSphere)
        {
            if (list != objectsInSphere)
            {
                break;
            }
            if (obj == null)
            {
                objectsInSphere.Remove(obj);
            }
        }
    }
}
