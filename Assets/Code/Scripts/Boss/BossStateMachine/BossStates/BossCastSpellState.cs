using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCastSpellState : BossBaseState {

    float animationDuration;
    float animationDurationMax;

    float distanceFromPlayer;
    float turnSpeed = 0.005f;
    Quaternion rotarionTarget;
    Vector3 direcetion;


    public override void CheckExitState(BossStateManager boss) {
        if (boss.status.hasDied) boss.SwitchState(boss.dieState);
        else {
            if (animationDuration >= animationDurationMax) {
                boss.SwitchState(boss.idleState);
            }
        }
    }

    public override void EnterState(BossStateManager boss) {
        boss.animator.CrossFadeInFixedTime("CastSpellLong", boss.helper.animationTransitionQuadruple);
        animationDurationMax = boss.helper.spellCastAnimation.length;
        animationDuration = 0.0f;

        boss.helper.agent.speed = 0.00f;
        boss.helper.agent.acceleration = 200.0f;
    }

    public override void ExitState(BossStateManager boss) {
        boss.helper.ResetSpellColldown();
    }

    public override void UpdatePhysicsState(BossStateManager boss) {

    }

    public override void UpdateState(BossStateManager boss) {
        animationDuration += Time.deltaTime;

        //Debug.Log(animationDuration);

        if (animationDuration > 2.05f && animationDuration < 2.2f) boss.helper.CastSpell();
        if (animationDuration > 3.05f && animationDuration < 3.2f) boss.helper.CastSpell();
        if (animationDuration > 4.05f && animationDuration < 4.2f) boss.helper.CastSpell();

        direcetion = (boss.playerLocation.transform.position - boss.transform.position);
        direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;

        rotarionTarget = Quaternion.LookRotation(direcetion);
        boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, rotarionTarget, turnSpeed);

        distanceFromPlayer = Vector3.Distance(boss.transform.position, boss.playerLocation.transform.position);

    }
}
