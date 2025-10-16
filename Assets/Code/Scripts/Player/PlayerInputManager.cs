using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {
    public Vector2 inputDirection { get; private set; }
    public Vector2 rotationDirection { get; private set; }

    public bool dashInput { get; private set; }

    public bool jumpInput { get; private set; }

    public bool attackLightInput { get; private set; }

    public bool interactInput { get; private set; }

    public bool escapeInput { get; private set; }

    private PlayerHelper helper;

    private void Start() {
        helper = GetComponent<PlayerHelper>();

        attackLightInput = false;
        jumpInput = false;
        inputDirection = Vector2.zero;
    }

    private void Update() {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (helper.isPerformingDash) {
            inputDirection = Vector2.up;
        }
        else {
            if (helper.isPerformingAttack) {
                if (helper.propelAttackForward) inputDirection = Vector2.up;
                else inputDirection = Vector2.zero;

                if (helper.changeAttackDirection) rotationDirection = direction;
                else rotationDirection = Vector2.zero;
            }
            else {
                inputDirection = direction;
                rotationDirection = direction;
            }
        }

        dashInput = Input.GetKeyDown(KeyCode.LeftShift);

        jumpInput = Input.GetButtonDown("Jump");

        attackLightInput = Input.GetButtonDown("Fire1");

        escapeInput = Input.GetKeyDown(KeyCode.P);

        interactInput = Input.GetKeyDown(KeyCode.E);
    }

    public void EscapeButton() {
        StartCoroutine(EscapeButtonCoroutine());
    }

    private IEnumerator EscapeButtonCoroutine() {
        escapeInput = true;
        yield return null;
        escapeInput = false;
    }
}
