using UnityEngine;

public class EnemyStatusManager : MonoBehaviour {

    private float currentHP;
    [SerializeField] private float MaxHP = 50.0f;

    private EnemyStateManager stateManager;

    //[SerializeField] private Collider headHitbox;
    [SerializeField] private Collider hitbox;

    private float ivunerabilityTime;
    private float ivunerabilityTimeMax = 0.25f;

    [HideInInspector] public int hitsTaken { get; private set; } = 0;
    [HideInInspector] public bool queueStagger { get; private set; } = false;
    [HideInInspector] public bool hyperArmor { get; private set; } = false;
    [HideInInspector] public bool hasDied { get; private set; } = false;

    [SerializeField] private ParticleSystem hitEffect;
    public ParticleSystem defaultEffect;
    public ParticleSystem dieEffect;

    public Material enemyBodyBaseMaterial;
    private Material enemyBodyMaterial;

    public Material enemyFaceBaseMaterial;
    private Material enemyFaceMaterial;

    public Material enemySwordBaseMaterial;
    private Material enemySwordMaterial;

    [SerializeField] private SkinnedMeshRenderer enemyMesh;
    [SerializeField] private MeshRenderer enemySwordMesh;

    private void Start() {
        currentHP = MaxHP;

        stateManager = GetComponent<EnemyStateManager>();
        ivunerabilityTime = ivunerabilityTimeMax;

        enemyBodyMaterial = new Material(enemyBodyBaseMaterial);
        enemyFaceMaterial = new Material(enemyFaceBaseMaterial);

        Material[] enemyBodyMaterials = { enemyBodyMaterial, enemyFaceMaterial };
        enemyMesh.materials = enemyBodyMaterials;

        enemySwordMaterial = new Material(enemySwordBaseMaterial);
        enemySwordMesh.material = enemySwordMaterial;
    }

    private void Update() {
        ivunerabilityTime -= Time.deltaTime;
    }

    public void TakeDamage(float damage) {
        if (ivunerabilityTime < 0.0f) {
            if(!hyperArmor) hitsTaken++;
            currentHP -= damage;
            SpawnHitEffect();
            ivunerabilityTime = ivunerabilityTimeMax;
            if (currentHP <= 0.0f) hasDied = true;
        }
    }

    private void SpawnHitEffect() {
        Vector3 newRotation = new Vector3(0.0f, Random.Range(-180.0f, 180.0f), 0.0f);
        hitEffect.transform.eulerAngles = newRotation;
        hitEffect.Play();
    }

    public void SetHyperArmor(bool hasHyperArmor) { hyperArmor = hasHyperArmor; }

    private void DropItem() {
        int dropItem = Random.Range(1, 4);
        if (dropItem == 3) {
            int dropSelector = Random.Range(1, 4);
        }
    }

    public void QueueStagger(bool queue) { queueStagger = queue; }

    public void Die() {
        DropItem();
        stateManager.enabled = false;
        Destroy(this.gameObject);
    }

    public void SetQueueAttack(bool queue) { queueStagger = queue; }

    public void DeactivateColliders() {
        hitbox.enabled = false;
    }

    public void DissolveEnemy(float dissolvePercent) {
        dissolvePercent = Mathf.Clamp(dissolvePercent, 0.0f, 1.0f);
        enemyBodyMaterial.SetFloat("_Opacity", dissolvePercent);
        enemyFaceMaterial.SetFloat("_Opacity", dissolvePercent);
        enemySwordMaterial.SetFloat("_Opacity", dissolvePercent);
        //Debug.Log("body: " + enemyBodyMaterial.GetFloat("_Opacity") + " || face: " + enemyFaceMaterial.GetFloat("_Opacity") + " || sword: " + enemySwordMaterial.GetFloat("_Opacity"));
    }

    public void ResetStagger() {
        hitsTaken = 0;
    }
}
