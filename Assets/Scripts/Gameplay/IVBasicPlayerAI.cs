using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IVBasicPlayerAI : MonoBehaviour
{
    private float m_speed = 5f;

    [SerializeField]
    private Vector3 m_nextTargetPosition;

    [SerializeField]
    private bool m_allowMovement = true;

    private Transform m_cachedTransform;

    private void Awake()
    {
        m_cachedTransform = transform;
#if DEDICATED_SERVER && !UNITY_EDITOR
        // Read m_allowMovement from commandline arguments for Server
        string[] args = System.Environment.GetCommandLineArgs();
        if (null != args && args.Length >= 3)
        {
            m_allowMovement = System.Convert.ToBoolean(args[args.Length - 1]);
        }
#endif

    }

    // Start is called before the first frame update
    void Start()
    {
        m_nextTargetPosition = IVUtil.GetRandomSpawnPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_allowMovement)
        {
            if (Vector3.SqrMagnitude(m_cachedTransform.position - m_nextTargetPosition) < 1f)
            {
                m_nextTargetPosition = IVUtil.GetRandomSpawnPoint();
            }

            m_cachedTransform.position = Vector3.MoveTowards(m_cachedTransform.position, m_nextTargetPosition, m_speed * Time.deltaTime);
        }
    }
}
