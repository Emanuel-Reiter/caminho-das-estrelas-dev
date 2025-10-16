using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState {

    float turnSpeed = 0.01f;
    Quaternion rotarionTarget;
    Vector3 direcetion;
    float distanceFromPlayer;

    float recalculateDestinationTimer;
    float recalculateDestinationTimerMax = 0.1f;


    public override void CheckExitState(EnemyStateManager enemy) {
        if (enemy.status.hasDied) enemy.SwitchState(enemy.dieState);
        else {
            if (enemy.status.hitsTaken >= enemy.helper.staggerResistance) enemy.SwitchState(enemy.flintchState);

            if (distanceFromPlayer < enemy.helper.meeleAttackRange) enemy.SwitchState(enemy.attackState);
        }
    }

    public override void EnterState(EnemyStateManager enemy) {
        enemy.animator.CrossFadeInFixedTime("Chase", enemy.helper.animationTransitionFull);

        enemy.helper.agent.speed = 7.0f;
        enemy.helper.agent.acceleration = 20.0f;
        enemy.helper.agent.SetDestination(enemy.playerLocation.transform.position);

        recalculateDestinationTimer = recalculateDestinationTimerMax;
    }

    public override void ExitState(EnemyStateManager enemy) {
        enemy.animator.speed = 1.0f;
    }

    public override void UpdatePhysicsState(EnemyStateManager enemy) {

    }

    public override void UpdateState(EnemyStateManager enemy) {
        distanceFromPlayer = Vector3.Distance(enemy.transform.position, enemy.playerLocation.transform.position);

        direcetion = (enemy.playerLocation.transform.position - enemy.transform.position);
        direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;

        rotarionTarget = Quaternion.LookRotation(direcetion);

        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotarionTarget, turnSpeed);

        if(recalculateDestinationTimer > 0.0f) {
            recalculateDestinationTimer -= Time.deltaTime;
        }
        else {
            enemy.helper.agent.SetDestination(enemy.playerLocation.transform.position);
            recalculateDestinationTimer = recalculateDestinationTimerMax;
        }

        if(distanceFromPlayer < enemy.helper.meeleAttackRange) enemy.helper.agent.speed = 0.1f;
    }
}
