using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitCommand : MonoBehaviour
{
    // Start is called before the first frame update
    public UnitStateMachine UnitStateMachine;



    void Start()
    {
        UnitStateMachine.ChangeToIdle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OrderUnit(RaycastHit hit, Vector3 moveLocation, string command = "")// When a player gives a command we stop the previous command issued to the unit.
    {
        /*Debug.Log(command);
        Debug.Log(hit.collider.tag);*/
        if (command == "Attack")// If the player clicks on an destroyable object we issue the attack command.
        {
            StartCoroutine(UnitStateMachine.ChangeToChase(hit.collider.gameObject));
            
        }
        if (command == "Move")// If the player clicks on a walkable plain we issue the move command.
        {
            StartCoroutine(UnitStateMachine.ChangeToMove(moveLocation));
        }

        if (command == "Stop")
        {
            UnitStateMachine.ChangeToIdle();
        }


    }


}
