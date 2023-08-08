using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour// This class can be inhertited to any different type of rts unit.
{
    // Start is called before the first frame update
    [Header("References")]
    public GameObject HealthBar;

    public AnimationHandler AnimationHandler;

    public Transform UnitSpawnTransform;
    public Transform VisionRaySpawnTransform;
    public Transform TargetTransform;

    public UnitAI UnitAI;
    public UnitCommand UnitCommand;
    public UnitStateMachine UnitStateMachine;

    [Header("Character Variables")]
    public string type;
    public bool alive;


    [Header("Character Prefabs")]
    public GameObject Soldier;
    public GameObject Capo;
    public GameObject UnderBoss;
    public GameObject Don;
    private GameObject SpawnedCharacter;

    // Private Variables
    public GameObject destroying;

    private Unit unitPtr;

    private bool isNotInRange;

    private RaycastHit enemyHit;

    private void Start()
    {
        if (tag == "Player")
        {
            UnitCommand.enabled = true;
            UnitAI.enabled = false;
        }

        if (tag == "Enemy")
        {
            UnitCommand.enabled = false;
            UnitAI.enabled = true;
        }

        if (type == "Soldier")
        {
            
            SpawnedCharacter = Instantiate(Soldier, UnitSpawnTransform.position, Quaternion.identity);
            SpawnedCharacter.transform.parent = gameObject.transform;
            unitPtr = SpawnedCharacter.GetComponent<Unit>();
            AnimationHandler.SetAnimator(SpawnedCharacter.GetComponent<Animator>());
            AnimationHandler.SetLookAt(SpawnedCharacter.GetComponent<LookAt>());
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

        alive = true;
        RagdollOff();
    }
    void Update()
    {

        if (unitPtr.CheckHealth())
        {
            Death();
        }

    }

    void RagdollOff()
    {
        Rigidbody[] rigidbodies = SpawnedCharacter.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.tag = "BodyPart";
            rigidbody.isKinematic = true;
            BodyPart bodyPart = rigidbody.GetComponent<BodyPart>();
            if (bodyPart != null)
            {
                bodyPart.Setup(this, unitPtr, tag);
            }
        }

        

        Collider[] colliders = SpawnedCharacter.GetComponentsInChildren<Collider>();
        SpawnedCharacter.GetComponent<Animator>().enabled = true;
    }

    void RagdollOn()
    {
        SpawnedCharacter.GetComponent<Animator>().enabled = false;
        Rigidbody []
        rigidbodies = SpawnedCharacter.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.tag = "BodyPart";
            rigidbody.isKinematic = false;
        }
    }
    
    public void EnableHealthBar()// When a player is selected we enable the health bar.
    {
        HealthBar.SetActive(true);
    }

    public void DisableHealthBar()// When a player is deselected we disable the health bar.
    {
        HealthBar.SetActive(false);
    }


    public void Attack(Transform attacking)
    {
        unitPtr.Attack(attacking, tag);
    }

    public void TakeDamage(float attackDamage)
    {
        unitPtr.TakeDamage(attackDamage);
    }

    void Death()
    {
        RagdollOn();
        alive = false;
        GetComponent<NavMeshObstacle>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<ObstacleAgent>().enabled = false;
        HealthBar.SetActive(false);
        UnitStateMachine.enabled = false;
        Destroy(gameObject, 10f);
    }
}
