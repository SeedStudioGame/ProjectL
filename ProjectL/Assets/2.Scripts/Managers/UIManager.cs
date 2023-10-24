using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 UI를 저장하고 불러오기 편하게 하기 위한 용도. 각 씬의 컨트롤러에서 UI 추가

public class UIManager : ManagerBase
{
    private Dictionary<Type, List<Define.UIData>> _uis = new Dictionary<Type, List<Define.UIData>>();

    public void Clear() { _uis.Clear(); }

    public void AddUI<T>(string uiName, GameObject obj) where T : UnityEngine.Object
    {
        if (!SetUIListAbleToAdd<T>(uiName))
        {
            Debug.LogError("Can't Add UI");
            return;
        }
        _uis[typeof(T)].Add(FindUIData<T>(uiName, null, obj));
    }

    public void AddUI<T>(string uiName, string obj) where T : UnityEngine.Object
    {
        if (!SetUIListAbleToAdd<T>(uiName))
        {
            Debug.LogError("Can't Add UI");
            return;
        }
        _uis[typeof(T)].Add(FindUIData<T>(uiName, obj));
    }

    public void AddUI<T>(string uiName) where T : UnityEngine.Object
    {
        if (!SetUIListAbleToAdd<T>(uiName))
        {
            Debug.LogError("Can't Add UI");
            return;
        }
        _uis[typeof(T)].Add(FindUIData<T>(uiName));
    }

    private bool SetUIListAbleToAdd<T>(string uiName) where T : UnityEngine.Object
    {
        if (!HasKey<T>())
        {
            AddKey<T>();
        }
        
        if (IsContainUI<T>(uiName))
        {
            Debug.LogError("It's already in _ui array");
            return false;
        }

        return true;
    }

    private bool HasKey<T>() where T : UnityEngine.Object
    {
        bool isContain = _uis.ContainsKey(typeof(T));

        return isContain;
    }

    private void AddKey<T>() where T : UnityEngine.Object
    {
        _uis.Add(typeof(T), new List<Define.UIData>());
    }

    public bool IsContainUI<T>(string uiName) where T : UnityEngine.Object
    {
        bool isContain = false;

        for (int i = 0; i < _uis[typeof(T)].Count; i++)
        {
            if (_uis[typeof(T)][i].name.Equals(uiName))
            {
                isContain = true;
                break;
            }
        }

        return isContain;
    }

    private Define.UIData FindUIData<T>(string uiName, string objName = null, GameObject obj = null) where T : UnityEngine.Object
    {
        Define.UIData uiData;

        if (obj != null)
        { uiData.gameObject = obj; }
        else if (objName != null)
        { uiData.gameObject = Util.Find(objName); }
        else
        { uiData.gameObject = Util.GetRoot(); }

        if(uiData.gameObject == null)
        {
            Debug.LogError("Can't Find Object");
        }
        uiData.name = uiName;
        uiData.component = uiData.gameObject.GetComponent<T>();
        return uiData;
    }

    public Define.UIData GetUIData<T>(string uiName) where T : UnityEngine.Object
    {
        foreach(Define.UIData uiData in _uis[typeof(T)])
        {
            if (uiData.name.Equals(uiName))
            {
                return uiData;
            }
        }

        Debug.LogError("Can't Find UI Data");
        return new Define.UIData();
    }

    public T GetUI<T>(string uiName) where T : UnityEngine.Object
    {
        return GetUIData<T>(uiName).component as T;
    }

    public GameObject GetUIObject<T>(string uiName) where T : UnityEngine.Object
    {
        return GetUIData<T>(uiName).gameObject;
    }
}