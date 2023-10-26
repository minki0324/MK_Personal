using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // ��������� ������ ����
    public GameObject[] Wall_Prefabs;

    // Ǯ ����� �ϴ� ����Ʈ��
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

        // ������ Ǯ�� ��� �ִ� ���� ������Ʈ�� ����
        // ������ �Ǿ��ٸ� select ������ �Ҵ�

        foreach (GameObject Clone in pools[index])
        {
            // ���� Ǯ���� Ŭ���� ��Ȱ��ȭ �Ǿ��ִٸ�
            if (!Clone.activeSelf)
            {
                // ��Ȱ��ȭ �� Ŭ���� �����ϰ� Ȱ��ȭ �Ѵ����� ������
                select = Clone;
                select.SetActive(true);
                break;
            }
        }

        // ���� ���� �ִٸ� ���Ӱ� �����ؼ� select ������ �Ҵ�
        if (select == null)
        {
            select = Instantiate(Wall_Prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
