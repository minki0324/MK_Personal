using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moon : MonoBehaviour
{
    public Transform earth;

    void Update()
    {
        transform.RotateAround(earth.position, Vector3.up, 600f * Time.deltaTime);

    }
}
