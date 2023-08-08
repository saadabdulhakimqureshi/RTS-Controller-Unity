using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
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
        BodyPart bodyPart= other.GetComponent<BodyPart>();
        if (bodyPart != null)
        {
            Unit unit = bodyPart.Unit;
            if (unit != null)
            {
                unit.RefillHealth();
                Destroy(gameObject);
            }
        }
    }
}
