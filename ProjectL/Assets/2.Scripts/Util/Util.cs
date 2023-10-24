using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Find, GetOrAddComponent 등 유용한 기능을 구현합니다.
public static class Util
{
    public static T GetOrAddComponent<T>(GameObject target) where T : Component
    {
        T component = target.GetComponent<T>();
        if(component == null)
        {
            component = target.AddComponent<T>();
        }
        return component;
    }

    private static Transform _root = null;

    public static GameObject Find(string target)
    {
        Transform go = null;

        go = GameObject.Find(target).transform;

        if (go == null)
            go = _root.Find(target);

        if (go == null)
        {
            for (int i = 0; i < _root.childCount; i++)
            {
                go = _root.GetChild(i).Find(target);
            }
        }

        if (go == null)
        {
            Debug.LogError("No Object");
            return null;
        }

        return go.gameObject;
    }

    public static void SetRoot(Transform root) { _root = root; }

    public static void SetRoot(GameObject root) { _root = root.transform; }

    public static void SetRoot(string root, bool recursive = false)
    {
        _root = null;
        if (!recursive)
        {
            _root = GameObject.Find(root).transform;
        }
        else
        {
            _root = Find(root).transform;
        }

        if(_root == null)
        {
            Debug.LogError("Can't Find Object");
        }
    }

    public static GameObject GetRoot()
    {
        return _root.gameObject;
    }
}
