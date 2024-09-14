using System.Collections;
using System.Collections.Generic;
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
}
