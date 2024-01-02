using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _h, _moveSpeeed, _jumpPower, _fallPower;
    private bool _isFrontWall;
    private Vector3 _dir, _velovity, _force, _forceImpulse, _forceForce, _preDir;

    private Rigidbody _rigid;

    private Transform _floorRayPoint;
    private Ray _wallRay, _flatRay;
    private RaycastHit _hit;

    private bool _isJump;
    private int _jumpCount, _jumpCountLimt;
    private bool _downJumpKey, _isOnFloor, _jumpOnMinH;
    private float _jumpH, _jumpLimitH, _jumpMinH, _fallLimitV;

    private bool _onAir, _isFall;

    private bool _isHeading;

    private bool _downDashKey, _isDash, _dashStoped;
    private float _dashTime, _dashTimer, _dashPower;
    private float _dashLimitDist;
    private Vector3 _dashDir, _dashPos;

    private Flat _flat;
    private bool _down;

    private void Start()
    {
        _preDir = Vector3.right;
        _moveSpeeed = 10f;
        SetJumpInfo();
        SetDashInfo();

        SetObjectParameters();

        _rigid = Util.GetOrAddComponent<Rigidbody>(gameObject);
    }

    private void SetJumpInfo()
    {
        _jumpPower = 20;
        _fallPower = 8;
        _jumpLimitH = 2.7f;
        _jumpMinH = 0.1f;
        _jumpOnMinH = false;
        _fallLimitV = -18f;
        _isJump = false;
        _jumpCount = 0;
        _jumpCountLimt = 2;
    }

    private void SetDashInfo()
    {
        _dashTime = 3;
        _dashTimer = 0;
        _isDash = false;
        _dashStoped = true;
        _dashPower = 60f;
        _dashLimitDist = 3.5f;
    }

    private void SetObjectParameters()
    {
        _floorRayPoint = transform.Find("FloorRayPoint");
    }

    /*
     * 
        _velovity = _rigid.velocity;

        KeyInput();

        CheckOnFloor();
        CheckOnFlat();
        CheckFrontWall();
        CheckBottomFloor();

        SetDir();

        OnPlatform();
        OnAir();
        Jump();
        Dash();
        Move();

        AddForce();
     *
     */

    private void FixedUpdate()
    {
        KeyInput();

        CheckFrontWall();

        SetDir();

        OnPlatform();
        Move();
    }

    private void Update()
    {
        KeyInput();

        _velovity = _rigid.velocity;

        CheckOnFloor();
        CheckOnFlat();
        CheckBottomFloor();

        OnAir();
        Jump();
        Dash();

        AddForce();
    }

    private void KeyInput()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _downJumpKey = Input.GetKeyDown(KeyCode.Space);
        _downDashKey = Input.GetMouseButtonDown(1);
        if(_downDashKey)
        {
            _dashDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            _dashDir.z = 0;
            _dashDir.Normalize();
        }
        _down = Input.GetKeyDown(KeyCode.S);
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

    private void OnPlatform()
    {
        if (_down)
        {
            UnFlat();
        }
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
            JumpHeightMin();
        }
        
        if(_jumpCount < _jumpCountLimt)
        {
            if(_downJumpKey)
            {
                if(_jumpCount == 0)
                {
                    if(_isOnFloor)
                    {
                        _jumpOnMinH = false;
                        _jumpH = transform.position.y;
                        _forceImpulse += Vector3.up * _jumpPower;
                        _isJump = true;
                        _jumpCount++;
                    }
                }
                else
                {
                    _velovity.y = 0;
                    _jumpH = transform.position.y;
                    _forceImpulse += Vector3.up * _jumpPower;
                    _isJump = true;
                    _jumpCount++;
                }
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

    private void JumpHeightMin()
    {
        if (_jumpH + _jumpMinH <= transform.position.y)
        {
            _jumpOnMinH = true;
        }
    }

    private void Move()
    {
        if(IsCanMove())
            transform.position += _dir.normalized * _moveSpeeed * Time.deltaTime;
    }

    private bool IsCanMove()
    {
        return (_dashStoped && !_isFrontWall);
    }

    private void Dash()
    {
        if(_downDashKey && !_isDash)
        {
            _isDash = true;
            _dashStoped = false;
            _dashTimer = _dashTime;
            _forceImpulse += _dashDir * _dashPower;
            _dashPos = transform.position;
            UnFlat();
        }

        if(_isDash && !_dashStoped)
        {
            float dist = Vector3.Distance(_dashPos, transform.position);
            if (dist >= _dashLimitDist)
            {
                _dashStoped = true;
                _velovity = Vector3.zero;
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
        if (_isOnFloor || !_dashStoped)
            return;

        float velocity = _velovity.y;
        if(velocity < -2)
        {
            velocity = -2;
        }
        _wallRay.origin = _floorRayPoint.position;
        _wallRay.direction = Vector3.down;

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * -0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.4f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            _velovity.y = velocity;
            return;
        }

        _wallRay.origin = _floorRayPoint.position;
        if (Physics.Raycast(_wallRay, out _hit, 0.4f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            _velovity.y = velocity;
            return;
        }

        _wallRay.origin = _floorRayPoint.position + (Vector3.right * 0.3f);
        if (Physics.Raycast(_wallRay, out _hit, 0.4f, 1 << 7))
        {
            _flat = _hit.transform.GetComponent<Flat>();
            OnFlat();
            OnLand();
            _velovity.y = velocity;
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

    private void UnFlat()
    {
        if (_flat)
        {
            _flat.UnFlat();
        }
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
        _isFrontWall = false;
        _wallRay.origin = _floorRayPoint.position;
        _wallRay.direction = Vector3.right * _h;

        if (Physics.Raycast(_wallRay, out _hit, 0.55f, 1 << 6))
        {
            FrontWall();
            return;
        }

        _wallRay.origin = _floorRayPoint.position + (Vector3.up * 0.84f);

        if (Physics.Raycast(_wallRay, out _hit, 0.55f, 1 << 6))
        {
            FrontWall();
            return;
        }
    }

    private void FrontWall()
    {
        if (_h > 0 && _velovity.x > 0)
        {
            _velovity.x = 0;
        }
        if (_h < 0 && _velovity.x < 0)
        {
            _velovity.x = 0;
        }
        _isFrontWall = true;
    }

    private void OnHeading()
    {
        _isHeading = true;
    }

    private void OnLand()
    {
        if (_rigid.velocity.y > 0)
            return;

        if(_jumpOnMinH)
            _jumpCount = 0;

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

        if (_forceForce != Vector3.zero)
        {
            _rigid.AddForce(_forceForce, ForceMode.Force);
            _forceForce = Vector3.zero;
        }
    }
}
