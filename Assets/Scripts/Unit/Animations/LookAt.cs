using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Target;
    [SerializeField] MultiAimConstraint[] Aims;

    public float targetWeight = 0.7f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aim(Transform gO)
    {
        Target.transform.position = gO.transform.position;
        foreach(MultiAimConstraint multiAimConstraint in Aims)
        {
            multiAimConstraint.weight = targetWeight;
        }
    }

    public void Default()
    {

        foreach (MultiAimConstraint multiAimConstraint in Aims)
        {
            multiAimConstraint.weight = 0f;
            multiAimConstraint.weight = 0f;
        }
    }
}
