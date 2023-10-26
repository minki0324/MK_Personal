using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Transform sun;
    public float Rspeed;
    public float Wspeed;
    void Update()
    {
        transform.Rotate(Vector3.down, Wspeed * Time.deltaTime);
        transform.RotateAround(sun.position, sun.transform.up, Rspeed * Time.deltaTime);
    }
}
