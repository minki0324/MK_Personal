using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu] // �������� ����� ���� ���.

public class Stage_Data : ScriptableObject
{
    [SerializeField] private Vector2 _LimitMin;
    [SerializeField] private Vector2 _LimitMax;

    public Vector2 Limit_Min
    {
        get
        {
            return _LimitMin;
        }
    }
    public Vector2 Limit_Max
    {
        get
        {
            return _LimitMax;
        }
    }
}
