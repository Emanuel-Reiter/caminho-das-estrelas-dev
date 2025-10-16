using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLongComboState : BossBaseState {

    float attackDuration;
    float maxAttackDuration;

    float turnSpeed = 0.00125f;
    Quaternion rotarionTarget;
    Vector3 direcetion;

    float recalculateDestinationTimer;
    float recalculateDestinationTimerMax = 0.1f;

    float distanceFromPlayer;


    public override void CheckExitState(BossStateManager boss) {
        if (boss.status.hasDied) boss.SwitchState(boss.dieState);
        else {
            if (attackDuration >= maxAttackDuration) boss.SwitchState(boss.idleState);
        }
    }

    public override void EnterState(BossStateManager boss) {
        //enemy.status.SetHyperArmor(true);
        boss.helper.agent.speed = 2.0f;
        boss.helper.agent.acceleration = 250.0f;

        maxAttackDuration = boss.helper.longComboAnimation.length;
        attackDuration = 0.0f;
        boss.animator.CrossFadeInFixedTime("SwordComboLong", boss.helper.animationTransitionFull);
        boss.helper.ToggleTrailEmission(true);
    }

    public override void ExitState(BossStateManager boss) {
        boss.helper.ToggleTrailEmission(false);
        boss.helper.SetWaitToAttack(true);
    }

    public override void UpdatePhysicsState(BossStateManager enemy) { }

    public override void UpdateState(BossStateManager boss) {
        attackDuration += Time.deltaTime;
        if (attackDuration <= maxAttackDuration * 0.1f && attackDuration >= maxAttackDuration * 0.6f) boss.helper.agent.speed = 0.00f;

        //attack the player
        if (attackDuration > maxAttackDuration * 0.105f && attackDuration < maxAttackDuration * 0.154f) {
            boss.helper.AttackPlayer();
            boss.helper.AttackPlayerClose();
        }

        if (attackDuration > maxAttackDuration * 0.262f && attackDuration < maxAttackDuration * 0.282f) {
            boss.helper.AttackPlayer();
            boss.helper.AttackPlayerClose();
        }

        if (attackDuration > maxAttackDuration * 0.448f && attackDuration < maxAttackDuration * 0.471f) {
            boss.helper.AttackPlayer();
            boss.helper.AttackPlayerClose();
        }

        if (attackDuration > maxAttackDuration * 0.625f && attackDuration < maxAttackDuration * 0.645f) {
            boss.helper.AttackPlayer();
            boss.helper.AttackPlayerClose();
        }

        if (attackDuration > maxAttackDuration * 0.82f && attackDuration < maxAttackDuration * 0.846f) {
            boss.helper.AttackPlayer();
            boss.helper.AttackPlayerClose();
        }

        direcetion = (boss.playerLocation.transform.position - boss.transform.position);
        direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;
        if (direcetion == Vector3.zero) direcetion = Vector3.forward * 0.001f;

        distanceFromPlayer = Vector3.Distance(boss.transform.position, boss.playerLocation.transform.position);

        direcetion = (boss.playerLocation.transform.position - boss.transform.position);
        direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;

        rotarionTarget = Quaternion.LookRotation(direcetion);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, rotarionTarget, turnSpeed);

        if (recalculateDestinationTimer > 0.0f) {
            recalculateDestinationTimer -= Time.deltaTime;
        }
        else {
            boss.helper.agent.SetDestination(boss.playerLocation.transform.position);
            recalculateDestinationTimer = recalculateDestinationTimerMax;
        }
    }
}
