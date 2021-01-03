using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToonGun : MonoBehaviour
{
    [SerializeField]
    [Range(0.2f, 1.5f)]
    private float _fireRate = 1f;

    [SerializeField]
    [Range(1f, 10f)]
    private float _damage = 1f;

    [SerializeField]
    private ParticleSystem _muzzleFlash;

    [SerializeField]
    private AudioSource _gunAudioSource;

    [SerializeField]
    private Camera _camera;

    private float _timer;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > _fireRate)
        {
            if(Input.GetMouseButtonDown(0))
            {
                _timer = 0f;
                FireGun();
            }
        }
    }

    private void FireGun()
    {
        Ray ray = _camera.ViewportPointToRay(Vector3.one * 0.5f);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo, 100))
        {
            // Damage
            //Destroy(hitInfo.collider.gameObject);
        }

        _muzzleFlash.Play();
        _gunAudioSource.Play();

        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5);
    }
}