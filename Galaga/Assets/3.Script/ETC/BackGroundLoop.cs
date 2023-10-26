using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundLoop : MonoBehaviour
{
    [SerializeField] private float ScrollRange = 9.9f;
    void Update()
    {
        if(transform.position.y <= -ScrollRange)
        {
            BackGroundOffset();
        }
    }

    public void BackGroundOffset()
    {
        Vector2 offset = new Vector2(0, ScrollRange * 2f);
        transform.position = (Vector2)transform.position + offset;
    }
}
