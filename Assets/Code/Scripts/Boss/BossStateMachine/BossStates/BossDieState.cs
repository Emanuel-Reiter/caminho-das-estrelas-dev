using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : BossBaseState {
    public override void CheckExitState(BossStateManager boss) {

    }

    public override void EnterState(BossStateManager boss) {
        boss.status.Die();
    }

    public override void ExitState(BossStateManager boss) { }

    public override void UpdatePhysicsState(BossStateManager boss) { }

    public override void UpdateState(BossStateManager boss) {

    }
}
