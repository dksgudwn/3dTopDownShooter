using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectPlayer : MonoBehaviour, IPoolable
{
    [field: SerializeField] public PoolTypeSO PoolType { get; private set; }

    public GameObject GameObject => gameObject;

    private List<ParticleSystem> _particles;

    private Pool _myPool;
    private float _lifrDuration;

    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>().ToList();
        _lifrDuration = _particles[0].main.duration;
    }

    public void PlayEffect(Vector3 position, Quaternion rotiation)
    {
        transform.SetPositionAndRotation(position, rotiation);
        foreach (ParticleSystem particle in _particles)
        {
            particle.Play();

        }
        DOVirtual.DelayedCall(_lifrDuration, () => _myPool.Push(this));
    }
    public void ResetItem()
    {
        foreach (ParticleSystem particle in _particles)
        {
            particle.Stop();
            particle.Simulate(0);
        }
    }

    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }
}
