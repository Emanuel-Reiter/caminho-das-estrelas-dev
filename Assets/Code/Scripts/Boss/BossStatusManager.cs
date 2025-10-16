using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossStatusManager : MonoBehaviour {

    private float currentHP;
    [SerializeField] private float MaxHP = 500.0f;

    private EnemyStateManager stateManager;

    [SerializeField] private Collider hitbox;

    private float ivunerabilityTime;
    private float ivunerabilityTimeMax = 0.333f;

    [HideInInspector] public int hitsTaken { get; private set; } = 0;

    [HideInInspector] public bool hasDied { get; private set; } = false;

    public ParticleSystem defaultEffect;
    [SerializeField] private ParticleSystem hitEffect;
    public ParticleSystem dieEffect;

    public Material enemyBodyBaseMaterial;
    private Material enemyBodyMaterial;

    public Material enemyFaceBaseMaterial;
    private Material enemyFaceMaterial;

    public Material enemySwordBaseMaterial;
    private Material enemySwordMaterial;

    [SerializeField] private SkinnedMeshRenderer enemyMesh;
    [SerializeField] private MeshRenderer enemySwordMesh;

    [SerializeField] public Slider bossHealthBar;

    private void Start() {
        currentHP = MaxHP;

        stateManager = GetComponent<EnemyStateManager>();
        ivunerabilityTime = ivunerabilityTimeMax;

        UpdateUI();
    }

    private void Update() {
        ivunerabilityTime -= Time.deltaTime;
    }

    public void TakeDamage(float damage) {
        if (ivunerabilityTime < 0.0f) {
            hitsTaken++;
            currentHP -= damage;
            
            SpawnHitEffect();

            ivunerabilityTime = ivunerabilityTimeMax;

            UpdateUI();

            if (currentHP <= 0.0f) hasDied = true;
        }
    }

    public void UpdateUI() {
        bossHealthBar.value = currentHP / MaxHP;
    }

    private void SpawnHitEffect() {
        Vector3 newRotation = new Vector3(0.0f, Random.Range(-180.0f, 180.0f), 0.0f);
        hitEffect.transform.eulerAngles = newRotation;
        hitEffect.Play();
    }

    public void Die() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("EndingScene");
        SceneManager.UnloadSceneAsync("DevelopmentScene");
    }

    public void DeactivateColliders() {
        hitbox.enabled = false;
    }

    public void DissolveEnemy(float dissolvePercent) {
        dissolvePercent = Mathf.Clamp(dissolvePercent, 0.0f, 1.0f);
        enemyBodyMaterial.SetFloat("_Opacity", dissolvePercent);
        enemyFaceMaterial.SetFloat("_Opacity", dissolvePercent);
        enemySwordMaterial.SetFloat("_Opacity", dissolvePercent);
    }

    public void ResetStagger() {
        hitsTaken = 0;
    }
}
