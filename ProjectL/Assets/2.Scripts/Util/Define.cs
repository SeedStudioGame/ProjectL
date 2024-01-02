using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public struct UIData
    {
        public string name;
        public GameObject gameObject;
        public UnityEngine.Object component;
    }

    public enum Scene
    {
        Awake, Load, Title, Lobby
    }

    public enum BGM
    {

    }

    public enum SFX
    {

    }
}
