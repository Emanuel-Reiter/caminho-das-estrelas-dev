using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossStateManager : MonoBehaviour {

    private BossBaseState currentState;
    private BossBaseState previousState;

    public BossIdleState idleState = new BossIdleState();
    public BossChaseState chaseState = new BossChaseState();
    public BossSlowComboState slowComboState = new BossSlowComboState();
    public BossLongComboState longComboState = new BossLongComboState();
    public BossDieState dieState = new BossDieState();
    public BossCastSpellState castSpellState = new BossCastSpellState();
    public BossExplosionState explosionState = new BossExplosionState();

    [HideInInspector] public BossHelper helper;
    [HideInInspector] public BossStatusManager status;
    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform playerLocation;

    private void Awake() {
        helper = GetComponent<BossHelper>();
        status = GetComponent<BossStatusManager>();
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerLocation = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
    }
    private void Start() {
        currentState = idleState;
        previousState = idleState;
        idleState.EnterState(this);
    }

    private void Update() {
        currentState.CheckExitState(this);
        currentState.UpdateState(this);
    }

    private void FixedUpdate() {
        currentState.UpdatePhysicsState(this);
    }

    public void SwitchState(BossBaseState state) {
        currentState.ExitState(this);
        previousState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }
}
