using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IVBasicPlayerAI : MonoBehaviour
{
    private float m_speed = 5f;

    [SerializeField]
    private Vector3 m_nextTargetPosition;

    private Transform m_cachedTransform;

    private void Awake()
    {
        m_cachedTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_nextTargetPosition = IVUtil.GetRandomSpawnPoint();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(Vector3.SqrMagnitude(m_cachedTransform.position - m_nextTargetPosition) < 1f)
    //    {
    //        m_nextTargetPosition = IVUtil.GetRandomSpawnPoint();
    //    }

    //    m_cachedTransform.position = Vector3.MoveTowards(m_cachedTransform.position, m_nextTargetPosition, m_speed * Time.deltaTime);
    //}
}
