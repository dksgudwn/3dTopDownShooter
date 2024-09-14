using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Idle info")]
    public float idleTime;
    public float aggressiveRange;

    [Header("Move data")]
    public float moveSpeed;
    public float turnSpeed;
    public float chaseSpeed;

    [Header("Patrol info")]
    [SerializeField] private Transform[] _patrolPoints;
    private int _currentPatrolIdx;

    [Header("Target info")]
    [SerializeField] private PlayerManagerSO _playerManager;
    public Player Target => _playerManager.Player;
    public Transform TargetTrm => _playerManager.PlayerTrm;

    public bool PlayerSpotted { get; protected set; }
    private bool _isEnterBattleMode;

    public EnemyStateMachine StateMachine { get; private set; }
    public bool IsDead { get; private set; }

    private Dictionary<Type, IEnemyComponent> _components;

    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();
        _components = new Dictionary<Type, IEnemyComponent>();
        GetComponentsInChildren<IEnemyComponent>().ToList().ForEach(compo => _components.Add(compo.GetType(), compo));

        _components.Values.ToList().ForEach(compo => compo.Initialize(this));
    }

    protected virtual void Start()
    {
        foreach (var point in _patrolPoints)
            point.SetParent(null);
    }

    private void Update()
    {
        if (StateMachine.CurrentState != null)
            StateMachine.UpdateMachine();

        CheckEnterBattleMode();
    }

    private void CheckEnterBattleMode()
    {
        if (_isEnterBattleMode == false && IsPlayerInAggressiveRange())
        {
            _isEnterBattleMode = true;
            EnteringBattleMode();
        }
    }

    private bool IsPlayerInAggressiveRange()
        => Vector3.Distance(TargetTrm.position, transform.position) < aggressiveRange;


    public Vector3 GetPatrolPosition()
    {
        Vector3 point = _patrolPoints[_currentPatrolIdx].position;
        _currentPatrolIdx = (_currentPatrolIdx + 1) % _patrolPoints.Length;
        return point;
    }

    public void FaceToTarget(Vector3 target)
    {
        Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
        Vector3 eulerAngles = transform.rotation.eulerAngles;

        float yRot = Mathf.LerpAngle(eulerAngles.y, targetRot.eulerAngles.y, turnSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(eulerAngles.x, yRot, eulerAngles.z);
    }


    public T GetCompo<T>() where T : class
    {
        if (_components.TryGetValue(typeof(T), out IEnemyComponent compo))
        {
            return compo as T;
        }
        return default;
    }
    public abstract EnemyState GetState(Enum enumType);
    public abstract void EnteringBattleMode();

    public void OnAnimationEnd()
    {
        StateMachine.CurrentState.SetAnimationTrigger();
    }

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggressiveRange);
        Gizmos.color = Color.white;
    }

#endif

}
