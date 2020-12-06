using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LBMLAPIMode
{
    NONE,
    HOST,
    DEDICATED_SERVER,
    CLIENT
}

public class IVUtil
{
    public static LBMLAPIMode MLAPIMode = LBMLAPIMode.NONE;

    public static Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(GetRandomXPositionForLevel(), 1f, GetRandomZPositionForLevel());
    }

    private static float GetRandomXPositionForLevel()
    {
        return UnityEngine.Random.Range(-120, 120);
    }

    private static float GetRandomZPositionForLevel()
    {
        return UnityEngine.Random.Range(-70, 70);
    }
}
