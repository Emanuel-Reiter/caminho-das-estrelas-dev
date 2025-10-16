using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLightPreparationState : PlayerBaseState {

    float attackPreparationTime;

    public override void CheckExitState(PlayerStateManager player) {
        attackPreparationTime -= Time.deltaTime;
        if (attackPreparationTime <= 0.0f) player.SwitchState(player.attackLightState);
    }

    public override void EnterState(PlayerStateManager player) {
        player.helper.SetPerformingAttack(true);

        attackPreparationTime = player.helper.attackLightPreparationAnimationDuration.length;
        player.animator.CrossFadeInFixedTime(player.helper.attackLightPreparationAnimation, player.helper.animationSmoothTimeFull);

        player.helper.ResetComboTime();
    }

    public override void ExitState(PlayerStateManager player) {
        player.helper.SetCurrentCombo(1);
    }

    public override void UpdatePhysicsState(PlayerStateManager player) {

    }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();
    }
}
