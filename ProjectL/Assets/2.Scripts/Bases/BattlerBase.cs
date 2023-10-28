using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base of battle objects
public class BattlerBase : MonoBehaviour
{
    private int _speed, firstAttack;
    public Transform _transform;

    public int maxSpeed;
    public int minSpeed;

    public int nowCardLen;

    public void BattlerInit(int maxSpeed, int minSpeed, int firstAttack, int nowCardLen)
    {
        this.maxSpeed = maxSpeed;
        this.minSpeed = minSpeed;

        this.firstAttack = firstAttack;

        _transform = GetComponent<Transform>();

        _speed = Random.Range(minSpeed, maxSpeed);
    }

    public int GetSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }



}
