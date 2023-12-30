using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flat : MonoBehaviour
{
    private Collider _collider;
    private bool _unFlat;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _unFlat = false;
    }

    public void UnFlat()
    {
        _unFlat = true;
        _collider.isTrigger = true;
    }

    public void OnFlat()
    {
        if (!_unFlat)
            _collider.isTrigger = false;
    }

    public void OffFlat()
    {
        _collider.isTrigger = true;
        _unFlat = false;
    }
}
