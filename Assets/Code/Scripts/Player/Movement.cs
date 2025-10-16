using UnityEngine;

public class Movement : MonoBehaviour {

    private PlayerStateManager player;
    private PlayerStatus status;

    private float targetSpeed;

    private void Start() {
        player = GetComponent<PlayerStateManager>();
        status = GetComponent<PlayerStatus>();
    }

    public void ApplyMovement() {
        ChangeDirection();

        if (player.helper.isPerformingDash) {
            if (player.helper.isDashing) targetSpeed = status.dashSpeed * player.input.inputDirection.magnitude;
            else targetSpeed = 0.5f * player.input.inputDirection.magnitude;
        }
        else if (player.helper.isPerformingAttack) targetSpeed = 1.5f * player.input.inputDirection.magnitude;
        else targetSpeed = status.runSpeed * player.input.inputDirection.magnitude;

        player.helper.currentSpeed = Mathf.SmoothDamp(player.helper.currentSpeed, targetSpeed, ref player.helper.speedSmoothVelocity, player.helper.GetModifiedSmoothTime(player.helper.speedSmoothTime));

        player.helper.velocityY += player.helper.currentGravity * Time.deltaTime;
        Vector3 velocity = (transform.forward * player.helper.currentSpeed) + (Vector3.up * player.helper.velocityY);

        player.helper.Move(velocity);

        if (player.controller.isGrounded) {
            player.helper.velocityY = -1f;
        }
    }

    public void ChangeDirection() {
        if (player.input.rotationDirection != Vector2.zero) {
            float targetRotation = Mathf.Atan2(player.input.rotationDirection.x, player.input.rotationDirection.y) * Mathf.Rad2Deg + player.mainCamera.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetRotation,
                ref player.helper.rotationSmoothVelocity,
                player.helper.GetModifiedSmoothTime(player.helper.rotationSmoothTime)
                );
        }
    }

    public void HandleJump() {
        if (player.controller.isGrounded) {
            float jumpVelocity = Mathf.Sqrt(-2.0f * player.helper.currentGravity * (status.jumpHeight * 1.25f));
            player.helper.velocityY = jumpVelocity;
        }
    }
}
