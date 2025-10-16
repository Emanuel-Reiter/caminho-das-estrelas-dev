using UnityEngine;

public class IdleState : PlayerBaseState {

    public override void CheckExitState(PlayerStateManager player) {
        if (player.controller.isGrounded) {
            if (player.input.jumpInput) player.SwitchState(player.jumpState);

            if (player.input.dashInput && player.status.dashCooldownTimer >= player.status.dashCooldownTimerMax) player.SwitchState(player.dashState);

            if (player.input.inputDirection != Vector2.zero) player.SwitchState(player.runState);

            if (player.input.attackLightInput) {
                if (player.helper.currentCombo == 0) player.SwitchState(player.attackLightPreparationState);
                else player.SwitchState(player.attackLightState);
            }
        }
        else {
            if (player.helper.velocityY < 0.0f) player.SwitchState(player.fallState);
            else player.SwitchState(player.jumpState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.animator.CrossFadeInFixedTime(player.helper.idleAnimation, player.helper.animationSmoothTimeQuadruple);
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void UpdatePhysicsState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();
    }
}
