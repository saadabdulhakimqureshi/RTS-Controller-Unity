using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    // Start is called before the first frame update
    public RTSController RTSController;
    public Unit Unit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup( RTSController rtscontroller, Unit unitPtr, string type)
    {
        RTSController = rtscontroller;
        Unit = unitPtr;
        tag = type;
    }

    public void LoseHealth(float attackDamage)
    {
        RTSController.TakeDamage(attackDamage);
        
    }
}
