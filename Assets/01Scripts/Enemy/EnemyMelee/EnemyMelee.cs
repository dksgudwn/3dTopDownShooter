using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeState
{
    Idle, Move, Recovery, Chase, Attack
}

public enum MeleeWeaponType
{
    OneHand, UnArmed
}
public class EnemyMelee : Enemy
{
    private Dictionary<MeleeState, EnemyState> _states;

    protected override void Awake()
    {
        base.Awake();
        _states = new Dictionary<MeleeState, EnemyState>()
        {
            {MeleeState.Idle, new EnemyMeleeIdle(this,StateMachine,"Idle") },
            {MeleeState.Move, new EnemyMeleeMove(this,StateMachine,"Move") },
            {MeleeState.Recovery, new EnemyMeleeRecovery(this,StateMachine,"Recovery") },
            {MeleeState.Chase, new EnemyMeleeChase(this,StateMachine,"Chase") },
            {MeleeState.Attack, new EnemyMeleeAttack(this,StateMachine,"Attack") },
        };
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(_states[MeleeState.Idle]);
    }
    public override EnemyState GetState(Enum enumType)
    {
        var state = (MeleeState)enumType;
        if (_states.TryGetValue(state, out EnemyState enemyState))
        {
            return enemyState;
        }
        return null;
    }

    public override void EnteringBattleMode()
    {
        PlayerSpotted = true;
        StateMachine.ChangeState(_states[MeleeState.Recovery]);
    }
}
