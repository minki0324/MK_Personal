using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroybyTime : MonoBehaviour
{
    [SerializeField] private float destroyTime;

    private void Awake()
    {
        //destroytime �ð� �ڿ� ������.
        Destroy(gameObject, destroyTime);    
    }
}
