using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using System;
using MLAPI.Spawning;
using UnityEngine.SceneManagement;

public class IVNetworkInitialise : MonoBehaviour
{
    private string m_clientCode = "12345";

    [SerializeField]
    private GameObject m_playerPrefab;

    [SerializeField]
    private GameObject m_serverNPCPrefab;

    [SerializeField]
    private GameObject m_serverCamera;

    [SerializeField]
    private int m_serverNPCCount = 10;

    // Start is called before the first frame update
    void Start()
    {
        NetworkingManager.Singleton.NetworkConfig.ConnectionApproval = true;
        //clientCode = UnityEngine.Random.Range(1, 9999).ToString("0000");

        SetupNetworkTransport(IVUtil.CONNECT_IP, IVUtil.CONNECT_PORT);

#if DEDICATED_SERVER
        SetupDedicatedServer();
#else

        if (IVUtil.MLAPIMode == LBMLAPIMode.NONE)
            SceneManager.LoadSceneAsync("MenuScene");
        else if (IVUtil.MLAPIMode == LBMLAPIMode.DEDICATED_SERVER)
            SetupDedicatedServer();
        else if (IVUtil.MLAPIMode == LBMLAPIMode.HOST)
            SetupHostServer();
        else
            SetupClient();
#endif
    }

    private void SetupNetworkTransport(string ip, ushort port)
    {
        ((RufflesTransport.RufflesTransport)NetworkingManager.Singleton.NetworkConfig.NetworkTransport).ConnectAddress = ip;
        ((RufflesTransport.RufflesTransport)NetworkingManager.Singleton.NetworkConfig.NetworkTransport).Port = port;
    }

    private void OnDestroy()
    {
        if(null != NetworkingManager.Singleton && NetworkingManager.Singleton.IsClient)
        {
            
        }
    }

    public void QuitGame()
    {
        if (null != NetworkingManager.Singleton && NetworkingManager.Singleton.IsClient)
        {
            NetworkingManager.Singleton.DisconnectClient(NetworkingManager.Singleton.LocalClientId);
        }
    }

    private void SetupDedicatedServer()
    {
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

        NetworkingManager.Singleton.OnServerStarted += OnServerStarted;

        NetworkingManager.Singleton.ConnectionApprovalCallback += OnConnectionApprovalCallback;

        NetworkingManager.Singleton.StartServer();

        m_serverCamera.SetActive(true);
    }

    private void SetupHostServer()
    {
        Debug.Log("[MLAPI] SetupHostServer");
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

        NetworkingManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkingManager.Singleton.ConnectionApprovalCallback += OnConnectionApprovalCallback;

        NetworkingManager.Singleton.StartHost();
        m_serverCamera.SetActive(false);
    }

    private void SetupClient()
    {
        NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectedCallback;

        NetworkingManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(m_clientCode.ToString());
        NetworkingManager.Singleton.StartClient();

        m_serverCamera.SetActive(false);
    }

    #region Dedicated Server callbacks

    private void OnClientConnectedCallback(ulong clientId)
    {
        Debug.Log($"[MLAPI] On Client Connected: {clientId}");
    }

    private void OnClientDisconnectedCallback(ulong clientId)
    {
        Debug.Log($"[MLAPI]  On Client Disconnected: {clientId}");

    }

    private void OnServerStarted()
    {
        Debug.Log("[MLAPI] OnServerStarted");
        //if (NetworkingManager.Singleton.IsClient)
        //    InstantiateMyPlayer(NetworkingManager.Singleton.LocalClientId);

        //#if HOST
        //        if (NetworkingManager.Singleton.IsHost)
        //            InstantiateMyPlayer(NetworkingManager.Singleton.LocalClientId);
        //#endif

        if (NetworkingManager.Singleton.IsServer)
        {
            InstantiateServerNPCs();
        }
    }

    private void OnConnectionApprovalCallback(byte[] connectionData, ulong clientId, NetworkingManager.ConnectionApprovedDelegate connApprovalDel)
    {
        bool approved = false;

        try
        {
            string receivedClientCode = System.Text.Encoding.ASCII.GetString(connectionData);
            if (0 == string.Compare(m_clientCode, receivedClientCode, true))
            {
                approved = true;
            }
            Debug.Log($"[MLAPI] Approved Client: {m_clientCode} receivedClientCode: {receivedClientCode}");
            ulong? prefabHash = SpawnManager.GetPrefabHashFromGenerator("FPSController");

            connApprovalDel(true, prefabHash, approved, IVUtil.GetRandomSpawnPoint(), Quaternion.identity);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[MLAPI]:  {ex.Message}");
        }
    }

    private void InstantiateMyPlayer(ulong clientId)
    {
        GameObject playerGo = Instantiate(m_playerPrefab, IVUtil.GetRandomSpawnPoint(), Quaternion.identity);
        NetworkedObject nObj = playerGo.GetComponent<NetworkedObject>();
        nObj.SpawnAsPlayerObject(clientId);
    }

    private void InstantiateServerNPCs()
    {
        if (!NetworkingManager.Singleton.IsServer)
            return;

        GameObject npcGo = null;
        for (int index = 0; index < m_serverNPCCount; ++index)
        {
            npcGo = Instantiate(m_serverNPCPrefab, IVUtil.GetRandomSpawnPoint(), Quaternion.identity);
            NetworkedObject nObj = npcGo.GetComponent<NetworkedObject>();
            npcGo.GetComponent<IVBasicPlayerAI>().enabled = true;
            npcGo.name = $"ServerNPC_{index}";
            nObj.Spawn();

            IVGameManager.Instance.AddNPC(nObj);
        }
    }

    #endregion


    #region Client Settings



    #endregion
}
