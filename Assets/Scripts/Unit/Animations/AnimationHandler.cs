using HutongGames.PlayMaker.ActionsInternal;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class AnimationHandler : MonoBehaviour
{
    public UnitAI UnitAI;
    public UnitStateMachine UnitStateMachine;
    public NavMeshAgent NavMeshAgent;
    public Animator Animator;
    public Transform pursuing;
    public LookAt LookAt;
    public float turnThreshold = 0.3f;

    [Header("Animator Paramaters")]
    public bool changingAnimation;
    public string currentState;
    public string Idle = "Idle";
    public string Moving = "Run";
    public string Firing = "Fire";
    public string Searching = "Idle";
    public string Roaming = "Run";
    public string Chasing = "Run";
    public string TakingCover = "Run";
    /*    public bool isRunning = false;
        public bool isFiring = false;
        public bool isRunningLeft = false;
        public bool isRunningRight = false;
        public bool isTurningLeft = false;
        public bool isTurningRight = false;*/


    private void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        changingAnimation = false;
    }

/*    public void SetRunning(bool running)
    {
        isRunning = running;
    }*/
    public void SetAnimator(Animator animator)
    {
        Animator = animator;
    }
    public void SetLookAt(LookAt lookAt)
    {
        LookAt = lookAt;
    }
    public void SetPursuing(Transform pursuing)
    {
        this.pursuing = pursuing;
    }

/*    public void SetFiring(bool firing)
    {
        isFiring = firing;
    }*/

    private void Update()
    {
        UpdateAnimations();
        UpdateVariables();
    }

    private void UpdateAnimations()
    {
        /*Animator.SetBool("IsRunning", isRunning);
        Animator.SetBool("IsFiring", isFiring);*/
        
        Animator.Play(currentState);
    }

    void PlayChangingAnimation(UnitStateMachine.State previousState, UnitStateMachine.State nextState)
    {
        //ChangeAnimationState();
    }

    void ChangeAnimationState(string state)
    {
        currentState = state;
    }

    private void UpdateVariables()
    {

        switch (UnitStateMachine.CurrentState())
        {
            case UnitStateMachine.State.Idle:
                {
                    //Debug.Log("Idle");
                    /*isFiring = false;
                    isRunning = false;*/
                    ChangeAnimationState(Idle);
                    LookAt.Default();
                    break;
                }


            case UnitStateMachine.State.Moving:
                {
                    //Debug.Log("Moving");
                    /*isFiring = false;
                    isRunning = true;*/
                    ChangeAnimationState(Moving);
                    LookAt.Default();
                    break;
                }

            case UnitStateMachine.State.Firing:
                {
                    //Debug.Log("Firing");
                    ChangeAnimationState(Firing);
                    if (pursuing != null)
                    {
                        LookAt.Aim(pursuing);
                    }
                    /*isFiring = true;
                    isRunning = false;*/
                    break;
                }

            case UnitStateMachine.State.Searching:
                {
                    //Debug.Log("Searching");
                    /*isFiring = false;
                    isRunning = false;*/
                    ChangeAnimationState(Searching);
                    LookAt.Default();
                    break;
                }

            case UnitStateMachine.State.Chasing:
                {
                    //Debug.Log("Chasing");
                    /*isFiring = false;
                    isRunning = true;*/
                    ChangeAnimationState(Chasing);
                    if (pursuing != null)
                    {
                        LookAt.Aim(pursuing);
                    }
                    break;
                }

            case UnitStateMachine.State.TakingCover:
                {
                    //Debug.Log("Chasing");
                    /*isFiring = false;
                    isRunning = true;*/
                    ChangeAnimationState(TakingCover);
                    if (pursuing != null)
                    {
                        LookAt.Aim(pursuing);
                    }
                    break;
                }

            case UnitStateMachine.State.Roaming:
                {
                    //Debug.Log("Roaming");
                    /*isFiring = false;
                    isRunning = true;*/
                    ChangeAnimationState(Roaming);
                    LookAt.Default();
                    break;
                }

            case UnitStateMachine.State.Dying:
                {
                    break;
                }
        }
    }
}