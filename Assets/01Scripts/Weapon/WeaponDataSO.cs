using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrabType
{
    Side = 0, Back = 1
}

public enum ShootType
{
    Single = 0, Auto = 1
}

[CreateAssetMenu(menuName = "SO/Weapon/Data")]
public class WeaponDataSO : ScriptableObject
{
    public string gunTrmName;

    [Header("Weapon info")]
    public float recoilPower;
    public float reloadSpeed;
    public float EquipSpeed;
    [Space]
    public ShootType shootType;
    public float fireRate = 1f;
    public float camDistance = 6f;

    [Header("Bullet spread")]
    public float spreadAmout;
    public float maxSpreadAmout;
    public float spreadIncRate = 0.15f;
    public float spreadCooldown = 1f;

    [Header("Ammo data")]
    public int maxAmmo;
    public float shootingRange = 2f;
    public float bulletSpeed = 60f;
    public int bulletPerShot = 1;
    public float impactForce;
    public float damage = 2;

    [Header("Animation setup")]
    public int animationLayer;
    public GrabType grabType;

    [Header("Left hand IK")]
    public Vector3 leftHandPositon;
    public Vector3 leftHandRotation;
    public Vector3 leftHandHintPosition;
}
