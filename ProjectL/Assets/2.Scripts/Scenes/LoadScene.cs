using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : SceneBase
{
    private float _speed;
    private Slider loadBar;


    private void Start()
    {
        Init();
        InitUI();
    }

    protected override void Init()
    {
        _type = Define.Scene.Load;
        _name = "Load";
        _speed = 100f / Managers.Scene.GetLoadTime();
    }

    protected override void InitUI()
    {
        Managers.UI.Clear();

        Util.SetRoot("Canvas");
        Managers.UI.AddUI<Slider>("LoadBar", "LoadBar");
        loadBar = Managers.UI.GetUI<Slider>("LoadBar");

        StartCoroutine(Loading());
    }

    private IEnumerator Loading()
    {
        while(loadBar.value < 100)
        {
            loadBar.value += _speed * Time.deltaTime;
            yield return null;
        }
        
        loadBar.value = 100;
        OnLoad();

        yield return null;
    }

    protected override void OnLoad()
    {
        Managers.Scene.OnLoad();
        Managers.Scene.LoadScene();
    }
}
