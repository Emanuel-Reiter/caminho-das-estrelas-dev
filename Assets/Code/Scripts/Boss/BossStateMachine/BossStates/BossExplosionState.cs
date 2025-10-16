using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosionState : BossBaseState {

    float animationTime;
    float animationTimeMax;

    public override void CheckExitState(BossStateManager boss) {
        if(boss.status.hasDied) {
            boss.SwitchState(boss.dieState);
        }
        else {
            if (animationTime > animationTimeMax) boss.SwitchState(boss.idleState);
        }
    }

    public override void EnterState(BossStateManager boss) {
        boss.agent.speed = 0.0f;
        boss.agent.acceleration = 100.0f;
        animationTimeMax = boss.helper.explosionAnimation.length;
        boss.animator.CrossFadeInFixedTime("Explosion", boss.helper.animationTransitionQuadruple);
        animationTime = 0.0f;
    }

    public override void ExitState(BossStateManager boss) {
        boss.helper.SetWaitToAttack(true);
    }

    public override void UpdatePhysicsState(BossStateManager boss) { }

    public override void UpdateState(BossStateManager boss) {
        animationTime += Time.deltaTime;

        if (animationTime > 5.01f && animationTime < 5.21f) boss.helper.CastExplosion();
        if (animationTime > 8.0f && animationTime < 8.2f) boss.helper.CastExplosion();
    }
}
