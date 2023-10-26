using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private MonWeapon weapon;

    private void Awake()
    {
        weapon = transform.GetComponent<MonWeapon>();
    }

    void Start()
    {
        weapon.StartFire();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("PlyBullet"))
        {
            Destroy(gameObject);
        }
    }

   
}
