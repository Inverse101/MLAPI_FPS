using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;

public class IVProjectileBullet : MonoBehaviour
{
    [SerializeField]
    private int m_bulletSpeed = 100;

    private float m_bulletTravelDistance = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetProjectileTravelDistance(float distance)
    {
        m_bulletTravelDistance = distance;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * m_bulletSpeed * Time.deltaTime);

        if(transform.position.magnitude > m_bulletTravelDistance)
        {
            IVGameManager.Instance.UnSpawnObject(gameObject);
        }
    }

    public void Spawn(Vector3 position)
    {
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(IVConstants.LAYER_REMOTE_PLAYER))
        {
            // Spawn blood effect
            
        }
        else
        {
            // Spawn effect based on surface
        }

        IVGameManager.Instance.UnSpawnObject(gameObject);
    }
}
