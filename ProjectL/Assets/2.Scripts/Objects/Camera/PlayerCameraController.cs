using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    [SerializeField]
    private Vector3 _weight;

    private void Start()
    {
        _weight = transform.position;
    }

    private void Update()
    {
        transform.position = _player.position + _weight;
    }
}
