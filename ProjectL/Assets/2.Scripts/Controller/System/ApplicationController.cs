using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ ��Ʈ�� ����Ʈ�̸� ���ӿ� ��ü������ �۵��� �� ���� �����ϰ� ������ �����Ѵ�.

[RequireComponent(typeof(Managers))] // �ʼ� ������Ʈ�� ����
public class ApplicationController : MonoBehaviour
{
    private void Awake()
    {
        Managers.Root = Util.GetOrAddComponent<Managers>(gameObject);
        Managers.Root.Init();
        DontDestroyOnLoad(gameObject);
    }
}
