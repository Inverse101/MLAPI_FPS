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

    float               _lastSnapshot;
    List<Snapshot>      _clientSnapshots            = new List<Snapshot>();

    Queue<Snapshot>     _clientSimulationQueue      = new Queue<Snapshot>();

    const float         SNAPSHOT_INTERVAL           = 0.1f;

    /// <summary>
    /// Currently this is hardcoded to 200ms. We can dynamically calculate this based on latency, jitter etc to be
    /// as close to the server player and provide smooth movement
    /// </summary>
    const float         INTERPOLATION_OFFSET        = 0.2f;

    public float        NetworkLag                  = 0.05f;

    private float       _clientInterpolationTime;

    private Vector3     _interpolateTo;
    private Vector3     _interpolateFrom;

    private float       _interpolateAlpha;

    ///////////////// Network Transform Sync related fields /////////
    public bool         SimulateNetworkTransformSync = false;

    private float       _interpolationStartTime;

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
        if (!SimulateNetworkTransformSync)
        {
            while(_clientSimulationQueue.Count > 0 && _clientSimulationQueue.Peek().DeliveryTime < Time.time)
            {
                // First Snapshot
                if(_clientSnapshots.Count == 0)
                    _clientInterpolationTime = _clientSimulationQueue.Peek().Time - INTERPOLATION_OFFSET;

                _clientSnapshots.Add(_clientSimulationQueue.Dequeue());
            }
        }
        else
        {
            Snapshot snapshot = _clientSimulationQueue.Dequeue();
            _interpolateFrom = Client.transform.position;
            _interpolateTo = snapshot.Position;
            _interpolationStartTime = Time.time;
        }
    }

    private void ClientRenderLatestPosition()
    {
        if (!SimulateNetworkTransformSync)
        {
            if(_clientSnapshots.Count > 0)
            {
                // Improvement: This needs to be adjusted when there is time sync issue
                _clientInterpolationTime += Time.unscaledDeltaTime;

                _interpolateFrom = default(Vector3);
                _interpolateTo = default(Vector3);
                _interpolateAlpha = default(float);

                for (int i = 0; i < _clientSnapshots.Count; ++i)
                {
                    if (i + 1 == _clientSnapshots.Count)
                    {
                        if (_clientSnapshots[0].Time > _clientInterpolationTime)
                        {
                            _interpolateFrom = _interpolateTo = _clientSnapshots[0].Position;
                            _interpolateAlpha = 0;
                        }
                        else
                        {
                            _interpolateFrom = _interpolateTo = _clientSnapshots[i].Position;
                            _interpolateAlpha = 0;
                        }
                    }
                    else
                    {
                        //                c
                        // [0][1][2][3][4][5][6][7][8][9]
                        //              f  t
                        int f = i, t = i + 1;

                        if (_clientSnapshots[f].Time <= _clientInterpolationTime && _clientSnapshots[t].Time >= _clientInterpolationTime)
                        {
                            _interpolateFrom = _clientSnapshots[f].Position;
                            _interpolateTo = _clientSnapshots[t].Position;

                            // f = 101.4
                            // v = 101.467
                            // t = 101.5
                            var range = _clientSnapshots[t].Time - _clientSnapshots[f].Time;
                            var current = _clientSnapshots[t].Time - _clientInterpolationTime;

                            _interpolateAlpha = 1 - Mathf.Clamp01(current / range);

                            break;
                        }
                    }
                }


                Client.transform.position = Vector3.Lerp(_interpolateFrom, _interpolateTo, _interpolateAlpha);
            }
        }
        else if (SimulateNetworkTransformSync)
        {
            Client.transform.position = Vector3.Lerp(_interpolateFrom, _interpolateTo, Mathf.Clamp01(Time.time - _interpolationStartTime) / SNAPSHOT_INTERVAL);
        }
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
