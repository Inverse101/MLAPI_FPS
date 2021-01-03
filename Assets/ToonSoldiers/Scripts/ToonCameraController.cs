using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ToonCameraController : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1f)]
    private float _sensitivity = 1f;

    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private CinemachineComposer _composer;

    // Start is called before the first frame update
    private void Start()
    {
        _composer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Mouse Y") * _sensitivity;
        _composer.m_TrackedObjectOffset.y += vertical;
        _composer.m_TrackedObjectOffset.y = Mathf.Clamp(_composer.m_TrackedObjectOffset.y, 0, 5);
    }
}
