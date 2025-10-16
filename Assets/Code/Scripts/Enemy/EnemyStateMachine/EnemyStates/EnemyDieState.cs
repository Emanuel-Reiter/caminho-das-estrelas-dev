using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : EnemyBaseState {

    float dieTimer;
    float dieTimerMax = 1.458f;

    float dissolveAmount;

    public override void CheckExitState(EnemyStateManager enemy) {

    }

    public override void EnterState(EnemyStateManager enemy) {
        enemy.status.defaultEffect.Stop();
        enemy.status.dieEffect.Play();
        enemy.status.SetHyperArmor(true);

        dissolveAmount = 0.0f;

        dieTimer = dieTimerMax;
        enemy.agent.acceleration = 100.0f;
        enemy.agent.SetDestination(enemy.transform.position);

        enemy.animator.CrossFadeInFixedTime("Die", enemy.helper.animationTransitionFull);

        enemy.status.DeactivateColliders();
    }

    public override void ExitState(EnemyStateManager enemy) {
        enemy.status.SetHyperArmor(false);
    }

    public override void UpdatePhysicsState(EnemyStateManager enemy) {

    }

    public override void UpdateState(EnemyStateManager enemy) {
        dissolveAmount += Time.deltaTime;
        enemy.status.DissolveEnemy(dissolveAmount);

        dieTimer -= Time.deltaTime;
        if (dieTimer < 0.0f) {
                enemy.status.Die();
            }
        }
    }
