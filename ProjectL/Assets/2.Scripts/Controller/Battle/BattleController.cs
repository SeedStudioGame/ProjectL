using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private BattlerBase _player;
    private BattlerBase[] _enemys;
    private int _enemyCount;
    private bool _isEndBattle, _isWin;

    [SerializeField]
    private List<Vector3[]> _enemyPoint = new List<Vector3[]>();
    [SerializeField]
    private Vector3 _playerPoint, _fightPointL, _fightPointR;

    private enum BattleState
    {
        Ready, DeckSetting, Fight
    }
    private BattleState _state;

    public void BattleInit(BattlerBase playerInfo, BattlerBase[] enemysInfo)
    {
        _state = BattleState.Ready;

        _player = playerInfo;
        _enemys = enemysInfo;

        _enemyCount = _enemys.Length;

        _isEndBattle = false;
        _isWin = false;

        BattlerPosSet();
        _state = BattleState.DeckSetting;
    }

    private void BattlerPosSet()
    {
        _player = _player.GetComponent<BattlerBase>();
        _player._transform.position = _playerPoint;

        for(int i = 0; i < _enemyCount; i++)
        {
            _enemys[i] = _enemys[i].GetComponent<BattlerBase>();
            _enemys[i]._transform.position = _enemyPoint[_enemyCount][i];
        }

    }

    public void OnClickStartFight()
    {
        _state = BattleState.Fight;
        fightCount = _enemyCount;
        nowFightCount = 0;
        SetUI();
        Fight();
    }

    private void SetUI()
    {
        switch(_state)
        {
            case BattleState.Ready:
                break;
            case BattleState.Fight:
                break;
            case BattleState.DeckSetting:
                break;
        }
    }

    int targetIndex, fightCount, nowFightCount;
    private void Fight()
    {
        if(IsEndAllFight())
        {
            OnEndTurn();
        }
        targetIndex = Targeting();
        SetFightPosition(targetIndex);
        nowFightCount++;
    }

    private bool IsEndAllFight()
    {
        return (nowFightCount >= fightCount);
    }

    private int Targeting()
    {
        int targetIndex = 0;
        for(int i = 0; i < _enemys.Length; i++)
        {
            
        }
        return targetIndex;
    }


    private bool _isOnFightPosL, _isOnFightPosR;
    private void SetFightPosition(int enemyIndex)
    {
        _isOnFightPosL = false;
        _isOnFightPosR = false;

        MoveTo(_player, _fightPointL, _isOnFightPosL);
        MoveTo(_enemys[enemyIndex], _fightPointR, _isOnFightPosR);
    }
    private void SetOriginPosition(int enemyIndex)
    {
        _isOnFightPosL = true;
        _isOnFightPosR = true;

        MoveTo(_player, _playerPoint, _isOnFightPosL);
        MoveTo(_enemys[enemyIndex], _enemyPoint[_enemyCount][enemyIndex], _isOnFightPosR);
    }

    private IEnumerator MoveTo(BattlerBase target, Vector3 pos, bool side)
    {
        Transform _target = target._transform;
        while (_target.position != pos)
        {
            //Move Method



            return null;
        }

        side = !side;
        OnPos();
        return null;
    }

    private void OnPos()
    {
        if (_isOnFightPosL && _isOnFightPosR)
        {
            UseDots();
        }
        if (!_isOnFightPosL && !_isOnFightPosR)
        {
            Fight();
        }
    }

    private void UseDots()
    {


        SetOriginPosition(targetIndex);
    }

    public void OnEndTurn()
    {
        _state = BattleState.DeckSetting;
        SetUI();
    }
}
