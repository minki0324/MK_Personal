using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu] // 에셋으로 만들기 위한 방법.

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
