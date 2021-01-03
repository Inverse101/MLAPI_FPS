using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 500f;

    [SerializeField]
    private float _turnSpeed = 5f;

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

        _characterController.SimpleMove(movement * Time.deltaTime * _moveSpeed);

        if(_movementMagnitudeCache > 0)
        {
            Quaternion lookRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation,  lookRotation, Time.deltaTime * _turnSpeed);
        }

        _animator.SetFloat("Speed", _movementMagnitudeCache);
    }
}
