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

    [SerializeField]
    private bool m_drawDebugRays = false;

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
        Vector3 firePosition = m_cameraFOV.transform.position;
        Vector3 fireDirection = m_cameraFOV.transform.forward;

        // Do server side hit detection only when there is a hit on client side. Just to save some server performance
        bool shouldVerifyHitOnServer = true;

        if (IVGameManager.Instance.m_useHitScan)
            shouldVerifyHitOnServer = FireHitScan(firePosition, fireDirection);
        else
            shouldVerifyHitOnServer = InstantiateBullet(firePosition, fireDirection); // Instantiate bullet immediately on the client who is firing

        using (PooledBitStream stream = PooledBitStream.Get())
        {
            using (PooledBitWriter writer = PooledBitWriter.Get(stream))
            {
                writer.WriteBool(shouldVerifyHitOnServer);
                writer.WriteVector3Packed(firePosition);
                writer.WriteVector3Packed(fireDirection);

                InvokeServerRpcPerformance(FireOnServer, stream);
            }
        }
    }

    bool FireHitScan(Vector3 pos, Vector3 direction)
    {
        if(m_drawDebugRays)
            Debug.DrawRay(pos, direction * m_currentWeapon.Range, Color.red, 60, false);

        RaycastHit hit;
        if (Physics.Raycast(pos, direction, out hit, m_currentWeapon.Range, LayerMask.GetMask(IVConstants.LAYER_REMOTE_PLAYER)))
        {
            Debug.Log("[Client] Hit the target " + hit.transform.name);
            if (hit.transform.CompareTag(IVConstants.TAG_REMOTE_PLAYER))
            {
                // Spawn blood effect
                return true;
            }
            else
            {
                // Spawn effect based on surface
                return false;
            }
        }
        
        return false;
    }

    bool InstantiateBullet(Vector3 pos, Vector3 direction)
    {
        NetworkedObject bullet = IVGameManager.Instance.GetBulletFromPool(pos, Quaternion.LookRotation(direction));
        IVProjectileBullet projectile = bullet.GetComponent<IVProjectileBullet>();
        projectile.SetProjectileTravelDistance(m_currentWeapon.Range);
        projectile.Spawn(pos);
        return true;
    }

    [ClientRPC]
    void FireOnClient(ulong clientId, Stream stream)
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            Vector3 shootPos = reader.ReadVector3Packed();
            Vector3 shootDirection = reader.ReadVector3Packed();

            if (IVGameManager.Instance.m_useHitScan)
                FireHitScan(shootPos, shootDirection);
            else
                InstantiateBullet(shootPos, shootDirection);
        }
    }

    [ServerRPC]
    void FireOnServer(ulong clientId, Stream stream)
    {
        using (PooledBitReader reader = PooledBitReader.Get(stream))
        {
            bool shouldDoHitDetection = reader.ReadBool();
            Vector3 shootPos = reader.ReadVector3Packed();
            Vector3 shootDirection = reader.ReadVector3Packed();

            // Call fire rpc on other clients to instantiate the bullet
            using (PooledBitWriter writer = PooledBitWriter.Get(stream))
            {
                writer.WriteVector3Packed(shootPos);
                writer.WriteVector3Packed(shootDirection);

                InvokeClientRpcOnEveryoneExceptPerformance(FireOnClient, clientId, stream);
            }

            if(shouldDoHitDetection)
                PerformShootRaycast(0f, shootPos, shootDirection);
        }
    }

    IEnumerator DoDestroy(NetworkedObject obj, float delay)
    {
        // TODO: Pool the bullet
        yield return new WaitForSeconds(delay);
        IVGameManager.Instance.UnSpawnObject(obj);
        //obj.UnSpawn();
    }


    // [Note] Lad Compensation is still not working fine. The problem is because of client and server time mismatch.
    private void PerformShootRaycast(float clientTime, Vector3 shootPos, Vector3 shootDir)
    {
        if (!this.IsServer)
            return;

        // [Hack]Round the float to 2 decimal points otherwisse we will get KeyNotFound exception
        //float secondsAgo = Mathf.Round((NetworkingManager.Singleton.NetworkTime - clientTime) * 100) / 100f;

        // We will use round trip time instead of client time.
        float rttInSeconds = this.IsHost ? 0f : NetworkingManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(this.OwnerClientId) / 1000f;

        // Mathf.Abs is a Hack to avoid negative value
        float secondsAgo = (rttInSeconds / 2f);

        Debug.Log($"PerformShootRaycast secondsAgo: {secondsAgo} Rtt: {rttInSeconds}");

        LagCompensationManager.Simulate(secondsAgo, () =>
        {
            RaycastHit hit;
            if (Physics.Raycast(shootPos, shootDir, out hit, m_currentWeapon.Range, LayerMask.GetMask(IVConstants.LAYER_REMOTE_PLAYER)))
            {
                Debug.Log("[Server] Hit the target " + hit.transform.name);
                if (hit.transform.CompareTag(IVConstants.TAG_REMOTE_PLAYER))
                {
                    IVClientPlayer targetPlayer = hit.transform.GetComponent<IVClientPlayer>();
                    //targetPlayer.TakeGamage(m_currentWeapon.Damage);
                }
            }
            if (m_drawDebugRays)
                Debug.DrawRay(shootPos, shootDir * m_currentWeapon.Range, Color.green, 60, false);
        });
    }
}
