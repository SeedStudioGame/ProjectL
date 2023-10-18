using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//공통으로 사용하는 자료 (Enum, Struct, Const)를 쉽게 접근하기 위한 클래스

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
