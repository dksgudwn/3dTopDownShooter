using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Weapon
{
    public WeaponDataSO weaponData;
    public int bulletInMagazine;

    public Transform GunTrm { get; set; }
    public Transform GunPoint { get; set; }

    protected float _nextShooTime;
    protected float _currentSpread, _lastSpreadTime;

    public void Shooting()
    {
        bulletInMagazine--;
        _nextShooTime = Time.time + 1 / weaponData.fireRate;
    }

    public void AdjustRig(Transform leftTarget, Transform leftHint)
    {
        leftTarget.localPosition = weaponData.leftHandPositon;
        leftTarget.localRotation = Quaternion.Euler(weaponData.leftHandRotation);
        leftHint.localPosition = weaponData.leftHandHintPosition;
    }

    #region Bullet spread region
    public void UpdateSpread()
    {
        if (Time.time > _lastSpreadTime + weaponData.spreadCooldown)
        {
            _currentSpread = weaponData.spreadAmout;
        }
        else
        {
            IncreaseSpread();
        }
        _lastSpreadTime = Time.time;
    }

    public void IncreaseSpread()
    {
        _currentSpread = Mathf.Clamp(_currentSpread + weaponData.spreadIncRate, weaponData.spreadAmout, weaponData.maxSpreadAmout);
    }

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomize = Random.Range(-_currentSpread, _currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomize, randomize, randomize);
        return spreadRotation * originalDirection;
    }

    #endregion

    public abstract bool CanShooting();

    #region Reloading ammo abstract

    public abstract bool CanReload();
    public abstract void TryToRelloadBullet();
    public abstract void FillBullet();
    #endregion
}
