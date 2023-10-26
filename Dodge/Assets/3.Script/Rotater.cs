using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [Range(10f,100f)]
    [SerializeField] private float Rotate_Speed = 60f;

    private void Update()
    {
        transform.Rotate(0, Rotate_Speed * Time.deltaTime, 0);
    }
}
