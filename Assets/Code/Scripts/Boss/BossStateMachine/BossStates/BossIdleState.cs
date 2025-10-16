using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState {

    float waitTime;
    float distanceFromPlayer;

    float turnSpeed = 0.00125f;
    Quaternion rotarionTarget;
    Vector3 direcetion;

    public override void CheckExitState(BossStateManager boss) {
        if (boss.status.hasDied) boss.SwitchState(boss.dieState);
        else {
            if (waitTime <= 0.0f) {
                if (boss.helper.hasSeenThePlayer) {
                    if (distanceFromPlayer < boss.helper.meeleAttackRange) {
                        int meeleDecider = Random.Range(1, 3);
                        if (meeleDecider == 1) boss.SwitchState(boss.slowComboState);
                        else boss.SwitchState(boss.longComboState);
                    }
                    int decider = Random.Range(1, 7);

                    if(decider == 1) boss.SwitchState(boss.explosionState);
                    if(decider > 1 && decider < 4) boss.SwitchState(boss.castSpellState);
                    if(decider >= 4) boss.SwitchState(boss.chaseState);
                } 
            }
        }
    }

    public override void EnterState(BossStateManager boss) {
        if (boss.helper.waitToAttackAgian) waitTime = 1.667f;
        else waitTime = 0.001f;

        boss.helper.agent.speed = 0.00f;
        boss.helper.agent.acceleration = 200.0f;

        boss.animator.CrossFadeInFixedTime("Idle", boss.helper.animationTransitionQuadruple);

        boss.helper.SetWaitToAttack(false);
    }

    public override void ExitState(BossStateManager boss) { }

    public override void UpdatePhysicsState(BossStateManager boss) { }

    public override void UpdateState(BossStateManager boss) {
        if(boss.helper.hasSeenThePlayer) {
            direcetion = (boss.playerLocation.transform.position - boss.transform.position);
            direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;

            rotarionTarget = Quaternion.LookRotation(direcetion);
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, rotarionTarget, turnSpeed);
        }

        distanceFromPlayer = Vector3.Distance(boss.transform.position, boss.playerLocation.transform.position);


        waitTime -= Time.deltaTime;
    }
}
