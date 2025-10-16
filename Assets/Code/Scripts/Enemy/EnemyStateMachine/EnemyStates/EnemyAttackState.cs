using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState {

    float attackDuration;
    float maxAttackDuration;

    float turnSpeed = 0.05f;
    Quaternion rotarionTarget;
    Vector3 direcetion;


    public override void CheckExitState(EnemyStateManager enemy) {
        if (enemy.status.hasDied) enemy.SwitchState(enemy.dieState);
        else {
            if (enemy.status.hitsTaken >= enemy.helper.staggerResistance) enemy.SwitchState(enemy.flintchState);
            if (attackDuration <= 0.0f) enemy.SwitchState(enemy.idleState);
        }
    }

    public override void EnterState(EnemyStateManager enemy) {
        //enemy.status.SetHyperArmor(true);

        maxAttackDuration = enemy.helper.attackAnimation.length;
        attackDuration = maxAttackDuration;
        enemy.animator.CrossFadeInFixedTime("Attack0", enemy.helper.animationTransitionFull);
        enemy.agent.acceleration = 50.0f;
    }

    public override void ExitState(EnemyStateManager enemy) {
        enemy.status.SetHyperArmor(false);
        enemy.helper.SetWaitToAttack(true);
    }

    public override void UpdatePhysicsState(EnemyStateManager enemy) {
   
    }

    public override void UpdateState(EnemyStateManager enemy) {
        attackDuration -= Time.deltaTime;

        direcetion = (enemy.playerLocation.transform.position - enemy.transform.position);
        direcetion = new Vector3(direcetion.x, 0.0f, direcetion.z).normalized;
        if (direcetion == Vector3.zero) direcetion = Vector3.forward * 0.001f;

        if (attackDuration <= maxAttackDuration * 0.6f && attackDuration >= maxAttackDuration * 0.4f) {
            enemy.helper.AttackPlayer();
        }

            if (attackDuration >= maxAttackDuration * 0.5f) {
            rotarionTarget = Quaternion.LookRotation(direcetion);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotarionTarget, turnSpeed);
        }
    }
}
