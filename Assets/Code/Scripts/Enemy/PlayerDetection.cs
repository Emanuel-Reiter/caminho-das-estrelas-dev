using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private Transform playerLocation;

    private float searchTimer;
    private float searchTimerMax = 0.1f;

    private float detectionRange = 12.0f;

    private EnemyHelper helper;
    private PlayerStateManager player;

    private void Start() {
        searchTimer = searchTimerMax;

        helper = GetComponent<EnemyHelper>();

        playerLocation = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStateManager>();
    }

    private void Update() {
        if (searchTimer > 0.0f) searchTimer -= Time.deltaTime;
        else {
            SearchPlayer();
            searchTimer = searchTimerMax;
        }
    }

    private void SearchPlayer() {
        Vector3 playerPositon = new Vector3(playerLocation.transform.position.x,
            playerLocation.transform.position.y,
            playerLocation.transform.position.z);
        Vector3 enemyPosition = new Vector3(this.transform.position.x,
            this.transform.position.y,
            this.transform.position.z);

        float distanceFromPlayer = Vector3.Distance(enemyPosition, playerPositon);

        if (distanceFromPlayer < detectionRange) {
            if (!helper.hasSeenThePlayer) {
                helper.hasSeenThePlayer = CheckForDirectLineOfSight(player);
            }
        }
    }

    private bool CheckForDirectLineOfSight(PlayerStateManager player) {
        RaycastHit hit;
        Physics.Raycast(
            this.transform.position,
            (player.transform.position - this.transform.position).normalized,
            out hit);

        GameObject hitGO = hit.collider.gameObject;

        if (hitGO != null) {
            if (hitGO.tag == "Player") {
                return true;
            }
            else {
                return false;
            }
        }
        return false;
    }
}
