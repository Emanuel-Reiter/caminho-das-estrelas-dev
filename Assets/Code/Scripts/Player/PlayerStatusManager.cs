using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStatusManager : MonoBehaviour {

    [Header("Player Status Texts")]
    public TMP_Text dashCooldownHudText;
    public TMP_Text damageHudText;

    [Header("Player Atribute Level Texts")]
    public TMP_Text hpLevelText;
    public TMP_Text damageLevelText;
    public TMP_Text dashLevelText;

    [Header("Player HP UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private PlayerHelper helper;
    private PlayerStatus status;

    private UpgradeAltarBehaviour currentAltar;

    [HideInInspector] public float dashCooldownTimer { get; private set; }
    [HideInInspector] public float dashCooldownTimerMax { get; private set; }


    private void Start () {
        helper = GetComponent<PlayerHelper>();
        status = GetComponent<PlayerStatus>();

        UpdatePlayerStatusHUD();
    }

    private void Update() {
        dashCooldownTimer = status.GetDashCooldown();
        dashCooldownTimerMax = status.GetDashCooldownMax();
    }

    public void UpdatePlayerStatusHUD() {
        healthSlider.value = status.currentHealth / status.healthMax;

        int tempHealth = (int)status.currentHealth;
        int tempHealthMax = (int)status.healthMax;
        healthText.text = tempHealth + " / " + tempHealthMax;

        hpLevelText.text = status.hpLevel.ToString();
        damageLevelText.text = status.damageLevel.ToString();
        dashLevelText.text = status.dashLevel.ToString();

        int temp = (int)status.lightAttackDamage;
        damageHudText.text = temp.ToString();
        dashCooldownHudText.text = status.dashCooldownMax.ToString("#.0") + " Segundos";
    }

    public void AddLife(float amount) {
        status.IncreaseHealth(amount);
        UpdatePlayerStatusHUD();
    }
    
    public void TakeDamage(float amount) {
        if(!helper.hasIvunerability) status.DecreaseHealth(amount);
        UpdatePlayerStatusHUD();
        if (status.currentHealth <= 0.0f) Die();
    }

    private void Die() {
        SceneManager.LoadScene("GameScene");
    }

    public void ResetDash() { status.ResetDashCooldown(); }

    public void SetUpgrade() {
        currentAltar.SetAltarAsUsed();
        currentAltar = null;
    }

    public void SetCurrentAltar(UpgradeAltarBehaviour altar) { currentAltar = altar; }
}
