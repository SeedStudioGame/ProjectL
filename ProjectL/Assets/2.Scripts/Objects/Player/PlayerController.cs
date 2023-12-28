using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _h, _v, _moveSpeeed, _jumpPower, _fallPower;
    private Vector3 _dir, _velovity, _force, _forceImpulse, _preDir;

    private Rigidbody _rigid;

    private Transform _floorRayPoint;
    private Ray _wallRay, _flatRay;
    private RaycastHit _hit;

    [SerializeField]
    private bool _isJump;
    private bool _downJumpKey, _isOnFloor;
    private float _jumpH, _jumpLimitH, _jumpMinH, _fallLimitV;

    private bool _onAir, _isFall;

    [SerializeField]
    private bool _isHeading;

    [SerializeField]
    private bool _downDashKey, _isDash;
    private float _dashTime, _dashTimer, _dashPower;
    [SerializeField]
    private float _dashW, _dashLimitW;
    [SerializeField]
    private Vector3 _dashDir;

    private Flat _flat;

    private void Start()
    {
        _preDir = Vector3.right;
        _moveSpeeed = 7f;
        SetJumpInfo();
        SetDashInfo();

        SetObjectParameters();

        _rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
    }

    private void SetJumpInfo()
    {
        _jumpPower = 15;
        _fallPower = 3;
        _jumpLimitH = 3;
        _jumpMinH = 0.1f;
        _fallLimitV = -10f;
        _isJump = false;
    }

    private void SetDashInfo()
    {
        _dashTime = 3;
        _dashTimer = 0;
        _isDash = false;
        _dashPower = 60f;
        _dashLimitW = 5;
    }

    private void SetObjectParameters()
    {
        _floorRayPoint = transform.Find("FloorRayPoint");
    }

    private void Update()
    {
        _velovity = _rigid.velocity;

        KeyInput();

        CheckOnFloor();
        CheckOnFlat();
        CheckFrontWall();
        CheckBottomFloor();

        SetDir();

        OnAir();
        Jump();
        Dash();
        Move();

        AddForce();
    }

    private void KeyInput()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _downJumpKey = Input.GetKeyDown(KeyCode.Space);
        _downDashKey = Input.GetMouseButtonDown(1);
    }

    private void SetDir()
    {
        _dir.x = _h;
        
        if(_dir != Vector3.zero)
            _preDir = _dir;
    }

    private void OnAir()
    {
        if (!_isOnFloor)
            _onAir = true;
        else
            return;

        Fall();
    }

    private void Fall()
    {
        _isFall = true;

        _force += Vector3.down * _fallPower;
        if(_rigid.velocity.y <= _fallLimitV)
        {
            _velovity.y = _fallLimitV;
        }
    }

    private void Jump()
    {
        if (_isJump)
        {
            if (_isHeading)
                _isJump = false;
            JumpHeightLimit();
        }
        else
        {
            if(_downJumpKey && _isOnFloor)
            {
                _jumpH = transform.position.y;
                _forceImpulse += Vector3.up * _jumpPower;
                _isJump = true;
            }
        }
    }

    private void JumpHeightLimit()
    {
        if (_jumpH + _jumpLimitH <= transform.position.y)
        {
            _velovity.y = 1;
            _isJump = false;
        }
    }

    private void Move()
    {
        transform.position += _dir.normalized * _moveSpeeed * Time.deltaTime;
    }

    private void Dash()
    {
        if(_downDashKey && !_isDash)
        {
            _isDash = true;
            _dashTimer = _dashTime;
            _dashDir = _preDir;
            _forceImpulse += _dashDir * _dashPower;
            _dashW = transform.position.x;
        }

        if(_isDash)
        {
            float dist = _dashW + (_dashDir.x * _dashLimitW) - transform.position.x;
            Debug.Log(dist);
            if (dist <= 0.5f && dist >= -0.5f)
            {
                _velovity.x = 0;
            }
        }

        if (_dashTimer >= 0)
        {
            _dashTimer -= Time.deltaTime;
        }
        else
        {
            _isDash = false;
            _dashTimer = 0;
        }
    }

    private void CheckOnFloor()
    {
        _wallRay.origin = _floorRayPoint.position;
        _wallRay.direction = Vector3.down;
        _isOnFloor = false;

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * -0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 6))
        {
            OnLand();
            return;
        }

        _wallRay.origin = _floorRayPoint.position;
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 6))
        {
            OnLand();
            return;
        }

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * 0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 6))
        {
            OnLand();
            return;
        }
    }

    private void CheckOnFlat()
    {
        if (_isOnFloor)
            return;

        _wallRay.origin = _floorRayPoint.position;
        _wallRay.direction = Vector3.down;

        Debug.DrawLine(_floorRayPoint.position, _wallRay.origin + (Vector3.down * 0.2f), Color.red);

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * -0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            return;
        }

        _wallRay.origin = _floorRayPoint.position;
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            return;
        }

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * 0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.2f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            return;
        }
        OffFlat();
    }

    private void OnFlat()
    {
        if (_flat)
        {
            _flat.GetComponent<Flat>().OnFlat();
        }
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

        _wallRay.origin += (Vector3.right * -0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.1f, 1 << 6))
        {
            OnHeading();
            return;
        }

        _wallRay.origin += (Vector3.right * 0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.1f, 1 << 6))
        {
            OnHeading();
            return;
        }

        _wallRay.origin += (Vector3.right * 0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.1f, 1 << 6))
        {
            OnHeading();
            return;
        }
    }

    private void CheckFrontWall()
    {
        _wallRay.origin = _floorRayPoint.position;
        _wallRay.direction = Vector3.right * _h;

        if (Physics.Raycast(_wallRay, out _hit, 0.55f, 1 << 6))
        {
            _h = 0;
            _velovity.x = 0;
            return;
        }

        _wallRay.origin = _floorRayPoint.position + (Vector3.up * 0.84f);

        if (Physics.Raycast(_wallRay, out _hit, 0.55f, 1 << 6))
        {
            _h = 0;
            _velovity.x = 0;
            return;
        }
    }

    private void OnHeading()
    {
        _isHeading = true;
        //_velovity.y = 0;
    }

    private void OnLand()
    {
        if (_rigid.velocity.y > 0)
            return;

        //if (_onAir)
            _velovity.y = -1;

        _isOnFloor = true;
        _isFall = false;
        _onAir = false;
    }

    private void AddForce()
    {
        if (_velovity != Vector3.zero || _velovity != _rigid.velocity)
        {
            _rigid.velocity = _velovity;
            _velovity = Vector3.zero;
        }

        if (_force != Vector3.zero)
        {
            _rigid.AddForce(_force);
            _force = Vector3.zero;
        }

        if (_forceImpulse != Vector3.zero)
        {
            _rigid.AddForce(_forceImpulse, ForceMode.Impulse);
            _forceImpulse = Vector3.zero;
        }
    }
}
