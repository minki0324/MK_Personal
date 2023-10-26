using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroybyTime : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private void Awake()
    {
        //destroytime 시간 뒤에 삭제됨.
        Destroy(gameObject, destroyTime);    
    }
}
