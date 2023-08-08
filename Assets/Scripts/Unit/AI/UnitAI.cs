using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAI : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitStateMachine UnitStateMachine;
    

    void Start()
    {
        UnitStateMachine.ChangeToIdle();
    }
}
