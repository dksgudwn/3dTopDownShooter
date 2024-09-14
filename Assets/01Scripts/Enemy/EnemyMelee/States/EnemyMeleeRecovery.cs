using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeRecovery : EnemyState
{
    private EnemyMovement _movement;
    private MeleeWeaponController _controller;

    public EnemyMeleeRecovery(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        _movement = enemy.GetCompo<EnemyMovement>();
        _controller = enemy.GetCompo<MeleeWeaponController>();
    }

    public override void Enter()
    {
        base.Enter();
        _movement.SetStop(true);
    }
    public override void UpdateState()
    {
        base.UpdateState();
        _enemy.FaceToTarget(_enemy.TargetTrm.position);

        if (_animationTrigger)
        {
            if (_controller.IsPlayerInAttackRange())
            {
                _stateMachine.ChangeState(_enemy.GetState(MeleeState.Attack));
            }
            else
            {
                _stateMachine.ChangeState(_enemy.GetState(MeleeState.Chase));

            }
        }
    }
    public override void Exit()
    {
        _movement.SetStop(false);
        base.Exit();
    }
}
