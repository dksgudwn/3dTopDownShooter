using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueEvents
{
    public static CamDistance CamDistanceEvent = new CamDistance();
}

public class CamDistance : GameEvent
{
    public float diatance;
}
