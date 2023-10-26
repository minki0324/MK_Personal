using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    private float height;

    void Start()
    {
        // 넓이는 BackGround의 박스콜라이더 y축
        height = transform.GetComponent<BoxCollider2D>().size.y*0.77f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -height)
        {
            Vector2 offset = new Vector2(0, height * 2f);
            transform.position = (Vector2)transform.position + offset;
        }
    }
}
