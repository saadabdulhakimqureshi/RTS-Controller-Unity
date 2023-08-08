using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera Camera;
    void Start()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera != null) 
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position);
    }
}
