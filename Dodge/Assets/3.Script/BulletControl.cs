using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float Speed = 8f;
    private Rigidbody bullet_r;

    private void Start()
    {
        if(TryGetComponent(out bullet_r))
        {
            bullet_r.velocity = transform.forward * Speed;
        }
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            if(col.TryGetComponent(out PlayerControl controller))
            {
                controller.Die();
            }
        }
    }
}
