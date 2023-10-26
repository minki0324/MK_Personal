using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefabs;
    private float Bullet_Cooltime = 0.5f;
   
    public Transform target; // player
    private float TimeAfterSpawn;

    private float RotateSpeed = 100f;
    private float wallHitCooldown = 0f; // ȸ�� ���� ���� �� ���� ��������� ��� �ð�
    private float wallHitDelay = 1f;  // ���� ��� �ð� �� (��: 0.5��)

    private void Start()
    {
        TimeAfterSpawn = 0;
        target = FindObjectOfType<PlayerControl>().transform; // ��ũ��Ʈ�� ����
    }

    private void Update()
    {
        TimeAfterSpawn += Time.deltaTime;
        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
        if (wallHitCooldown > 0)
        {
            wallHitCooldown -= Time.deltaTime;  // ��� �ð� ����
        }
        
        bool isHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 10f);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);

        if(isHit)
        {
            switch (hitInfo.collider.tag)
            {
                case "Player":
                    if(Bullet_Cooltime <= TimeAfterSpawn)
                    {
                        TimeAfterSpawn = 0;
                        GameObject bullet = Instantiate(bulletPrefabs, transform.position, transform.rotation);
                    }
                    break;
                case "Wall":
                    if (wallHitCooldown <= 0) // ��� �ð��� ���� ��쿡�� ȸ�� ���� ����
                    {
                        RotateSpeed *= -1;
                        wallHitCooldown = wallHitDelay;  // ��� �ð� ����
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
