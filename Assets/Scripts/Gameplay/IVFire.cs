using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.LagCompensation;
using System.IO;
using MLAPI.Serialization.Pooled;
using UnityStandardAssets.CrossPlatformInput;

public class IVFire : NetworkedBehaviour
{
    [SerializeField]
    private GameObject m_bulletPrefab = null;

    private float m_lastShotTime;

    [SerializeField]
    private Camera m_cameraFOV;

    [SerializeField]
    private IVPlayerWeapon m_currentWeapon;

    [SerializeField]
    private IVClientPlayer m_clientPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsLocalPlayer)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > m_lastShotTime)
            {
                m_lastShotTime = Time.time + m_currentWeapon.ShootDelay;

                Fire();
            }
        }
    }

    //private void FixedUpdate()
    //{
        
    //}

    private void Fire()
    {
        using (PooledBitStream stream = PooledBitStream.Get())
        {
            using (PooledBitWriter writer = PooledBitWriter.Get(stream))
            {
                // Send Fire time to server
                writer.WriteSingle(NetworkingManager.Singleton.NetworkTime);
                writer.WriteVector3Packed(transform.position);
                writer.WriteVector3Packed(m_cameraFOV.transform.forward);

                InvokeServerRpcPerformance(FireOnServer, stream);
            }
        }

        // Instantiate bullet immediately on the client who is firing
        InstantiateBullet(transform.position, m_cameraFOV.transform.forward);
    }

    void InstantiateBullet(Vector3 pos, Vector3 direction)
    {
        NetworkedObject bullet = IVGameManager.Instance.GetBulletFromPool(pos, Quaternion.LookRotation(direction));
        IVProjectileBullet projectile = bullet.GetComponent<IVProjectileBullet>();
        projectile.SetProjectileTravelDistance(m_currentWeapon.Range);
        projectile.Spawn(pos);
    }

    [ClientRPC]
    void FireOnClient(ulong clientId, Stream stream)
    {
        //Debug.Log("FireOnClient " + clientId);
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            Vector3 shootPos = reader.ReadVector3Packed();
            Vector3 shootDirection = reader.ReadVector3Packed();

            InstantiateBullet(shootPos, shootDirection);
        }
    }

    [ServerRPC]
    void FireOnServer(ulong clientId, Stream stream)
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            float clientTime = reader.ReadSingle();
            Vector3 shootPos = reader.ReadVector3Packed();
            Vector3 shootDirection = reader.ReadVector3Packed();


            List<ulong> otherClients = IVGameManager.Instance.GetClientIdsListExcept(clientId);
            if(null != otherClients && otherClients.Count > 0)
            {
                // Call fire rpc on other clients to instantiate the bullet
                using (PooledBitWriter writer = PooledBitWriter.Get(stream))
                {
                    writer.WriteVector3Packed(shootPos);
                    writer.WriteVector3Packed(shootDirection);

                    InvokeClientRpcOnEveryoneExceptPerformance(FireOnClient, clientId, stream);
                }
            }

            PerformShootRaycast(clientTime, shootPos, shootDirection);
        }
    }

    IEnumerator DoDestroy(NetworkedObject obj, float delay)
    {
        // TODO: Pool the bullet
        yield return new WaitForSeconds(delay);
        IVGameManager.Instance.UnSpawnObject(obj);
        //obj.UnSpawn();
    }

    private void PerformShootRaycast(float clientTime, Vector3 shootPos, Vector3 shootDir)
    {
        if (!this.IsServer)
            return;

        // Round the float to 2 decimal points
        float secondsAgo = Mathf.Round((NetworkingManager.Singleton.NetworkTime - clientTime) * 100) / 100f;
        Debug.Log($"PerformShootRaycast secondsAgo: {secondsAgo}");

        LagCompensationManager.Simulate(secondsAgo, () => {
            RaycastHit hit;
            if (Physics.Raycast(shootPos, shootDir, out hit, m_currentWeapon.Range, LayerMask.GetMask(IVConstants.LAYER_REMOTE_PLAYER)))
            {
                Debug.Log("Hit the target " + hit.transform.name);
                if(hit.transform.CompareTag(IVConstants.TAG_REMOTE_PLAYER))
                {
                    IVClientPlayer targetPlayer = hit.transform.GetComponent<IVClientPlayer>();
                    targetPlayer.TakeGamage(m_currentWeapon.Damage);
                }
            }
        });
    }
}
