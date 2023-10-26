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

        // ������ 1���� �����ϴ� ������ ��ũ��Ʈ�� ��� �ִ� spawner �ڱ� �ڽ��� ���� ��ȯ�ϱ� ����
        Wall.transform.position = transform.position;

    }
}
