using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyVisual : MonoBehaviour, IEnemyComponent
{
    [Header("Crystal visual")]
    [SerializeField] private EnemyCrystal[] _crystals;
    [SerializeField] private int _crystalAmount;

    [Header("Color")]
    [SerializeField] private Material[] _colorMaterials;
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;

    private Enemy _enemy;

    public void Initialize(Enemy enemy)
    {
        _enemy = enemy;

        _crystals = GetComponentsInChildren<EnemyCrystal>(true); //true: 꺼져있는것도가져온다

        SetUpRandomCrystal();
        SetUpRandomColor();
    }

    private void SetUpRandomColor()
    {
        int idx = Random.Range(0, _colorMaterials.Length);
        _meshRenderer.material = _colorMaterials[idx];
    }

    private void SetUpRandomCrystal()
    {
        for (int i = 0; i < _crystalAmount; i++)
        {
            int randIdx = Random.Range(0, _crystals.Length - i);
            int lastIdx = _crystals.Length - i - 1;

            (_crystals[randIdx], _crystals[lastIdx]) = (_crystals[lastIdx], _crystals[randIdx]);
        }

        for (int i = 0; i < _crystals.Length - _crystalAmount; i++)
        {
            _crystals[i].SetActiveCrystal(false);
        }
        //_crystals.ToList().ForEach(x => x.SetActiveCrystal(false));
    }
}
