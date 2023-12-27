using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _h, _v, _moveSpeeed, _jumpPower, _fallPower;
    private Vector3 _dir, _velovity;

    private Rigidbody _rigid;

    private Transform _floorRayPoint;
    private Ray _floorRay, _wallRay, _flatRay;
    private RaycastHit _hit;
    [SerializeField]
    private bool _downJumpKey, _isJump, _isOnFloor;
    [SerializeField]
    private bool _onAir, _isFall;
    [SerializeField]
    private bool _isHeading;
    private float _jumpH, _jumpLimitH, _jumpMinH, _fallLimitV;

    private Transform _flat;

    private void Start()
    {
        _moveSpeeed = 7f;
        SetJumpInfo();

        SetObjectParameters();

        _rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
    }

    private void SetJumpInfo()
    {
        _jumpPower = 15;
        _fallPower = 3;
        _jumpLimitH = 5;
        _jumpMinH = 0.3f;
        _fallLimitV = -10f;
        _isJump = false;
    }

    private void SetObjectParameters()
    {
        _floorRayPoint = transform.Find("FloorRayPoint");
    }

    private void Update()
    {
        KeyInput();

        CheckOnFloor();
        CheckBottomFloor();
        SetDir();

        OnAir();
        Jump();
        transform.position += _dir.normalized * Time.deltaTime * _moveSpeeed;
    }

    private void KeyInput()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _downJumpKey = Input.GetKeyDown(KeyCode.Space);
        //_v = Input.GetAxisRaw("Vertical");
    }

    private void SetDir()
    {
        _dir.x = _h;
        _dir.y = _v;
    }

    private void OnAir()
    {
        if (_isOnFloor)
            return;

        Fall();
    }

    private void Fall()
    {
        if(!_onAir)
            return;

        _isFall = true;
        
        _rigid.AddForce(Vector3.down * _fallPower);
        if(_rigid.velocity.y <= _fallLimitV)
        {
            _velovity = _rigid.velocity;
            _velovity.y = _fallLimitV;
            _rigid.velocity = _velovity;
        }
    }

    private void Jump()
    {
        if (_isJump)
        {
            if (_jumpH + _jumpLimitH <= transform.position.y)
            {
                _rigid.velocity = Vector3.up;
                _isJump = false;
            }

            if(_jumpH + _jumpMinH <= transform.position.y)
            {
                if(_isOnFloor)
                    _isJump = false;
            }
            else
            {
                if (_isHeading)
                    _isJump = false;
            }
        }
        else
        {
            if(_downJumpKey && _isOnFloor)
            {
                _jumpH = transform.position.y;
                _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
                _isJump = true;
            }
        }
    }

    private void CheckOnFloor()
    {
        _floorRay.origin = _floorRayPoint.position;
        _floorRay.direction = Vector3.down;
        _isOnFloor = false;

        if (Physics.Raycast(_floorRay, out _hit, 0.15f, 1 << 6))
        {
            OnLand();
        }
        else
        {
            _onAir = true;
        }

        _flatRay.origin = _floorRayPoint.position;
        _flatRay.direction = Vector3.down;
        if (Physics.Raycast(_flatRay, out _hit, 0.15f, 1 << 8))
        {
            _flat = _hit.transform;
            OnFlat();
            OnLand();
        }
        else
        {
            OffFlat();
            _onAir = true;
        }
    }

    private void OnFlat()
    {
        _flat.GetComponent<Flat>().OnFlat();
    }

    private void OffFlat()
    {
        if(_flat)
            _flat.GetComponent<Flat>().OffFlat();
        
        _flat = null;
    }

    private void CheckBottomFloor()
    {
        _wallRay.origin = _floorRayPoint.position + (Vector3.up * 0.94f);
        _wallRay.direction = Vector3.up;
        _isHeading = false;

        if (Physics.Raycast(_wallRay, out _hit, 0.1f, 1 << 7))
        {
            OnHeading();
        }
    }

    private void OnHeading()
    {
        _isHeading = true;
        _rigid.velocity = Vector3.zero;
    }

    private void OnLand()
    {
        if (_rigid.velocity.y > 0)
            return;

        if (_isOnFloor)
            return;

        _rigid.velocity = Vector3.zero;
        _isOnFloor = true;
        _isFall = false;
        _onAir = false;
    }
}
