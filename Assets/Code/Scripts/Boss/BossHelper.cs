using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class BossHelper : MonoBehaviour {

    [HideInInspector] public bool hasSeenThePlayer = false;
    public float meeleAttackRange = 2.0f;

    [HideInInspector] public NavMeshAgent agent;

    public AnimationClip slowComboAnimation;
    public AnimationClip longComboAnimation;
    public AnimationClip spellCastAnimation;
    public AnimationClip meteorCastAnimation;
    public AnimationClip explosionAnimation;

    private float dealDamageCooldown;
    private float maxDealDamageCooldown = 0.5f;

    public Transform swordLocation;

    [SerializeField] private Vector3 attackHitboxSize;
    [SerializeField] private float forwardOffset = 1.25f;
    [SerializeField] private float upOffset = 1.0f;

    public int staggerResistance = 1;
    [HideInInspector] public float animationTransitionHalf = 0.041f;
    [HideInInspector] public float animationTransitionFull = 0.083f;
    [HideInInspector] public float animationTransitionDouble = 0.167f;
    [HideInInspector] public float animationTransitionQuadruple = 0.333f;

    [HideInInspector] public bool waitToAttackAgian { get; private set; } = false;

    [HideInInspector] public bool allowToDealDamage { get; private set; } = false;

    public ParticleSystem darkTrail;
    public ParticleSystem lightTrail;

    public GameObject bossUI;

    public GameObject bossDoor;

    public MusicBox musicBox;

    public Transform castPoint;
    public GameObject trackerSpell;
    public float minSpellDistance = 10.0f;

    public float spellCastCooldown = 0.0f;
    public float spellCastCooldownMax = 10.0f;

    private float castSubTime;
    private float castSubTimeMax = 0.333f;

    public Transform explosionCastPoint;
    public GameObject explosion;

    private float explosionSubTime;
    private float explosionSubTimeMax = 0.333f;

    public Transform closeHitPoint;
    public float closeHitRadius;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        dealDamageCooldown = maxDealDamageCooldown;
        bossUI.SetActive(false);

        bossDoor.SetActive(false);
    }

    private void Update() {
        dealDamageCooldown -= Time.deltaTime;

        spellCastCooldown += Time.deltaTime;

        castSubTime -= Time.deltaTime;

        explosionSubTime -= Time.deltaTime;
    }

    public void AttackPlayer() {
        Vector3 attackPosition = new Vector3(
            this.transform.position.x,
            this.transform.position.y + upOffset,
            this.transform.position.z
            ) + (transform.forward * forwardOffset);

        Collider[] objectsInRange = Physics.OverlapBox(swordLocation.position, attackHitboxSize);

        for (int i = 0; i < objectsInRange.Length; i++) {
            if (objectsInRange[i].gameObject != this.gameObject) {
                if (objectsInRange[i].CompareTag("Player")) {
                    PlayerStatusManager player = objectsInRange[i].gameObject.GetComponent<PlayerStatusManager>();
                    if (dealDamageCooldown < 0.0f) {
                        player.TakeDamage(16);
                        dealDamageCooldown = maxDealDamageCooldown;
                    }
                }
            }
        }
    }

    public void AttackPlayerClose() {
        Vector3 attackPosition = new Vector3(
            this.transform.position.x,
            this.transform.position.y + upOffset,
            this.transform.position.z
            ) + (transform.forward * forwardOffset);

        Collider[] objectsInRange = Physics.OverlapSphere(closeHitPoint.position, closeHitRadius);

        for (int i = 0; i < objectsInRange.Length; i++) {
            if (objectsInRange[i].gameObject != this.gameObject) {
                if (objectsInRange[i].CompareTag("Player")) {
                    PlayerStatusManager player = objectsInRange[i].gameObject.GetComponent<PlayerStatusManager>();
                    if (dealDamageCooldown < 0.0f) {
                        player.TakeDamage(16);
                        dealDamageCooldown = maxDealDamageCooldown;
                    }
                }
            }
        }
    }

    public void CastSpell() {
        if (castSubTime < 0.0f) {
            castSubTime = castSubTimeMax;
            Instantiate(trackerSpell, castPoint.position, Quaternion.identity);
        }
        else return;
    }

    public void CastExplosion() {
        if (explosionSubTime < 0.0f) {
            explosionSubTime = explosionSubTimeMax;
            Instantiate(explosion, explosionCastPoint.position, Quaternion.identity);
        }
        else return;
    }
        
    public void AllowToDealDamage() { allowToDealDamage = true; }
    public void DenyToDealDamage() { allowToDealDamage = false; }
    public void SetWaitToAttack(bool wait) { waitToAttackAgian = wait; }

    public void ResetSpellColldown() { spellCastCooldown = 0.0f; }
    public void ResetExpolionCooldown() { explosionSubTime = 0.0f; }

    public void TargerPlayer() { 
        hasSeenThePlayer = true;
        bossUI.SetActive(true);
        musicBox.PlayBossMusic();
        bossDoor.SetActive(true);
    }
    public void UntargetPlayer() { 
        hasSeenThePlayer = false;
        bossUI.SetActive(false);
        musicBox.PlayerExplorationMusic();
        bossDoor.SetActive(false);
    }

    public void ToggleTrailEmission(bool emit) {
        if(emit) {
            darkTrail.Play();
            lightTrail.Play();

        }
        else {
            darkTrail.Stop();
            lightTrail.Stop();
        }
    }

    public void OnDrawGizmosSelected() {
        Gizmos.DrawWireCube(swordLocation.position, attackHitboxSize);
        Gizmos.DrawWireSphere(closeHitPoint.position, closeHitRadius);
    }
}