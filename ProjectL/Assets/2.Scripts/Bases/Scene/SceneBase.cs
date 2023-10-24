using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase : MonoBehaviour
{
    protected Define.Scene _type;
    protected string _name;

    protected virtual void OnLoad() { }

    protected virtual void Init() { }

    protected virtual void InitUI() { }
}
