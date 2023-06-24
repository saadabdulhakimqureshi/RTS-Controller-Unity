using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour// This class can be inhertited to any different type of rts unit.
{
    // Start is called before the first frame update
    [Header("References")]
    public GameObject HealthBar;
    public GameObject MuzzleFlash;
    public GameObject DetectionSphere;

    public ObstacleAgent ObstacleAgent;
    
    public Animator Animator;
    
    public Transform UnitSpawnTransform;
    public Transform VisionRaySpawnTransform;

    public Transform PathTransform;
    [Header("Variables")]
    public string type;

    public float soldierHealth;
    public float capoHealth;
    public float underbossHealth;
    public float donHealth;
    public float soldierAttackDamage;
    public float capoAttackDamage;
    public float underbossAttackDamage;
    public float donAttackDamage;


    [Header("Prefabs")]
    public GameObject Soldier;
    public GameObject Capo;
    public GameObject UnderBoss;
    public GameObject Don;

    // Private Variables
    public GameObject destroying;

    public RaycastHit enemyHit;

    Unit unitPtr;

    public bool isMoving = true;
    private bool isShooting = false;
    private bool isNotInRange = false;
    private bool playerDetected = false;
    public List<Transform>steps;
    public int currentStep;
    public int maximumStep;
    // Update is called once per frame

    private void Start()
    {
        isShooting = false;
        isMoving = true;
        isNotInRange = true;

        if (type == "Soldier")
        {
            unitPtr = new Soldier(soldierHealth, soldierAttackDamage);
            Soldier.SetActive(true);
            Animator = Soldier.GetComponent<Animator>();
        }
        if (type == "Capo")
        {
            
        }
        if (type == "Underboss")
        {
            
        }
        if (type == "Associate")
        {
            
        }

        foreach(Transform step in PathTransform)
        {
            if (step != null)
            {
                steps.Add(step);
            }
        }
        maximumStep = PathTransform.childCount;
    }
    public void Update()
    {
        Navigate(); UpdateAnimations();
    }

    public void Navigate()
    {
        if (!playerDetected)
        {
            if (ObstacleAgent.SetDestination(steps[currentStep].position))
            {
                if (ObstacleAgent.Reached())
                {
                    currentStep++;

                    if (currentStep > maximumStep)
                    {
                        currentStep = 0;
                    }
                }
            }

        }

    }

    public void UpdateAnimations()
    {
        Animator.SetBool("isWalking", isMoving);
    }
    public void TakeDamage(float amount)
    {
        unitPtr.TakeDamage(amount);
    }

    public void AttackPlayer(GameObject target)
    {
        unitPtr.Attack(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Health")
        {
            unitPtr.RefillHealth();
        }
    }

}
