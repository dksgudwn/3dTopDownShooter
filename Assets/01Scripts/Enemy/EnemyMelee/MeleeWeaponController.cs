using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeWeaponController : MonoBehaviour, IEnemyComponent
{
    [Header("Weapon model")]
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private MeleeWeaponModel[] _weaponModels;
    [SerializeField] private MeleeWeaponType _weaponType;

    public MeleeWeaponModel currentWeapon;

    [Header("Attack data")]
    public MeleeAttackDataSO attackData;
    public List<MeleeAttackDataSO> atkList;
    [SerializeField] private float _closeDistance = 1.3f;

    private Enemy _enemy;
    public void Initialize(Enemy enmey)
    {
        _enemy = enmey;
        _weaponModels = _weaponHolder.GetComponentsInChildren<MeleeWeaponModel>(true);

        SetupRandomWeapon();
    }

    private void SetupRandomWeapon()
    {
        List<MeleeWeaponModel> fillterList = new List<MeleeWeaponModel>();

        foreach (var model in _weaponModels)
        {
            model.SetActiveModel(false);
            if (model.weaponType == _weaponType)
                fillterList.Add(model);
        }

        if (fillterList.Count <= 0)
        {
            Debug.LogWarning($"No weapon - {gameObject.name}");
            return;
        }

        int idx = Random.Range(0, fillterList.Count);

        currentWeapon = fillterList[idx];
        currentWeapon.SetActiveModel(true);

        _enemy.GetCompo<EnemyAnimator>().SetAnimatorControllerIfExist(currentWeapon.animController);
        atkList = currentWeapon.attackDataList;
        attackData = atkList[0];
    }

    public bool IsPlayerInAttackRange()
    {
        Vector3 targetPos = _enemy.TargetTrm.position;
        return Vector3.Distance(targetPos, transform.position) < attackData.atkRange;
    }

    public bool IsPlayerClosed()
    {
        Vector3 targetPos = _enemy.TargetTrm.position;
        return Vector3.Distance(targetPos, transform.position) < _closeDistance;
    }

    public void UpdateNextAttack()
    {
        List<MeleeAttackDataSO> validAttacks;

        var type = IsPlayerClosed() ? MeleeAttackType.Close : MeleeAttackType.Charge;
        validAttacks = atkList.Where(x => x.attackType == type).ToList();

        int randIdx = Random.Range(0, validAttacks.Count);
        attackData = validAttacks[randIdx];
    }
}
