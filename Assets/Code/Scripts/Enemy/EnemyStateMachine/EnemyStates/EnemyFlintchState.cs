using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlintchState : EnemyBaseState {
    float flintchTime;
    float flintchTimeMax = 0.958f;
    public override void CheckExitState(EnemyStateManager enemy) {
        if(enemy.status.hasDied) enemy.SwitchState(enemy.dieState);
        else {
            if (enemy.status.queueStagger) enemy.SwitchState(enemy.flintchState);
            if (flintchTime <= 0.0f) {
                enemy.SwitchState(enemy.idleState);
            }
        }
    }

    public override void EnterState(EnemyStateManager enemy) {
        enemy.helper.IncreaseStaggerResistance();
        enemy.status.ResetStagger();
        enemy.status.QueueStagger(false);

        flintchTime = flintchTimeMax;

        enemy.agent.acceleration = 100.0f;
        enemy.agent.SetDestination(enemy.transform.position);

        enemy.animator.CrossFadeInFixedTime("TakeDamage", enemy.helper.animationTransitionHalf);
    }

    public override void ExitState(EnemyStateManager enemy) {
        enemy.status.SetHyperArmor(false);
    }

    public override void UpdatePhysicsState(EnemyStateManager enemy) { }

    public override void UpdateState(EnemyStateManager enemy) {
        if (enemy.status.hitsTaken >= enemy.helper.staggerResistance) enemy.status.QueueStagger(true);
        flintchTime -= Time.deltaTime;
    }
}
