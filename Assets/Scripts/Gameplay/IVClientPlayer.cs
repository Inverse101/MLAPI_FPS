using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkedVar;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using System.IO;

public class IVClientPlayer : NetworkedBehaviour
{
    [HideInInspector]
    private NetworkedVar<int> m_health;

    [SerializeField]
    private GameObject m_playerUIPrefab;

    [SerializeField]
    private FirstPersonController m_fpsController;

    [SerializeField]
    private IVFire m_fire;

    [SerializeField]
    private GameObject[] m_objectsToDisable;

    [SerializeField]
    private NetworkedObject m_networkedObject;

    private GameObject m_playerUI;

    private Transform m_cachedTransform;

    private void Awake()
    {
        m_cachedTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(this.IsClient)
        {
            if (this.IsLocalPlayer)
            {
                gameObject.layer = LayerMask.NameToLayer(IVConstants.LAYER_PLAYER);
                m_playerUI = Instantiate(m_playerUIPrefab);
                m_playerUI.name = m_playerUIPrefab.name;
                m_playerUI.SetActive(true);
                m_health.OnValueChanged += OnHealthUpdate;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer(IVConstants.LAYER_REMOTE_PLAYER);
                m_fpsController.enabled = false;
                DisableObjects();
            }
        }

        // If this is server
        if (this.IsServer)
        {
            // Only server / host can set the health
            SetHealth(100);

            // If we are on dedicated server then disable FPS controller and change the layer to RemotePlayer
            if (!this.IsClient)
            {
                DisableObjects();
                gameObject.layer = LayerMask.NameToLayer(IVConstants.LAYER_REMOTE_PLAYER);
            }
        }
    }

    public override void NetworkStart(Stream stream)
    {
        base.NetworkStart();
        if (!this.IsServer)
            return;

        m_networkedObject.CheckObjectVisibility += HandleCheckObjectVisibility;
    }

    private void DisableObjects()
    {
        m_fpsController.enabled = false;

        // Disable Audio source and listener if there is any
        AudioListener listener = GetComponentInChildren<AudioListener>();
        if (null != listener)
            listener.enabled = false;

        AudioSource source = GetComponentInChildren<AudioSource>();
        if (null != source)
            source.enabled = false;

        foreach (var obj in m_objectsToDisable)
            obj.SetActive(false);
    }

    private void OnHealthUpdate(int previousValue, int newValue)
    {
        // Update local UI

    }

    private void OnDisable()
    {
        if(null != m_playerUI)
        {
            m_playerUI.SetActive(false);
        }
    }

    public void SetHealth(int health)
    {
        m_health.Value = health;
    }

    public void TakeGamage(int damage)
    {
        m_health.Value -= damage;

        if(m_health.Value <= 0)
        {
            GetComponent<NetworkedObject>().UnSpawn();
            Destroy(gameObject);
        }
    }

    private bool HandleCheckObjectVisibility(ulong clientId)
    {
        // Return true for same client
        if (clientId == this.OwnerClientId)
            return true;

        // return true to show the object, return false to hide it
        float distance = Vector3.Distance(NetworkingManager.Singleton.ConnectedClients[clientId].PlayerObject.transform.position, m_cachedTransform.position);
        //Debug.Log($"Client: {clientId} Distance: {distance}");
        if (distance < IVGameManager.Instance.m_visRange)
        {
            // Only show the object to players that are within m_networkVisibility meters. Note that this has to be rechecked by your own code
            // If you want it to update as the client and objects distance change.
            // This callback is usually only called once per client
            return true;
        }
        else
        {
            // Dont show this object
            return false;
        }
    }
}
