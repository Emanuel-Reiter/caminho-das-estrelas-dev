using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    //movement
    [HideInInspector] public float runSpeed { get; private set; } = 5.0f;
    [HideInInspector] public float dashSpeed { get; private set; } = 40.0f;

    [HideInInspector] public float jumpHeight { get; private set; } = 0.833f;

    [HideInInspector] public float dashCooldown { get; private set; }
    [HideInInspector] public float dashCooldownMax { get; private set; } = 1.5f;

    //status
    [HideInInspector] public float healthMax { get; private set; } = 130.0f;
    [HideInInspector] public float currentHealth { get; private set; }

    [HideInInspector] public float lightAttackDamage { get; private set; } = 20.0f;

    private PlayerStatusManager statusManager;

    public int hpLevel;
    public int damageLevel;
    public int dashLevel;

    private void Awake() {
        currentHealth = healthMax;
    }

    private void Start() {
        statusManager = GetComponent<PlayerStatusManager>();

        dashCooldown = dashCooldownMax;

        hpLevel = 1;
        damageLevel = 1;
        dashLevel = 1;
    }

    private void Update() {
        dashCooldown += Time.deltaTime;
    }

    public void IncreaseMaxHealth(float amountMultiplaier) {
        amountMultiplaier = Mathf.Clamp(amountMultiplaier, 1.0f, 1000.0f);
        healthMax *= amountMultiplaier;

        hpLevel++;

        float previousHealth = currentHealth;
        float healthToAdd = (currentHealth * amountMultiplaier) - previousHealth;
        IncreaseHealth(healthMax);

        statusManager.UpdatePlayerStatusHUD();
    }

    public void IncreaseHealth(float amount) {
        amount = Mathf.Clamp(amount, 0.0f, 1000.0f);
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, healthMax);

        statusManager.UpdatePlayerStatusHUD();
    }

    public void DecreaseHealth(float amount) {
        amount = Mathf.Clamp(amount, 0.0f, 1000.0f);
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0.0f, healthMax);

        statusManager.UpdatePlayerStatusHUD();
    }

    public void IncreaseDamage(float amountMultiplaier) {
        amountMultiplaier = Mathf.Clamp(amountMultiplaier, 1.0f, 1000.0f);
        lightAttackDamage *= amountMultiplaier;

        damageLevel++;

        statusManager.UpdatePlayerStatusHUD();
    }

    public void DecreaseDashMaxCooldown(float amountMultiplaier) {
        amountMultiplaier = Mathf.Clamp(amountMultiplaier, 0.1f, 1000.0f);
        dashCooldownMax -= dashCooldownMax * amountMultiplaier;
        dashCooldownMax = Mathf.Clamp(dashCooldownMax, 0.1f, 1000.0f);

        dashLevel++;

        statusManager.UpdatePlayerStatusHUD();
    }
    public void ResetDashCooldown() { dashCooldown = 0.0f; }

    public float GetDashCooldown() { return dashCooldown; }
    public float GetDashCooldownMax() { return dashCooldownMax; }


}
