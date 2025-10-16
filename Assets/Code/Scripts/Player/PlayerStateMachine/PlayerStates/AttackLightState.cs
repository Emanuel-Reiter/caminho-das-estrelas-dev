using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackLightState : PlayerBaseState {

    float attackDuration;
    float maxAttackDuration;

    public override void CheckExitState(PlayerStateManager player) {
        if(attackDuration <= maxAttackDuration) attackDuration += Time.deltaTime;

        if (player.controller.isGrounded) {
            if (attackDuration >= maxAttackDuration) {
                if (player.helper.queueDash) player.SwitchState(player.dashState);
                else {
                    if(player.helper.queueAttack) player.SwitchState(player.attackLightState);
                    else player.SwitchState(player.idleState);
                }
            }
        }
        else player.SwitchState(player.fallState);
    }

    public override void EnterState(PlayerStateManager player) {
        player.helper.PlaySwordSlashSFX();
        player.helper.SetPerformingAttack(true);

        //Debug.Log("Array: " + player.helper.attackLightAnimation[player.helper.currentCombo - 1].ToString());
        maxAttackDuration = player.helper.attackLightAnimationDuration[player.helper.currentCombo -1].length;
        attackDuration = 0.0f;

        player.helper.ToggleSwordTrail(true);
        player.animator.CrossFadeInFixedTime(player.helper.attackLightAnimation[player.helper.currentCombo - 1], player.helper.animationSmoothTimeFull);

        player.helper.SetQueueAttack(false);
    }

    public override void ExitState(PlayerStateManager player) {
        player.helper.ToggleSwordTrail(false);

        player.helper.ResetComboTime();
        player.helper.SetCurrentCombo(player.helper.currentCombo + 1);

        player.helper.SetPerformingAttack(false);
    }

    public override void UpdatePhysicsState(PlayerStateManager player) {
         
    }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.ApplyMovement();

        if (attackDuration > maxAttackDuration * 0.05f && attackDuration < maxAttackDuration * 0.55f) player.helper.AttackEnemy();
        if (attackDuration > maxAttackDuration * 0.5f && player.input.attackLightInput) player.helper.SetQueueAttack(true);

        if (player.input.dashInput && player.status.dashCooldownTimer >= player.status.dashCooldownTimerMax) player.helper.SetQueueDash(true);

        if (attackDuration < maxAttackDuration * 0.2f) player.helper.SetChangeAttackDirection(true);
        else player.helper.SetChangeAttackDirection(false);

        if (attackDuration > maxAttackDuration * 0.10f && attackDuration < maxAttackDuration * 0.50f) player.helper.SetPropelAttackForward(true);
        else player.helper.SetPropelAttackForward(false);
    }
}
