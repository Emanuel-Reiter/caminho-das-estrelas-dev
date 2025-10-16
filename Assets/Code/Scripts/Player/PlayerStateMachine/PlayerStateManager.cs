using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    private PlayerBaseState currentState;
    private PlayerBaseState previousState;

    public IdleState idleState = new IdleState();
    public RunState runState = new RunState();
    public DashState dashState = new DashState();
    public SprintState sprintState = new SprintState();

    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();

    public AttackLightPreparationState attackLightPreparationState = new AttackLightPreparationState();
    public AttackLightState attackLightState = new AttackLightState();

    //other classes
    [HideInInspector] public PlayerInputManager input;
    [HideInInspector] public PlayerStatusManager status;
    [HideInInspector] public PlayerHelper helper;
    [HideInInspector] public Movement movement;

    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;

    [HideInInspector] public Camera mainCamera;

    private void Awake() {
        //utility initializations
        input = GetComponent<PlayerInputManager>();
        status = GetComponent<PlayerStatusManager>();
        helper = GetComponent<PlayerHelper>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        movement = GetComponent<Movement>();
        mainCamera = Camera.main;
    }

    private void Start() {
        //state initialization
        currentState = idleState;
        previousState = idleState;

        currentState.EnterState(this);
    }

    private void Update() {
        currentState.CheckExitState(this);
        currentState.UpdateState(this);
    }

    private void FixedUpdate() {
        currentState.UpdatePhysicsState(this);
    }

    public void SwitchState(PlayerBaseState state) {
        currentState.ExitState(this);
        previousState = currentState;
        currentState = state;
        currentState.EnterState(this);
    }

    private void OnGUI() {
        bool enableDebug = false;
        if (enableDebug) {
            GUI.skin.label.fontSize = 24;
            GUI.Label(new Rect(128, 256, 512, 32), "Current state: " + currentState.ToString());
            GUI.Label(new Rect(128, 288, 512, 32), "Attack depth: " + helper.currentCombo.ToString());
            GUI.Label(new Rect(128, 320, 512, 32), "Fall speed: " + helper.velocityY.ToString());
            GUI.Label(new Rect(128, 352, 512, 32), "Is grounded " + controller.isGrounded.ToString());
        }
    }
}
