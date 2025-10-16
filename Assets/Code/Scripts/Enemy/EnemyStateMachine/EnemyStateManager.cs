using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour {

    private EnemyBaseState currentState;
    private EnemyBaseState lastState;

    public EnemyIdleState idleState = new EnemyIdleState();
    public EnemyPatrolState patrolState = new EnemyPatrolState();
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyAttackState attackState = new EnemyAttackState();
    public EnemyDieState dieState = new EnemyDieState();
    public EnemyFlintchState flintchState = new EnemyFlintchState();

    [HideInInspector] public EnemyHelper helper;
    [HideInInspector] public EnemyStatusManager status;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform playerLocation;
     
    public Material enemyMaterial;

    private void Awake() {
        helper = GetComponent<EnemyHelper>();
        status = GetComponent<EnemyStatusManager>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerLocation = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }
    private void Start() {
        currentState = idleState;
        lastState = idleState;
        idleState.EnterState(this);
    }

    private void Update() {
        currentState.CheckExitState(this);
        currentState.UpdateState(this);
    }

    private void FixedUpdate() {
        currentState.UpdatePhysicsState(this);
    }

    public void SwitchState(EnemyBaseState state) {
        currentState.ExitState(this);
        lastState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
}
