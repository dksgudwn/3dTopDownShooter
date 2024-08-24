using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour, IPlayerComponent
{

    private readonly int _xVelocityHash = Animator.StringToHash("xVelocity");
    private readonly int _zVelocityHash = Animator.StringToHash("zVelocity");
    private readonly int _isRunningHash = Animator.StringToHash("isRunning");
    private readonly int _fireTriggerHash = Animator.StringToHash("fire");
    private readonly int _grabTypeHash = Animator.StringToHash("weaponGrabType");
    private readonly int _grabTriggerHash = Animator.StringToHash("weaponGrab");
    private readonly int _equipSpeedHash = Animator.StringToHash("equipSpeed");

    private Animator _animator;
    private Player _player;

    private PlayerMovement _moveCompo;
    private bool _isPlayRunAnimation;
    private PlayerWeaponController _weaponController;

    public event Action<bool> GrabStatusChangeEvent; // 무기 집기 상태 알림
    public event Action WeaponGrabTriggerEvent; // 백업 집는 순간

    public void Initialize(Player player)
    {
        _player = player;
        _animator = GetComponent<Animator>();
        _moveCompo = _player.GetCompo<PlayerMovement>();
        _moveCompo.OnMovement += HandleMovement;
        _moveCompo.OnRunning += HandleOnRunning;

        _weaponController = _player.GetCompo<PlayerWeaponController>();
        _weaponController.WeaponFireEvent += HandleWeaponFireEvent;
        _weaponController.WeaponChangeStartEvent += HandleWeaponChangeStart;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < _animator.layerCount; i++)
        {
            _animator.SetLayerWeight(i, 0);
        }
        _animator.SetLayerWeight(layerIndex, 1);
    }

    private void PlayGrabAnimation(GrabType grabType, float equipSpeed)
    {
        GrabStatusChangeEvent?.Invoke(false);
        _animator.SetFloat(_equipSpeedHash, equipSpeed);
        _animator.SetInteger(_grabTypeHash, (int)grabType);
        _animator.SetTrigger(_grabTriggerHash);
    }

    private void HandleWeaponChangeStart(PlayerWeapon before, PlayerWeapon next)
    {
        int layerIndex = next.weaponData.animationLayer;
        SwitchAnimationLayer(layerIndex);
        PlayGrabAnimation(next.weaponData.grabType, next.weaponData.EquipSpeed);
    }

    public void ChangeWeaponAnimation() => WeaponGrabTriggerEvent?.Invoke();
    public void GrabAnimationEnd() => GrabStatusChangeEvent?.Invoke(true);

    private void HandleWeaponFireEvent()
    {
        _animator.SetTrigger(_fireTriggerHash);
    }

    private void HandleOnRunning(bool isRunning)
    {
        _isPlayRunAnimation = isRunning;
    }

    private void HandleMovement(Vector3 movement)
    {
        float x = Vector3.Dot(movement, transform.right);
        float z = Vector3.Dot(movement, transform.forward);

        float dampTime = 0.1f;
        _animator.SetFloat(_xVelocityHash, x, dampTime, Time.deltaTime);
        _animator.SetFloat(_zVelocityHash, z, dampTime, Time.deltaTime);

        bool isPlay = movement.sqrMagnitude > 0 && _isPlayRunAnimation;

        _animator.SetBool(_isRunningHash, _isPlayRunAnimation);
    }
}
