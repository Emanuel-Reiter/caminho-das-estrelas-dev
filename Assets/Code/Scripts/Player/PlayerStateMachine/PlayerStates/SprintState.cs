using UnityEngine;

public class SprintState : PlayerBaseState {

    private float outOfGroundTimer = 0.0f;
    private float outOfGroundTimerMax = 0.167f;

    public override void CheckExitState(PlayerStateManager player) {
        if (player.controller.isGrounded) {
            outOfGroundTimer = outOfGroundTimerMax;

            if (player.input.inputDirection == Vector2.zero) player.SwitchState(player.idleState);

            if (player.input.jumpInput) player.SwitchState(player.jumpState);

            if (player.input.attackLightInput) {
                if (player.helper.currentCombo == 0) player.SwitchState(player.attackLightPreparationState);
                else player.SwitchState(player.attackLightState);
            }
        }
        else {
            outOfGroundTimer -= Time.deltaTime;
            if (outOfGroundTimer < 0.0f) player.SwitchState(player.fallState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        outOfGroundTimer = outOfGroundTimerMax;
        player.animator.CrossFadeInFixedTime(player.helper.runAnimation, player.helper.animationSmoothTimeFull);
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void UpdatePhysicsState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();
    }
}
