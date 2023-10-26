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
        // ���� ����Ʈ ����
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
        // ���ӿ��� ����ó��
        if(GameManager.Instance != null && GameManager.Instance.isGameOver == true)
        {
            return;
        }

        if(zombie_List.Count <= 0)
        {
            // ���̺� �ø��� �޼ҵ�
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
        // ���̺� ����, ���� ���� �� ��� �����ϴ��� ����
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
            zombie data �����ϰ� ������
            zombie spawnpoint �����ϰ� ������
            ���� �׾��� �� �̺�Ʈ �߰�
            1. List���� ����
            2. ���� ������Ʈ ����
            3. ���ھ� ���
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
