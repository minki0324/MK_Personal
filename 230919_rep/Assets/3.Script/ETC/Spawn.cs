using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Monster_Spwan mon_sp;
    void Start()
    {
        mon_sp = transform.GetComponent<Monster_Spwan>();
    }
    private void Awake()
    {
        mon_sp.StartSpawn();
    }


}
