using System.Net;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHelper : MonoBehaviour {

    [HideInInspector] public bool hasSeenThePlayer = false;
    public float meeleAttackRange = 2.0f;

    [HideInInspector] public NavMeshAgent agent;

    public AnimationClip attackAnimation;

    private float dealDamageCooldown;
    private float maxDealDamageCooldown = 1.25f;

    [SerializeField] private float attackRadius = 1.0f;
    [SerializeField] private float forwardOffset = 1.25f;
    [SerializeField] private float upOffset = 1.0f;

    public int staggerResistance = 1;
    [HideInInspector] public float animationTransitionHalf = 0.081f;
    [HideInInspector] public float animationTransitionFull = 0.083f;
    [HideInInspector] public float animationTransitionDouble = 0.167f;
    [HideInInspector] public float animationTransitionQuadruple = 0.333f;

    [HideInInspector] public bool waitToAttackAgian { get; private set; } = false;


    [HideInInspector] public bool allowToDealDamage { get; private set; } = false;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start() {
        dealDamageCooldown = maxDealDamageCooldown;
    }
        
    private void Update() {
        dealDamageCooldown -= Time.deltaTime;
    }

    public void AttackPlayer() {
        Vector3 attackPosition = new Vector3(
            this.transform.position.x,
            this.transform.position.y + upOffset,
            this.transform.position.z
            ) + (transform.forward * forwardOffset);

        Collider[] objectsInRange = Physics.OverlapSphere(attackPosition, attackRadius);

        for (int i = 0; i < objectsInRange.Length; i++) {
            if (objectsInRange[i].gameObject != this.gameObject) {
                if (objectsInRange[i].CompareTag("Player")) {
                    PlayerStatusManager player = objectsInRange[i].gameObject.GetComponent<PlayerStatusManager>();
                    if(dealDamageCooldown < 0.0f) {
                        player.TakeDamage(16);
                        dealDamageCooldown = maxDealDamageCooldown;
                    }
                }
            }
        }
    }

    public void AllowToDealDamage() { allowToDealDamage = true; }
    public void DenyToDealDamage() { allowToDealDamage = false; }
    public void SetWaitToAttack(bool wait) { waitToAttackAgian = wait; }

    public void IncreaseStaggerResistance() { staggerResistance += 0; }

    public void OnDrawGizmosSelected() {
        Vector3 attackPosition = new Vector3(
            this.transform.position.x,
            this.transform.position.y + upOffset,
            this.transform.position.z 
            ) + (transform.forward * forwardOffset);
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }
}