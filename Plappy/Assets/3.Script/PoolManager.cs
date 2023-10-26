using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] Wall_Prefabs;

    // 풀 담당을 하는 리스트들
    private List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[Wall_Prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀에 놀고 있는 게임 오브젝트를 선택
        // 선택이 되었다면 select 변수에 할당

        foreach (GameObject Clone in pools[index])
        {
            // 만약 풀링할 클론이 비활성화 되어있다면
            if (!Clone.activeSelf)
            {
                // 비활성화 된 클론을 선택하고 활성화 한다음에 내보냄
                select = Clone;
                select.SetActive(true);
                break;
            }
        }

        // 전부 쓰고 있다면 새롭게 생성해서 select 변수에 할당
        if (select == null)
        {
            select = Instantiate(Wall_Prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
