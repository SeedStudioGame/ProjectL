using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : SceneBase
{
    [SerializeField]
    private GameObject[] _enemys = new GameObject[0];

    [SerializeField]
    private GameObject _enemyRoot;
    private HorizontalLayoutGroup _enemyLayout;

    private int _enemyCount;

    private void Start()
    {
        Init();
        InitUI();
    }

    protected override void Init()
    {
        _name = "battle";
    }


    protected override void InitUI()
    {
        Managers.UI.Clear();
        _enemyLayout = _enemyRoot.GetComponent<HorizontalLayoutGroup>();

        Util.SetRoot("Canvas");
    }
}
