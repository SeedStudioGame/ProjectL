using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMapController : MonoBehaviour
{
    [SerializeField]
    private GameObject _point, _line;

    [SerializeField]
    private int _horizontal, _vertical;

    private float _lineDiff = 50, _pointDiff = 100;
    private Vector3 _pointStartPos = new Vector3(-900, 500);
    private Vector3 _horizontalLineStartPos = new Vector3(-850, 500);
    private Vector3 _verticalLineStartPos = new Vector3(-900, 500);

    private void Start()
    {
        for(int i = -8; i < 9; i++)
        {
            for(int j = 5; j > -6; j--)
            {

                GameObject point = Instantiate(_point, transform);
                point.GetComponent<RectTransform>().anchoredPosition = new Vector3(i*100, j*100 ,0);
            }
        }
    }
}
