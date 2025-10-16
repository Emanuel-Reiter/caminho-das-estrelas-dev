using UnityEngine;

public class DashState : PlayerBaseState {

    float animationTime;
    float animationTimeMax;

    public override void CheckExitState(PlayerStateManager player) {
        if (animationTime >= animationTimeMax) {
            if (player.helper.queueAttack) {
                player.helper.SetCurrentCombo(2);
                player.SwitchState(player.attackLightState);
            }
            else {
                if(player.controller.isGrounded) {
                    player.SwitchState(player.idleState);
                }
                else {
                    player.SwitchState(player.fallState);
                }

            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        animationTimeMax = player.helper.dashAnimationDuration.length;
        animationTime = 0.0f;

        player.animator.CrossFadeInFixedTime(player.helper.dashAnimation, player.helper.animationSmoothTimeHalf);

        player.helper.SetQueueDash(false);

        player.helper.SetIsPerformingDashing(true);
        player.helper.SetIsDashing(true);
        player.helper.SetHasIvunerability(true);
    }

    public override void ExitState(PlayerStateManager player) {
        player.helper.SetIsPerformingDashing(false);
        player.helper.SetIsDashing(false);
        player.status.ResetDash();
        player.helper.SetHasIvunerability(false);
    }

    public override void UpdatePhysicsState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        animationTime += Time.deltaTime;
        player.movement.ApplyMovement();
        if (animationTime > animationTimeMax * 0.25f) player.helper.SetIsDashing(false);

        if (player.input.attackLightInput) {
            player.helper.SetQueueAttack(true);
            player.helper.SetCurrentCombo(1);
        }

    }
}
