using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public abstract bool CanShooting();
}
