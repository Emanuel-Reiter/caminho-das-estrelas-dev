using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        if (player.controller.isGrounded) player.SwitchState(player.idleState);
    }

    public override void EnterState(PlayerStateManager player) {
        player.animator.CrossFade(player.helper.fallAnimation, player.helper.animationSmoothTimeFull);
    }

    public override void ExitState(PlayerStateManager player) {

    }

    public override void UpdatePhysicsState(PlayerStateManager player) {

    }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();
    }
}
