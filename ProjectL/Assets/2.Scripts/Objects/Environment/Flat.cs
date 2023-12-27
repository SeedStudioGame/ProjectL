using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flat : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void OnFlat()
    {
        _collider.isTrigger = false;
    }

    public void OffFlat()
    {
        _collider.isTrigger = true;
    }
}
