using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item_Type
{
    Coin = 10
}

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject ItemEffectPrefabs;

   public void Exit()
    {
        Instantiate(ItemEffectPrefabs, transform.position, Quaternion.identity);
        // �������� �Ծ��� �� ȣ���� �޼ҵ�
        Destroy(gameObject);
    }
}
