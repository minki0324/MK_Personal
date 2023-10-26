using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public ZombieData[] zombieDatas;
    public ZombieControl zombie;

    [SerializeField] private Transform[] spawnPoint;
    private List<ZombieControl> zombie_List = new List<ZombieControl>();

    private int Wave;

    private void Awake()
    {
        // 스폰 포인트 설정
        Setup_SpawnPoint();
    }

    private void Setup_SpawnPoint()
    {
        spawnPoint = new Transform[transform.childCount];

        for (int i = 0; i < spawnPoint.Length; i++)
        {
            spawnPoint[i] = transform.GetChild(i).transform;
        }
    }

    private void Update()
    {
        // 게임오버 예외처리
        if(GameManager.Instance != null && GameManager.Instance.isGameOver == true)
        {
            return;
        }

        if(zombie_List.Count <= 0)
        {
            // 웨이브 늘리는 메소드
            Spawn_Wave();
        }

        Update_UI();
    }

    private void Update_UI()
    {
        HUD.Instance.Update_Wave(Wave, zombie_List.Count);
    }

    private void Spawn_Wave()
    {
        // 웨이브 증가, 좀비 생성 및 몇마리 스폰하는지 결정
        Wave++;
        int count = Mathf.RoundToInt(Wave * 2);

        for(int i = 0; i < count; i++)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        /*
            zombie data 랜덤하게 정해줌
            zombie spawnpoint 랜덤하게 정해줌
            좀비가 죽었을 때 이벤트 추가
            1. List에서 삭제
            2. 좀비 오브젝트 삭제
            3. 스코어 계산
        */

        ZombieData data = zombieDatas[Random.Range(0, zombieDatas.Length)];
        Transform point = spawnPoint[Random.Range(0, spawnPoint.Length)];

        ZombieControl zombie = Instantiate(this.zombie, point.position, point.rotation);

        zombie.Setup(data);
        zombie_List.Add(zombie);

        zombie.onDead += () => { zombie_List.Remove(zombie); };
        zombie.onDead += () => { Destroy(zombie.gameObject, 10f); };
        zombie.onDead += () => { GameManager.Instance.AddScore(10); };
    }
}
