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

    [SerializeField] private GameObject P_Bullet;
    [SerializeField] private GameObject E_Bullet;
    
    [SerializeField] private Stage_Data stage_data;
    private float destroyheight = 2.0f;
    [SerializeField] private Player_Score Playerscore;

    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Playerscore);
    }
    //
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameObject.CompareTag("Player_Bullet") && collision.CompareTag("Enemy"))
        {
            Playerscore.set_Score(3);
            collision.GetComponent<EnemyController>().Take_dam(1);
            Destroy(gameObject);
        }
    
        if(gameObject.CompareTag("Mon_Bullet") && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDam(1);
            Destroy(gameObject);
        }
    }

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
