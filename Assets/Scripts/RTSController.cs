using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public int power;

    public Camera mainCamera;
    public NavMeshAgent agent;
    public GameObject healthBar;
    public GameObject muzzleFlash;

    public Animator animator;

    private bool isMoving = false;
    private bool isShooting = false;

    public GameObject destroying;
    // Update is called once per frame
   
    void Update()
    {
        CurrentOrders();
    }

    public void CurrentOrders()// Checks if a unit is currently moving or shooting and performs the necessary actions.
    {
        if (isMoving)
        {
            CheckIfReached();
        }

        if (isShooting)
        {
            //Debug.Log("Shooting");
            CheckIfDestroyed();
        }
    }

    public void CheckIfReached()
    {
        float speed = agent.velocity.magnitude;
        //Debug.Log("Current Speed: " + speed);
        animator.SetFloat("walkingSpeed", speed * 0.4f);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isMoving = false;
            // Agent has stopped moving, stop the animation
            AnimateMovement();   
        }
    }

    public void CheckIfDestroyed()
    {
        if (destroying != null)
        {
            destroying.GetComponent<Destroyable>().LoseHealth(power);
        }
        else
        {
            isShooting = false;
            AnimateShooting();
        }
    }

    public void AnimateMovement()
    {
        animator.SetBool("isWalking", isMoving);
    }

    public void AnimateShooting()
    {
        muzzleFlash.SetActive(isShooting);
        animator.SetBool("isShooting", isShooting);
    }

    public void EnableHealthBar()
    {
        healthBar.SetActive(true);
    }

    public void DisableHealthBar()
    {
        healthBar.SetActive(false);
    }

    public void OrderUnit(bool click = true, string command = "")
    {
        if (click)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                // Move our agent.
                if (hit.collider.tag == "Destroyable")
                {
                    AttackObject(hit);
                }
                else if (hit.collider.tag == "Wall")
                {
                    StopUnit();
                }
                if (hit.collider.tag == "Ground")
                {
                    MoveUnit(hit);
                }


            }
        }
        else
        {
            if (command == "Stop")
            {
                StopUnit();
            }
            
        }
    }

    private void MoveUnit(RaycastHit hit)
    {
        if (agent.SetDestination(hit.point))
        {
            Debug.Log("Walking");
            isMoving = true;
            isShooting = false;
            AnimateMovement();
            AnimateShooting();
        }
    }

    private void AttackObject(RaycastHit hit)
    {
        agent.SetDestination(agent.transform.position);
        Vector3 direction = hit.point - agent.transform.position;
        direction.y = 0f;


        agent.transform.rotation = Quaternion.LookRotation(direction);
        isShooting = true;
        AnimateShooting();

        destroying = hit.collider.gameObject;
    }

    private void StopUnit()
    {
        isMoving = false;
        isShooting = false;
        AnimateMovement();
        AnimateShooting();
        agent.SetDestination(agent.transform.position);
    }
}
