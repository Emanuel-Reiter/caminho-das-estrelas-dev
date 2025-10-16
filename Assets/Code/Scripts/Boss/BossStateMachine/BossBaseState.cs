using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBaseState {
    public abstract void EnterState(BossStateManager boss);
    public abstract void UpdateState(BossStateManager boss);
    public abstract void UpdatePhysicsState(BossStateManager boss);
    public abstract void CheckExitState(BossStateManager boss);
    public abstract void ExitState(BossStateManager boss);

}
