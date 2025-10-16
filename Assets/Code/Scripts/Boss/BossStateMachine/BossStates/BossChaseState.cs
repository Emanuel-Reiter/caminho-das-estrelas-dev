using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BossChaseState : BossBaseState {

    float turnSpeed = 0.01f;
    Quaternion rotarionTarget;
    Vector3 direcetion;

    float recalculateDestinationTimer;
    float recalculateDestinationTimerMax = 0.1f;

    float distanceFromPlayer;

    public override void CheckExitState(BossStateManager boss) {
        if (boss.status.hasDied) boss.SwitchState(boss.dieState);
        else {
            if (distanceFromPlayer > boss.helper.minSpellDistance) {
                if (boss.helper.spellCastCooldown > boss.helper.spellCastCooldownMax) {
                    int decider = Random.Range(1, 5);
                    if (decider == 1) boss.SwitchState(boss.explosionState);
                    else boss.SwitchState(boss.castSpellState);
                }
            }
            else if (distanceFromPlayer < boss.helper.meeleAttackRange) {
                int decider = Random.Range(1, 3);
                if (decider == 1) boss.SwitchState(boss.slowComboState);    
                else boss.SwitchState(boss.longComboState);
            }
        }
    }

    public override void EnterState(BossStateManager boss) {
        boss.animator.CrossFadeInFixedTime("Chase", boss.helper.animationTransitionDouble);

        boss.helper.agent.speed = 3.0f;
        boss.helper.agent.acceleration = 20.0f;
        boss.helper.agent.SetDestination(boss.playerLocation.transform.position);

        recalculateDestinationTimer = recalculateDestinationTimerMax;
    }

    public override void ExitState(BossStateManager boss) { }

    public override void UpdatePhysicsState(BossStateManager boss) { }

    public override void UpdateState(BossStateManager boss) {

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

        if (distanceFromPlayer < boss.helper.meeleAttackRange) boss.helper.agent.speed = 0.1f;
    }
}
