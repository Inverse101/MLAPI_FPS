using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Connection;

public class IVGameManager : NetworkedBehaviour
{
    public static IVGameManager Instance;

    [HideInInspector]
    public uint ClientTick;

    [HideInInspector]
    public uint LastRecievedServerTick;

    [SerializeField]
    private int m_poolSize = 10;

    private NetworkedObject[] m_netowrkedBulletPool;

    [SerializeField]
    private GameObject m_bulletPrefab = null;

    [SerializeField]
    private Text m_ping;

    [SerializeField]
    private float m_visInterval = 1;

    [SerializeField]
    public float m_visRange = 20;

    [SerializeField]
    public bool m_useHitScan = false;

    private float m_lastVisCheckTime = 0f;

    private float m_lastIntervalledUpdate = 0f;

    private float m_updateInterval = 0.5f;

    private bool m_visibilityCheckRunning = false;

    private List<NetworkedObject> m_serverNPCList;

    private bool m_isNetworkReady = false;

    private void Awake()
    {
        Instance = this;
        m_serverNPCList = new List<NetworkedObject>(50);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_netowrkedBulletPool = new NetworkedObject[m_poolSize];

        for(int i = 0; i < m_poolSize; ++i)
        {
            GameObject bullet = Instantiate(m_bulletPrefab, Vector3.one * 100, Quaternion.identity);
            bullet.name = string.Format("Bullet_{0}", i);
            NetworkedObject obj = bullet.GetComponent<NetworkedObject>();
            m_netowrkedBulletPool[i] = obj;
            bullet.SetActive(false);
        }

        SetupSpawnPooling();

        m_lastVisCheckTime = Time.time;
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    ClientTick++;
    //}

    public override void NetworkStart()
    {
        base.NetworkStart();
        m_isNetworkReady = true;
    }

    private void Update()
    {
        if (!m_isNetworkReady)
            return;

        // Code which executes only on server
        if(NetworkingManager.Singleton.IsServer)
        {
            if(Time.time > m_lastVisCheckTime+m_visInterval && !m_visibilityCheckRunning)
            {
                m_lastVisCheckTime = Time.time;
                StartCoroutine(CheckVisibilityInMultipleFrames());
            }
        }
        else if (!NetworkingManager.Singleton.IsHost && Time.time > m_lastIntervalledUpdate + m_updateInterval)
        {
            m_lastIntervalledUpdate = Time.time;

            // Tasks we want to perform ever 500ms
            UpdatePing();
        }
    }

    #region Networked pool
    void SetupSpawnPooling()
    {
        SpawnManager.RegisterSpawnHandler(SpawnManager.GetPrefabHashFromGenerator("Bullet"), BulletSpawnHandler);
        SpawnManager.RegisterCustomDestroyHandler(SpawnManager.GetPrefabHashFromGenerator("Bullet"), BulletDestroyHandler);
    }

    private NetworkedObject BulletSpawnHandler(Vector3 position, Quaternion rotation)
    {
        return GetBulletFromPool(position, rotation);
    }

    private void BulletDestroyHandler(NetworkedObject networkedObject)
    {
        UnSpawnObject(networkedObject);
    }

    public NetworkedObject GetBulletFromPool(Vector3 position, Quaternion rotation)
    {
        NetworkedObject bullet = null;
        foreach (var obj in m_netowrkedBulletPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                //Debug.Log("Activating GameObject " + obj.name + " at " + position);
                bullet = obj;
                break;
            }
        }
        if(null == bullet)
        {
            //Debug.LogError("Could not grab GameObject from pool, Choosing the first bullet anyways");
            bullet = m_netowrkedBulletPool[0];
        }
        
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void UnSpawnObject(NetworkedObject spawned)
    {
        spawned.gameObject.SetActive(false);
        spawned.transform.position = Vector3.zero;
    }

    public void UnSpawnObject(GameObject spawned)
    {
        spawned.SetActive(false);
        spawned.transform.position = Vector3.zero;
    }

    #endregion

    public List<ulong> GetClientIdsListExcept(ulong clientId)
    {
        List<ulong> clients = new List<ulong>(NetworkingManager.Singleton.ConnectedClientsList.Count);
        for(int index = 0; index < NetworkingManager.Singleton.ConnectedClientsList.Count; ++index)
        {
            NetworkedClient client = NetworkingManager.Singleton.ConnectedClientsList[index];
            if(client.ClientId.CompareTo(clientId) != 0)
            {
                clients.Add(client.ClientId);
            }
        }
        return clients;
    }

    IEnumerator CheckVisibilityInMultipleFrames()
    {
        if ((!NetworkingManager.Singleton.IsServer || NetworkingManager.Singleton.IsHost)
            || null == NetworkingManager.Singleton.ConnectedClientsList || NetworkingManager.Singleton.ConnectedClientsList.Count < 1)
            yield break;

        //Debug.Log($"[LBGameManager] CheckVisibilityInMultipleFrames: {NetworkingManager.Singleton.ConnectedClientsList.Count}");

        m_visibilityCheckRunning = true;

        // Manage visibility of Server owned objects
        for (int index = 0; index < NetworkingManager.Singleton.ConnectedClientsList.Count; ++index)
        {
            NetworkedObject client = NetworkingManager.Singleton.ConnectedClientsList[index].PlayerObject;
            Vector3 pos1 = client.transform.position;
            for (int otherIndex = 0; otherIndex < m_serverNPCList.Count; ++otherIndex)
            {
                NetworkedObject remote = m_serverNPCList[otherIndex];
                if (remote == client)
                    continue;

                HandleVisibilityOn(client, remote);
            }

            yield return null;
        }

        // Manage visibility of all clients
        for (int index = 0; index < NetworkingManager.Singleton.ConnectedClientsList.Count; ++index)
        {
            NetworkedObject client = NetworkingManager.Singleton.ConnectedClientsList[index].PlayerObject;
            Vector3 pos1 = client.transform.position;
            for (int otherIndex = 0; otherIndex < NetworkingManager.Singleton.ConnectedClientsList.Count; ++otherIndex)
            {
                NetworkedObject remote = NetworkingManager.Singleton.ConnectedClientsList[otherIndex].PlayerObject;
                if (remote == client)
                    continue;

                HandleVisibilityOn(client, remote);
            }

            yield return null;
        }

        m_visibilityCheckRunning = false;
    }

    private void HandleVisibilityOn(NetworkedObject client, NetworkedObject remoteObj)
    {
        if (null == client || null == remoteObj)
            return;

        float distance = Vector3.Distance(client.transform.position, remoteObj.transform.position);

        if (distance <= m_visRange && !remoteObj.IsNetworkVisibleTo(client.OwnerClientId))
        {
            remoteObj.NetworkShow(client.OwnerClientId);
            //Debug.Log($"Calling show for {client.OwnerClientId}");
        }
        else if (distance >= m_visRange && remoteObj.IsNetworkVisibleTo(client.OwnerClientId))
        {
            remoteObj.NetworkHide(client.OwnerClientId);
            //Debug.Log($"Calling Hide for {remoteObj.OwnerClientId}");
        }
    }

    public void AddNPC(NetworkedObject npc)
    {
        if(!m_serverNPCList.Contains(npc))
            m_serverNPCList.Add(npc);
    }

    public void RemoveNPC(NetworkedObject npc)
    {
        if (m_serverNPCList.Contains(npc))
            m_serverNPCList.Remove(npc);
    }

    public void ClearNPCList()
    {
        m_serverNPCList.Clear();
    }

    private void UpdatePing()
    {
        // Commented because its throwing KeyNotFound Exception in Ruffle Transport
        if (m_isNetworkReady)
            m_ping.text = string.Format("Ping: {0}", NetworkingManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(NetworkingManager.Singleton.ServerClientId));
    }
}
