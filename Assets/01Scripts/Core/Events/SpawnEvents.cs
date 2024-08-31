using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnEvents
{
    public static PlayerBulletCreate PlayerBulletCreate = new PlayerBulletCreate();
    public static EffectSpawn EffectSpawn = new EffectSpawn();
}

public class PlayerBulletCreate : GameEvent
{
    public Vector3 position;
    public Quaternion rotation;
    public BulletPayload payload;
}

public class EffectSpawn : GameEvent
{
    public PoolTypeSO effectType;
    public Vector3 point;
    public Quaternion rotation;
}