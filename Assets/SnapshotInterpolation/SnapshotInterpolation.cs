using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapshotInterpolation : MonoBehaviour
{
    public GameObject Server;
    public GameObject Client;

    struct Snapshot
    {
        public Vector3 Position;
        public float Time;
        public float DeliveryTime;
    }

    float _lastSnapshot;
    List<Snapshot> _clientSnapshots = new List<Snapshot>();

    Queue<Snapshot> _clientSimulationQueue = new Queue<Snapshot>();

    const float SNAPSHOT_INTERVAL = 0.1f;

    public float NetworkLag = 0.05f;


    ///////////////// Network Transform Sync related fields /////////
    public bool SimulateNetworkTransformSync = false;

    private Vector3 _interpolateTo;
    private Vector3 _interpolateFrom;
    private float _interpolationStartTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // On Server
        ServerMovement();
        ServerSnapshot();

        // On Client
        ClientReceiveDataFromServer();
        ClientRenderLatestPosition();
    }

    private void ClientReceiveDataFromServer()
    {
        if (_clientSimulationQueue.Count > 0 && _clientSimulationQueue.Peek().DeliveryTime < Time.time)
        {
            Snapshot snapshot = _clientSimulationQueue.Dequeue();

            if (!SimulateNetworkTransformSync)
            {
                _clientSnapshots.Add(snapshot);
            }
            else
            {
                _interpolateFrom = Client.transform.position;
                _interpolateTo = snapshot.Position;
                _interpolationStartTime = Time.time;
            }
        }
    }

    private void ClientRenderLatestPosition()
    {
        if (!SimulateNetworkTransformSync && _clientSnapshots.Count > 0)
        {
            Client.transform.position = _clientSnapshots[_clientSnapshots.Count - 1].Position;
        }

        if (SimulateNetworkTransformSync)
            Client.transform.position = Vector3.Lerp(_interpolateFrom, _interpolateTo, Mathf.Clamp01(Time.time - _interpolationStartTime) / SNAPSHOT_INTERVAL);
    }

    void ServerMovement()
    {
        Vector3 pos = default;
        pos.x = Mathf.PingPong(Time.time * 5, 10f);

        Server.transform.position = pos;
    }

    void ServerSnapshot()
    {
        if (_lastSnapshot + SNAPSHOT_INTERVAL < Time.time)
        {
            _lastSnapshot = Time.time;
            _clientSimulationQueue.Enqueue(new Snapshot()
            {
                Position = Server.transform.position,
                Time = _lastSnapshot,
                DeliveryTime = Time.time + (UnityEngine.Random.value * NetworkLag)
            });
        }
    }
}
