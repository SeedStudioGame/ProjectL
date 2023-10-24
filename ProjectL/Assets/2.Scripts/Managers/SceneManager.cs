using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;

//씬 전환 기능을 담당하고 씬 정보를 갖고 있음

public class SceneManager : ManagerBase
{
    private SceneBase _scene;
    private Define.Scene _nextScene;
    private float _loadingTime;
    private bool _onLoading;

    public override void Init()
    {
        _onLoading = false;
        _loadingTime = 3;
    }

    public void LoadScene(Define.Scene scene = Define.Scene.Load)
    {
        if (_onLoading)
        {
            _onLoading = false;
            SM.LoadScene(System.Enum.GetName(typeof(Define.Scene), _nextScene));
        }
        else
        {
            _nextScene = scene;
            SM.LoadScene("Load");
        }
    }

    public Define.Scene GetNextScene()
    {
        return _nextScene;
    }

    public float GetLoadTime()
    {
        return _loadingTime;
    }

    public void OnLoad()
    {
        _onLoading = true;
    }

    public void OnLoad(SceneBase scene)
    {
        _scene = scene;
        _onLoading = true;
    }
}