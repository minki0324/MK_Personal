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
