using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IEnemyComponent
{
    private Enemy _enemy;
    private Animator _animator;
    public Animator Animator => _animator;
    private EnemyMovement _movement;

    private readonly int _atkAnimSpeedHash = Animator.StringToHash("AtkAnimationSpeed");
    private readonly int _atkIndexHash = Animator.StringToHash("AtkIndex");
    private readonly int _slashIndexHash = Animator.StringToHash("SlashIndex");

    public void Initialize(Enemy enmey)
    {
        _enemy = enmey;
        _animator = GetComponent<Animator>();
        _movement = enmey.GetCompo<EnemyMovement>();
    }

    public void SetAnimatorControllerIfExist(AnimatorOverrideController animController)
    {
        if (animController == null) return;
        _animator.runtimeAnimatorController = animController;
    }

    internal void SetAttackParam(int atkIndex, int slashIndex, float animationSpeed)
    {
        _animator.SetInteger(_atkIndexHash, atkIndex);
        _animator.SetInteger(_slashIndexHash, slashIndex);
        _animator.SetFloat(_atkAnimSpeedHash, animationSpeed);
    }

    private void AnimationEnd()
    {
        _enemy.OnAnimationEnd();
    }

    private void StartManualMovement() => _movement.SetManualMovement(true);
    private void StopManualMovement() => _movement.SetManualMovement(false);

    private void StartManualRotation() => _movement.SetManualRotation(true);
    private void StopManualRotation() => _movement.SetManualRotation(false);
}
