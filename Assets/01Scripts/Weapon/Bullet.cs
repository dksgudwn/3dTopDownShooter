using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//모든 총알이 사용할 데이터를 공용클래스로 만들어서 재사용한다.
public class BulletPayload
{
    public float mass;
    public Vector3 velocity;
    public float shootingRange;
    public float impactForce;
    public float damage;
}
public class Bullet : MonoBehaviour, IPoolable
{
    protected float _maxDistance;
    protected Vector3 _startPosition;
    public Rigidbody RbCompo { get; protected set; }

    [field: SerializeField] public PoolTypeSO PoolType { get; private set; }

    public GameObject GameObject => gameObject;

    protected float _impactForce, _damage;
    protected TrailRenderer _trailRenderer;

    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private PoolTypeSO _impactType;

    private Pool _myPool;

    protected virtual void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        RbCompo = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, _startPosition);
        if (distance >= _maxDistance * 0.6f)
        {
            _trailRenderer.time -= 2 * Time.deltaTime;
        }

        if (distance >= _maxDistance)
        {
            _myPool.Push(this);

            Debug.Log(distance);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactVFX(collision);
        _myPool.Push(this);
    }

    private void CreateImpactVFX(Collision other)
    {
        if (other.contacts.Length > 0)
        {
            ContactPoint contact = other.contacts[0];
            var evt = SpawnEvents.EffectSpawn;
            evt.effectType = _impactType;
            evt.point = contact.point;
            evt.rotation = Quaternion.LookRotation(contact.normal);
            _spawnChannel.RaiseEvent(evt);
        }
    }

    public void Fire(Vector3 position, Quaternion rotation, BulletPayload payload)
    {
        transform.SetPositionAndRotation(position, rotation);
        _trailRenderer.Clear();
        RbCompo.mass = payload.mass;
        RbCompo.velocity = payload.velocity;
        _maxDistance = payload.shootingRange;
        _impactForce = payload.impactForce;
        _damage = payload.damage;
        _startPosition = position;
    }

    public void SetUpPool(Pool pool)
    {
        _myPool = pool;
    }

    public void ResetItem()
    {
        _trailRenderer.time = 0.25f;
    }
}
