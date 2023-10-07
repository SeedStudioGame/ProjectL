using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _model;
    private bool _isStopLook;

    void Start()
    {
        _model = transform;
    }

    void Update()
    {
        if (_isStopLook)
            return;

        Look();
    }

    private void Look()
    {
        _model.LookAt(Camera.main.transform);
    }

    public void StopBillboard(bool isOn)
    {
        _isStopLook = isOn;
    }
}
