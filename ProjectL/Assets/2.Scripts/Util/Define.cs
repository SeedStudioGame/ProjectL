using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��ü���� Ȱ��� �� �ִ� enum, struct, const�� �����մϴ�.
public class Define
{
    public enum Attribute
    {
        fire,
        wood,
        water,
        metal,
        earth,
        yang,
        yin
    }

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
