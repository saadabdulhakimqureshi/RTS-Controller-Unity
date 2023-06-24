using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour// This class can be inhertited to any different type of rts unit.
{
    // Start is called before the first frame update
    [Header("References")]
    public Camera mainCamera;
    public ObstacleAgent agent;
    //public NavMeshObstacle Obstacle;
    public GameObject healthBar;
    public GameObject muzzleFlash;
    public GameObject CollisionSphere;
    public Animator Animator;
    public Transform bulletSpawnTransform;
    public NavMeshSurface surface;

    [Header("Variables")]
    public int health;
    public int power;

    
    // Private Variables
    private bool isMoving = false;
    private bool isShooting = false;
    private bool isNotInRange;
    public GameObject destroying;
    public RaycastHit enemyHit;
    // Update is called once per frame

    private void Start()
    {
        isShooting = false;
        isMoving = false;
        isNotInRange = true;
    }
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
            //Debug.Log(isNotInRange);
            if (isNotInRange) { // While the player is moving we stop them only when the destroyable object is within range.
                CheckIfInRange(); 
                
            }
            else // When the player can shoot at the game object we call the function to lose their health.
            {
                DestroyEnemy();
            }

        }
    }

    public void CheckIfReached()// Checking if the player has reached the destination then stopping him.
    {
        float speed = agent.GetSpeed();
        Animator.SetFloat("walkingSpeed", speed * 0.4f);
        if (agent.Agent.enabled == false)
        {
            isMoving = false;
            AnimateMovement();
        }
    }

    public void CheckIfInRange()// / Checking if the destroyable object is within the player's range.
    {
        List<GameObject> rangeList = CollisionSphere.GetComponent<CollisionSphere>().rangeList;

        foreach (GameObject gameObject in rangeList)
        {
            if (gameObject == destroying)
            {
                //Debug.Log("Koi Milgaya");

                
                Vector3 direction = enemyHit.point - agent.transform.position;
                agent.transform.rotation = Quaternion.LookRotation(direction);
                RaycastHit bulletHit;
                
                Vector3 origin = bulletSpawnTransform.position;
                direction = enemyHit.point - origin;
                if (Physics.Raycast(origin, direction, out bulletHit))
                {
                    if (bulletHit.collider.gameObject == destroying)
                    {
                        agent.SetDestination(agent.transform.position);
                        isMoving = false;
                        AnimateShooting();
                        isNotInRange = false;
                    }                  
                }

                break;
            }
        }
    }

    public void DestroyEnemy()
    {
        if (destroying != null)
        {
            destroying.GetComponent<Destroyable>().LoseHealth(power);
        }
        else
        {
            StopUnit();
            AnimateShooting();
        }
    }

    public void AnimateMovement()// Updating player animations.
    {
        Animator.SetBool("isWalking", isMoving);
    }

    public void AnimateShooting()// Updating player animations.
    {
        muzzleFlash.SetActive(isShooting);
        Animator.SetBool("isShooting", isShooting);
    }

    public void EnableHealthBar()// When a player is selected we enable the health bar.
    {
        healthBar.SetActive(true);
    }

    public void DisableHealthBar()// When a player is deselected we disable the health bar.
    {
        healthBar.SetActive(false);
    }

    public void OrderUnit(RaycastHit hit, Vector3 moveLocation, string command = "")// When a player gives a command we stop the previous command issued to the unit.
    {
        StopUnit();

        if (command ==  "Attack")// If the player clicks on an destroyable object we issue the attack command.
        {
            AttackObject(hit);
        }
        if (command == "Move")// If the player clicks on a walkable plain we issue the move command.
        {
            MoveUnit(moveLocation);
        }

        if (command == "Stop")
        {
            StopUnit();
        }
            
        
    }

    private void MoveUnit(Vector3 movePosition)
    {
        if (agent.SetDestination(movePosition))// If the hit is a walkable point we move the agent towards that point.
        {
            //surface.BuildNavMesh();
            isNotInRange = true;
            isMoving = true;
            isShooting = false;
            AnimateMovement();
            AnimateShooting(); 
        }
    }

    private void AttackObject(RaycastHit hit)
    {
        List<GameObject> rangeList = CollisionSphere.GetComponent<CollisionSphere>().rangeList;

        foreach (GameObject gameObject in rangeList)// Checking if the hit point game object is within the player's range.
        {
            if (gameObject == hit.collider.gameObject)
            {
                
                agent.SetDestination(agent.transform.position);
                Vector3 direction = hit.point - agent.transform.position;
                agent.transform.rotation = Quaternion.LookRotation(direction);
                RaycastHit bulletHit;
                Debug.Log(hit.point);
                Vector3 origin = bulletSpawnTransform.position;
                direction = hit.point - origin;
                Debug.Log(direction);
                if (Physics.Raycast(origin, direction, out bulletHit))// Checking if the player can launch a bullet at the hit point.
                {
                    if (bulletHit.collider == hit.collider)// Checking if the bullet actually hits the intended object and not a obstacle in between.
                    {
                        Debug.Log("Ray hit object");

                        isMoving = false;
                        isShooting = true;
                        AnimateShooting();
                        destroying = hit.collider.gameObject;
                        enemyHit = hit;
                        isNotInRange = false;

                    }
                    
                }
                break;
            }
        }

        if (isNotInRange == true)// If the object is either not within the range or there is an obstacle in between we move the player close.
        {
            if (agent.SetDestination(hit.point))
            {
                //surface.BuildNavMesh();
                isMoving = true;
                isShooting = true;
                AnimateMovement();
                destroying = hit.collider.gameObject;
                enemyHit = hit;
            }
            
        }

        

    }

    private void StopUnit()// Stopping the current command given to player.
    {
        isNotInRange = true;
        isMoving = false;
        isShooting = false;
        AnimateMovement();
        AnimateShooting();
        agent.SetDestination(agent.transform.position);
    }

    public void TakeDamage(float attackDamage)
    {

    }
}
