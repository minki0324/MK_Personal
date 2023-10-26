using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_Obj : MonoBehaviour
{
    [SerializeField] private float Scroll_Speed = 2f;

    void Update()
    {
        transform.Translate(Vector3.down * Scroll_Speed * Time.deltaTime);
    }
}
