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
    private float wallHitCooldown = 0f; // 회전 방향 변경 후 다음 변경까지의 대기 시간
    private float wallHitDelay = 1f;  // 실제 대기 시간 값 (예: 0.5초)

    private void Start()
    {
        TimeAfterSpawn = 0;
        target = FindObjectOfType<PlayerControl>().transform; // 스크립트를 추적
    }

    private void Update()
    {
        TimeAfterSpawn += Time.deltaTime;
        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
        if (wallHitCooldown > 0)
        {
            wallHitCooldown -= Time.deltaTime;  // 대기 시간 감소
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
                    if (wallHitCooldown <= 0) // 대기 시간이 끝난 경우에만 회전 방향 변경
                    {
                        RotateSpeed *= -1;
                        wallHitCooldown = wallHitDelay;  // 대기 시간 설정
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
