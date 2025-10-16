using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class BossSpellBehaviour : MonoBehaviour
{
    public ParticleSystem trail;
    public ParticleSystem bodyEffect;
    public ParticleSystem remains;
    public GameObject body;

    public float chaseSpeed;
    public float minDistanceFromPlayer;

    public Transform player;

    public float turnSpeed = 0.01f;
    Quaternion rotarionTarget;
    Vector3 direcetion;
    float distanceFromPlayer;

    public float damage;

    private bool chasePlayer = true;

    SphereCollider spellCollider;

    private float chaseMinTime;
    public float chaseMaxTimeMax = 1f;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("SpellTarget").gameObject.transform;
        spellCollider = GetComponent<SphereCollider>();
        chaseMinTime = 0.0f;
    }

    void Update() {
        chaseMinTime += Time.deltaTime;

        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        direcetion = (player.transform.position - this.transform.position);
        direcetion = direcetion.normalized;

        if (chasePlayer) {
            rotarionTarget = Quaternion.LookRotation(direcetion);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotarionTarget, turnSpeed);
        }

        transform.position += transform.forward * chaseSpeed * Time.deltaTime;

        if (distanceFromPlayer <= minDistanceFromPlayer && chaseMinTime > chaseMaxTimeMax) chasePlayer = false;
    }


    public void OnTriggerEnter(Collider other) {
        GameObject hitGO = other.gameObject;

        if (hitGO != null) {
            if (hitGO.tag == "Player") {
                PlayerStatusManager player = hitGO.GetComponent<PlayerStatusManager>();
                player.TakeDamage(damage);
            }

            chaseSpeed = chaseSpeed / 10.0f;
            spellCollider.enabled = false;
            trail.Stop();
            bodyEffect.Stop();
            remains.Stop();
            body.SetActive(false);
            Destroy(this.gameObject, 5.0f);
        }
    }
}
