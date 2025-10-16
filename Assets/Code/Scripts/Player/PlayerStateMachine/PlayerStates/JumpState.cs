using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        if (player.helper.velocityY < 0.0f) player.SwitchState(player.fallState);
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.HandleJump();
        player.animator.CrossFadeInFixedTime(player.helper.jumpAnimation, player.helper.animationSmoothTimeFull);
        player.helper.SetHasIvunerability(true);
    }

    public override void ExitState(PlayerStateManager player) {
        player.helper.SetHasIvunerability(false);
    }

    public override void UpdatePhysicsState(PlayerStateManager player) {

    }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();
    }
}
