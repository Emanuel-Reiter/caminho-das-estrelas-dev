using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState {

    float waitTime;
    float distanceFromPlayer;

    public override void CheckExitState(EnemyStateManager enemy) {
        if (enemy.status.hasDied) enemy.SwitchState(enemy.dieState);
        else {
            if (enemy.status.hitsTaken >= enemy.helper.staggerResistance) enemy.SwitchState(enemy.flintchState);

            if(waitTime < 0.0f) {
                if (enemy.helper.hasSeenThePlayer) {
                    if (distanceFromPlayer < enemy.helper.meeleAttackRange) enemy.SwitchState(enemy.attackState);
                    enemy.SwitchState(enemy.chaseState);
                }
            }
        }
    }

    public override void EnterState(EnemyStateManager enemy) {
        if (enemy.helper.waitToAttackAgian && distanceFromPlayer < enemy.helper.meeleAttackRange + 1) waitTime = Random.Range(1.0f, 2.0f);
        else waitTime = 0.001f;

        enemy.animator.CrossFadeInFixedTime("Idle", enemy.helper.animationTransitionQuadruple);

        enemy.helper.SetWaitToAttack(false);
    }

    public override void ExitState(EnemyStateManager enemy) {

    }

    public override void UpdatePhysicsState(EnemyStateManager enemy) {

    }

    public override void UpdateState(EnemyStateManager enemy) {
        distanceFromPlayer = Vector3.Distance(enemy.transform.position, enemy.playerLocation.transform.position);

        waitTime -= Time.deltaTime;
    }
}
