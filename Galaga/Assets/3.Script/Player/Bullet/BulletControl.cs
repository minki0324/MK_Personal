using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    /*
        bullet 삭제 
        화면 바깥으로 일정 거리를 나가게 되면 Destroy되도록 만들어야됨.
        1. 화면사이즈
        2. 일정 거리

        위로 나가는 경우
        좌로 나가는 경우
        아래로 나가는 경우 (적)
        우로 나가는 경우
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
