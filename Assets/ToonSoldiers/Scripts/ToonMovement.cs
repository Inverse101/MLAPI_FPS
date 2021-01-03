using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonMovement : MonoBehaviour
{
    [SerializeField]
    private float _forwardMoveSpeed = 7.5f;

    [SerializeField]
    private float _backwardMoveSpeed = 4f;

    [SerializeField]
    private float _turnSpeed = 150f;

    private Animator _animator;

    private float _movementMagnitudeCache;

    private CharacterController _characterController;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var movement = new Vector3(horizontal, 0, vertical);
        _movementMagnitudeCache = movement.magnitude;

        _animator.SetFloat("Speed", vertical);

        transform.Rotate(Vector3.up, horizontal * _turnSpeed * Time.deltaTime);

        if(vertical != 0)
        {
            float moveSpeedToUse = (vertical > 0) ? _forwardMoveSpeed : _backwardMoveSpeed;
            _characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);
        }
    }
}
