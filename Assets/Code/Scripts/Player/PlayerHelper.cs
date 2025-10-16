using UnityEngine;

public class PlayerHelper : MonoBehaviour {

    //animation references
    [HideInInspector] public float animationSmoothTimeQuadruple { get; private set; } = 0.333f;
    [HideInInspector] public float animationSmoothTimeDouble { get; private set; } = 0.167f;
    [HideInInspector] public float animationSmoothTimeFull { get; private set; } = 0.083f;
    [HideInInspector] public float animationSmoothTimeHalf { get; private set; } = 0.041f;

    [HideInInspector] public string idleAnimation { get; private set; } = "Idle";
    [HideInInspector] public string runAnimation { get; private set; } = "Run";
    [HideInInspector] public string dashAnimation { get; private set; } = "Dash";
    [HideInInspector] public string jumpAnimation { get; private set; } = "Jump";
    [HideInInspector] public string fallAnimation { get; private set; } = "Jump";
    [HideInInspector] public string attackLightPreparationAnimation { get; private set; } = "AttackLightPreparation";
    [HideInInspector] public string[] attackLightAnimation { get; private set; } = { "AttackLight0", "AttackLight1" };

    //attack variables
    [HideInInspector] public int currentCombo { get; private set; }
    [HideInInspector] public float comboResetTime { get; private set; }
    [HideInInspector] public float comboResetTimeMax { get; private set; } = 0.333f;

    [HideInInspector] public bool isPerformingAttack { get; private set; } = false;
    [HideInInspector] public bool queueAttack { get; private set; } = false;
    [HideInInspector] public bool changeAttackDirection { get; private set; } = false;
    [HideInInspector] public bool propelAttackForward { get; private set; } = false;

    public AnimationClip[] attackLightAnimationDuration;
    public AnimationClip attackLightPreparationAnimationDuration;

    public AnimationClip dashAnimationDuration;
    [HideInInspector] public bool isDashing { get; private set; } = false;
    [HideInInspector] public bool isPerformingDash { get; private set; } = false;

    //movement variables
    [HideInInspector] public float airControlPercentage { get; private set; } = 0.1f;

    private PlayerStateManager player;
    private PlayerStatus status;

    [HideInInspector] public float currentSpeed;

    [HideInInspector] public float rotationSmoothTime = 0.05f;
    [HideInInspector] public float rotationSmoothVelocity;

    [HideInInspector] public float speedSmoothTime = 0.1f;
    [HideInInspector] public float speedSmoothVelocity;

    [HideInInspector] public float velocityX;
    [HideInInspector] public float velocityY;

    [HideInInspector] public const float gravity = -9.81f;
    [HideInInspector] public float currentGravity;
    [HideInInspector] public float gravityMultiplaier = 1.0f;

    //temporary references
    [SerializeField] private ParticleSystem voidTrailsEffect;
    [SerializeField] private ParticleSystem voidDeepTrailsEffect;

    //attack hitbox variables
    private float dealDamageCooldown;
    private float maxDealDamageCooldown = 0.333f;

    [SerializeField] private Vector3 hitboxSize;
    [SerializeField] private Transform hitOrigin;

    [HideInInspector] public bool hasIvunerability{ get; private set; } = false;

    //SFX
    public GameObject voidSwordSFX;

    [HideInInspector] public bool queueDash { get; private set; } = false;


    private void Start() {
        player = GetComponent<PlayerStateManager>();
        status = GetComponent<PlayerStatus>();

        gravityMultiplaier = 4.0f;
        currentGravity = gravity * gravityMultiplaier;
        dealDamageCooldown = maxDealDamageCooldown;
    }

    private void Update() {
        dealDamageCooldown -= Time.deltaTime;

        if (!isPerformingAttack) comboResetTime -= Time.deltaTime;
        comboResetTime = Mathf.Clamp(comboResetTime, 0.0f, comboResetTimeMax);
        if (comboResetTime <= 0.005f) currentCombo = 0;
    }

    public float GetModifiedSmoothTime(float smoothTime) {
        if (player.controller.isGrounded) return smoothTime;

        if (airControlPercentage == 0.0f) return float.MaxValue;

        return smoothTime / airControlPercentage;
    }

    public void Move(Vector3 velocity) {
        player.controller.Move(velocity * Time.deltaTime);
    }

    public void SetHasIvunerability(bool value) { hasIvunerability = value; }

    public void AttackEnemy() {
        Collider[] objectsInRange = Physics.OverlapBox(hitOrigin.position, hitboxSize);

        for (int i = 0; i < objectsInRange.Length; i++) {
            if (objectsInRange[i].gameObject != this.gameObject) {
                if (objectsInRange[i].CompareTag("Enemy")) {
                    EnemyStatusManager enemy = objectsInRange[i].gameObject.GetComponent<EnemyStatusManager>();
                    enemy.TakeDamage(status.lightAttackDamage);
                }
                if (objectsInRange[i].CompareTag("Boss")) {
                    BossStatusManager enemy = objectsInRange[i].gameObject.GetComponent<BossStatusManager>();
                    enemy.TakeDamage(status.lightAttackDamage);
                }
            }
        }
    }

    public void ToggleSwordTrail(bool toggle) {

        if (toggle) {
            voidDeepTrailsEffect.Play();
            voidTrailsEffect.Play();
        }
        else {
            voidDeepTrailsEffect.Stop();
            voidTrailsEffect.Stop();
        }
    }
    public void ResetComboTime() { comboResetTime = comboResetTimeMax; }
    public void SetPerformingAttack( bool attaking) { isPerformingAttack = attaking; }
    public void SetCurrentCombo(int combo) {
        if (combo >= attackLightAnimationDuration.Length + 1) combo = 1;
        currentCombo = combo;
    }
    public void SetQueueAttack(bool queue) { queueAttack = queue; }
    public void SetQueueDash(bool queue) { queueDash = queue; }

    public void SetChangeAttackDirection(bool changeDirection) { changeAttackDirection = changeDirection; }
    public void SetPropelAttackForward(bool propelAttack) { propelAttackForward = propelAttack; }

    public void SetIsDashing(bool value) { isDashing = value; }
    public void SetIsPerformingDashing(bool value) { isPerformingDash = value; }

    public void PlaySwordSlashSFX() { Instantiate(voidSwordSFX, transform.position, Quaternion.identity); }

    public void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(hitOrigin.position, hitboxSize); 
    }
}
