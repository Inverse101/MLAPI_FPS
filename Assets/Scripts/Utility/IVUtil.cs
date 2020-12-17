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

    public static string CONNECT_IP
    {
        get
        {
#if DEDICATED_SERVER && !UNITY_EDITOR
            string[] args = System.Environment.GetCommandLineArgs();
            if (null != args && args.Length >= 2)
            {
                return args[args.Length-2];
            }
            return "127.0.0.1";
#else
            //return "127.0.0.1";
            return "176.57.181.227";
#endif
        }
    }

    public static ushort CONNECT_PORT
    {
        get
        {
#if DEDICATED_SERVER && !UNITY_EDITOR
            string[] args = System.Environment.GetCommandLineArgs();
            if (null != args && args.Length >= 2)
            {
                return System.Convert.ToUInt16(args[args.Length - 1]);
            }
            return 7777;
#else
            //return 7777;
            return 32001;
#endif
        }
    }

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
