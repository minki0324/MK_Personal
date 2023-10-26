using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    /*
        bullet ���� 
        ȭ�� �ٱ����� ���� �Ÿ��� ������ �Ǹ� Destroy�ǵ��� �����ߵ�.
        1. ȭ�������
        2. ���� �Ÿ�

        ���� ������ ���
        �·� ������ ���
        �Ʒ��� ������ ��� (��)
        ��� ������ ���
    */

    [SerializeField] private Stage_Data stage_data;
    private float destroyheight = 2.0f;

    private void LateUpdate()
    {
        if(transform.position.y < stage_data.Limit_Min.y - destroyheight ||
            transform.position.y > stage_data.Limit_Max.y + destroyheight ||
            transform.position.x < stage_data.Limit_Min.x - destroyheight ||
            transform.position.x > stage_data.Limit_Max.x + destroyheight)
        {
            Destroy(gameObject);
        }
    }
   
}
