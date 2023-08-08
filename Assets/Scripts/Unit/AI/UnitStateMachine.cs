using Palmmedia.ReportGenerator.Core.Plugin;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitStateMachine: MonoBehaviour
{
    // Start is called before the first frame update
    [Header("References")]
    public ObstacleAgent ObstacleAgent;
    public AnimationHandler AnimationHandler;
    public Sphere UnitDetectionSphere;
    public Sphere UnitFireSphere;
    public Sphere UnitDangerBox;

    public GameObject Vision;

    public Transform PathTransform;

    public List<Transform> steps;
    public Transform Covers;
    public Cover currentCover;

    public Transform pursuing;

    public RTSController RTSController;

    public Unit Unit;

    

    [Header("Patrolling")]
    public int currentStep;
    public int maximumStep;

    [Header("State Machine Variables")]
    [SerializeField] private State currentState;
    [SerializeField] private State defaultState;
    private bool reload = false;
    public enum State
    {
        Idle,
        Moving,
        Roaming,
        Searching,
        Chasing,
        TakingCover,
        Firing,
        Dying
    }

    [Header("State Machine Delays")]
    public float IdleTransitionDelay;
    public float MoveTransitionDelay;
    public float RoamTransitionDelay;
    public float TakeCoverTransitionDelay;
    public float SearchTransitionDelay;
    public float ChaseTransitionDelay;
    public float FiringTransitionDelay;
    public float DyingTransitionDelay;

    public void ChangeState(State newState)
    {
        this.currentState = newState;
    }

    public State CurrentState()
    {
        return currentState;
    }

    public State DefaultState()
    {
        return defaultState;
    }

    void Start()
    {
        GetPath();
        pursuing = null;
        Unit = GetComponent<Unit>();    
        ChangeToDefault();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }


    public void CheckState()
    {
        if (currentState != State.Dying)
        {
            switch (CurrentState())
            {
                case State.Idle:
                    {
                        //Debug.Log("Idle");
                        Idle();
                        break;
                    }
                case State.Moving:
                    {
                        //Debug.Log("Moving");
                        Move();
                        break;
                    }
                case State.Firing:
                    {
                        //Debug.Log("Firing");
                        Fire();
                        break;
                    }

                case State.Searching:
                    {
                        //Debug.Log("Searching");
                        Search();
                        break;
                    }

                case State.Chasing:
                    {
                        //Debug.Log("Chasing");
                        Chase();
                        break;
                    }

                case State.TakingCover:
                    {
                        Debug.Log("Taking Cover");
                        TakeCover();
                        break;
                    }

                case State.Roaming:
                    {
                        //Debug.Log("Roaming");
                        Roam();
                        break;
                    }
            }
        }
        
    }

    public void GetPath()
    {
        foreach (Transform step in PathTransform)
        {
            if (step != null)
            {
                steps.Add(step);
            }
        }
        maximumStep = PathTransform.childCount;
    }

    void ChangeToDefault()
    {
        reload = true;
        if (defaultState == State.Idle)
        {
            ChangeToIdle();
        }

        if (defaultState == State.Roaming)
        {
            StartCoroutine(ChangeToRoam());
        }
    }

    public void ChangeToIdle()
    {
        ObstacleAgent.Stop();
        pursuing = null;
        
        ChangeState(State.Idle);
    }

    public IEnumerator ChangeToMove(Vector3 moveLocation)
    {
        yield return new WaitForSeconds(MoveTransitionDelay);
        pursuing = null;
        ObstacleAgent.SetDestination(moveLocation);
        ChangeState(State.Moving);
    }

    public IEnumerator ChangeToRoam()
    {
        yield return new WaitForSeconds(RoamTransitionDelay);
        pursuing = null;
        ObstacleAgent.SetDestination(steps[currentStep].position);
        ChangeState(State.Roaming);
    }

    public IEnumerator ChangeToTakeCover()
    {

        // Getting all covers.
        Collider[] covers = Physics.OverlapSphere(transform.position, 10f);
        foreach (Collider coverCollider in covers)
        {
            if (coverCollider != null)
            {
                Transform coverTransform = coverCollider.transform;
                Cover cover = coverCollider.GetComponent<Cover>();
                if (cover != null && !cover.isOccupied())
                {
                    
                    
                    bool found = true;
                    Vector3 coverPosition = coverTransform.position;
                    Sphere coverViewersSphere = cover.CoverViewersSphere; 
 

                    // Getting all units near cover.
                    foreach (GameObject viewer in coverViewersSphere.objectsInSphere)
                    {
                        if (viewer != null && viewer.tag == "Player")
                        {
                            
                            Vector3 unitPosition = viewer.transform.position;

                            Vector3 direction = (coverPosition - unitPosition).normalized;
                            RaycastHit hit;
                            // If cover is in sight of any player unit.
                            if (Physics.Raycast(unitPosition, direction, out hit))
                            {
                                if (hit.collider.tag == "Cover")
                                {
                                    found = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (found)
                    {
                        
                        
                        currentCover = cover;

                        break;
                    }
                }


            }
        }
        yield return new WaitForSeconds(TakeCoverTransitionDelay);
        if (currentCover != null) {
            Debug.Log("Change to taking cover.");
            SphereCollider unitFireSphereCollider = UnitFireSphere.GetComponent<SphereCollider>();
            SphereCollider unitDetectionSphereCollider = UnitDetectionSphere.GetComponent<SphereCollider>();

            unitFireSphereCollider.radius = unitDetectionSphereCollider.radius;
            currentCover.setOccupied(true);
            ObstacleAgent.SetDestination(currentCover.transform.position);
            ChangeState(State.TakingCover);
        }
    }

    public void CheckFire() 
    {
        Sphere dangerSphere = UnitDangerBox.GetComponent<Sphere>();
        foreach (GameObject gO in dangerSphere.objectsInSphere)
        {

            if (gO != null)
            {
                Bullet bullet = gO.GetComponent<Bullet>();
                if (bullet != null)
                {
                    //Debug.Log(gO.tag);
                    if (bullet.shooter == "Player")
                    {
                        //Debug.Log("Player Shooting");
                        StartCoroutine(ChangeToTakeCover());
                    }
                }
            }
        }
    }

    


    public IEnumerator ChangeToSearch()
    {
        yield return new WaitForSeconds(SearchTransitionDelay);
        ChangeState(State.Searching);
    }

    public IEnumerator ChangeToChase(GameObject chaseGameObject = null)
    {
        yield return new WaitForSeconds(ChaseTransitionDelay);

        if (chaseGameObject != null)
        {
            //Debug.Log(chaseGameObject.tag);
            pursuing = chaseGameObject.transform;
            
        }
        if (pursuing != null)
        {
            ObstacleAgent.SetDestination(pursuing.position);
            ChangeState(State.Chasing);
        }
        else
        {
            ChangeToDefault();
        }
    }

    public IEnumerator ChangeToFire()
    {
        yield return new WaitForSeconds(FiringTransitionDelay);
        AnimationHandler.SetPursuing(pursuing);
        ObstacleAgent.SetDestination(transform.position);
        ChangeState(State.Firing);
    }

    public IEnumerator ChangeToDying()
    {
        yield return new WaitForSeconds(DyingTransitionDelay);
        ObstacleAgent.Stop();
        ChangeState(State.Dying);
        pursuing = null;
    }

    public void Idle()
    {
        CheckObjectsNearEnemy();
    }

    public void Move()
    {
        if (!ObstacleAgent.IsMoving())
        {
            ChangeToIdle();
        }
    }



    public void Roam()
    {
        CheckObjectsNearEnemy();


        if (ObstacleAgent.Reached())
        {
            ObstacleAgent.SetDestination(steps[currentStep].position);
            currentStep += 1;

            if (currentStep >= maximumStep)
            {
                currentStep = 0;
            }
            StartCoroutine(ChangeToSearch());
        }

    }


    public void Search()
    {
        CheckObjectsNearEnemy();
        StartCoroutine(LookArround());
    }


    public void Chase()
    {
        if (CheckAlive())
        {
            CheckObjectsInRangeOfEnemy();
            if (tag == "Enemy")
                CheckObjectsNearEnemy();

            if (reload && pursuing != null)
            {
                Vector3 direction = (pursuing.position - Vision.transform.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(Vision.transform.position, direction, out hit, LayerMask.NameToLayer("RTS")) && UnitDetectionSphere.objectsInSphere.Contains(pursuing.gameObject))
                {
                    if (hit.collider.gameObject == pursuing.gameObject)
                    {
                        RTSController.Attack(pursuing);
                        reload = false;
                        StartCoroutine(Reload());
                    }
                   
                }
            }
        }
        else
        {
            ChangeToDefault();
        }
    }

    void TakeCover()
    {
/*        Debug.Log("Taking Cover");
        if (ObstacleAgent.IsMoving())
        {
            if (CheckAlive())
            {
                if (reload && pursuing != null)
                {
                    Vector3 direction = (pursuing.position - Vision.transform.position).normalized;
                    RaycastHit hit;
                    if (Physics.Raycast(Vision.transform.position, direction, out hit, LayerMask.NameToLayer("RTS")) && UnitDetectionSphere.objectsInSphere.Contains(pursuing.gameObject))
                    {
                        if (hit.collider.gameObject == pursuing.gameObject)
                        {
                            RTSController.Attack(pursuing);
                            reload = false;
                            StartCoroutine(Reload());
                        }
                    }
                }
            }
        }
        else
        {
            

            ChangeToDefault();
        }*/
    }
    public void Fire()
    {
        /*if (tag == "Enemy" && currentCover == null)
            CheckFire();*/

        if (pursuing != null)
        {
            if (CheckAlive())
            {
                //transform.LookAt(pursuing.position);

                CheckObjectsInRangeOfEnemy();
                if (reload)
                {
                    RTSController.Attack(pursuing);
                    reload = false;
                    StartCoroutine(Reload());
                }
                
            }
            else
            {
                ChangeToDefault();
            }
        }
        else
        { 
            ChangeToDefault();
        }
    }

    IEnumerator Reload()
    {
        
        yield return new WaitForSeconds(0.4f);

        reload = true;
    }

    IEnumerator LookArround()
    {
        yield return new WaitForSeconds(4f);

        ChangeToDefault();
    }

    bool CheckAlive()
    {
        if (pursuing != null)
        {
            if ((pursuing.GetComponent<RTSController>() != null && !pursuing.GetComponent<RTSController>().alive)|| (pursuing.GetComponent<Destroyable>() != null && !pursuing.GetComponent<Destroyable>().alive))
            {
                //Debug.Log("Target is dead");
                pursuing = null;
                return false;
            }
        }
        return true;
    }

    public void CheckObjectsNearEnemy()
    {
        if (pursuing != null)
        {
            if (!UnitDetectionSphere.objectsInSphere.Exists(o => o == pursuing.gameObject))
            {
                pursuing = null;
                ChangeToDefault();
            }
            else
            {
                if ((CurrentState() != State.Chasing) && (CurrentState() != State.Firing))
                {
                    StartCoroutine(ChangeToChase());
                }
            }
        }
        else
        {
            foreach (GameObject objectNearEnemy in UnitDetectionSphere.objectsInSphere)
            {
                if (objectNearEnemy != null)
                {
                    if ((objectNearEnemy.tag == "Player" && tag == "Enemy") || (objectNearEnemy.tag == "Enemy" && tag == "Player"))
                    {
                        if (objectNearEnemy.GetComponent<RTSController>() != null)
                        {
                            pursuing = objectNearEnemy.transform;
                            if (CheckAlive())
                            {
                                StartCoroutine(ChangeToChase());
                            }
                        }
                    }
                }
            }

            if (pursuing == null)
            {
                ChangeToDefault();
            }
        }
    }






    public void CheckObjectsInRangeOfEnemy()
    {
        if (pursuing != null)
        {
            if (UnitFireSphere.objectsInSphere.Exists(o => o == pursuing.gameObject))
            {
                Vector3 direction = (pursuing.position - Vision.transform.position).normalized;
                RaycastHit hit;
                if (Physics.Raycast(Vision.transform.position, direction, out hit))
                {
                    if ((hit.collider.gameObject == pursuing.gameObject))
                    {
                        StartCoroutine(ChangeToFire());
                    }
                }
            }
            else
            {
                StartCoroutine(ChangeToChase());
            }
        }
    }






}
