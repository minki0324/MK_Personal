using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PoolManager pool;
    [SerializeField] private float Spawn_Time = 2f;
    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= Spawn_Time)
        {
            Spawn();
            time = 0;
        }
    }

    private void Spawn()
    {
        GameObject Wall = pool.Get(0);

        // 랜덤을 1부터 시작하는 이유는 스크립트를 들고 있는 spawner 자기 자신을 빼고 소환하기 때문
        Wall.transform.position = transform.position;

    }
}
