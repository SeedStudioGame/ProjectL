using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�������� ����ϴ� �ڷ� (Enum, Struct, Const)�� ���� �����ϱ� ���� Ŭ����

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
}
