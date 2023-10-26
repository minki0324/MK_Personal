using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Stage_Data stage_Data;
    [SerializeField] private GameObject Enemy_Prefabs;
    [SerializeField] private GameObject Enemy_Prefabs_2;
    [SerializeField] private float SpawnTime;
    [SerializeField] private int poolsize = 50;
    private Vector3 poolposition = new Vector3(0, -25f, 0);

    private Queue<GameObject> enemypool = new Queue<GameObject>();
    [SerializeField] private GameObject Enemy_HpBar;
    [SerializeField] private Transform Canvas;
    

    private void Awake()
    {
        enemypool = new Queue<GameObject>();
        for(int i = 0; i<poolsize; i++)
        {
            if(i%4 != 0)
            {
                GameObject enemy = Instantiate(Enemy_Prefabs, poolposition, Quaternion.identity);
                enemy.SetActive(false);
                enemypool.Enqueue(enemy);
            }
            else if(i%4 == 0)
            {
                GameObject enemy = Instantiate(Enemy_Prefabs_2, poolposition, Quaternion.identity);
                enemy.SetActive(false);
                enemypool.Enqueue(enemy);
            }
        }

        // for(int i = 0; i < poolsize; i++)
        // {
        //     GameObject Enemy = Instantiate(Enemy_Prefabs, Vector3.zero, Quaternion.identity);
        //     Enemy.SetActive(false);
        //     enemypool.Enqueue(Enemy);
        // }

        StartCoroutine("Spawn_Enemy_co");
    }

    public void Takeout_enemy(Vector3 position)
    {
        if(enemypool.Count > 0)
        {
            GameObject Enemy = enemypool.Dequeue();
            if(!Enemy.activeSelf)
            {
                Enemy.SetActive(true);
            }
            Enemy.transform.position = position;
            SpawnEnemy_HP(Enemy.GetComponent<EnemyController>());
        }
    }

    public void Takein_enemy(GameObject Enemy)
    {
        Enemy.transform.position = Vector3.zero;
        if(Enemy.activeSelf)
        {
            Enemy.SetActive(false);
        }
        enemypool.Enqueue(Enemy);
    }

    private IEnumerator Spawn_Enemy_co()
    {
        WaitForSeconds wfs = new WaitForSeconds(SpawnTime);
        // while���� ���������� �ݺ��� �Ǵµ�  ��� waitForSeconds�� ���� �Ǹ� ������ �� ������ �÷����� ���� ����� ��
        // �׷��� waitForSeconds ĳ���̶�� ���� ����Ͽ��� ������ �÷��Ͱ� �������� �ʵ��� ����
        // ĳ���̶� ��ǻ�Ϳ��� �����ð� �ɸ��� �۾��� ����� �����ؼ� �ð��� ����� �����ϴ� ���


        while (true)
        {
            float positionX = Random.Range(stage_Data.Limit_Min.x, stage_Data.Limit_Max.x);
            Vector3 position = new Vector3(positionX, stage_Data.Limit_Max.y + 1f, 0);
            Takeout_enemy(position);

            // ���� �Ѱ�
            // GameObject Enemy = enemypool.Dequeue();
            // Enemy.SetActive(true);
            // Enemy.transform.position = position;
            // ActiveEnemy.Add(Enemy);
        
            yield return wfs;
        }
    }

    private void SpawnEnemy_HP(EnemyController enemy)
    {
        GameObject sliderClone = Instantiate(Enemy_HpBar);
        // �θ� ������ ���� �޼ҵ� SetParent()
        sliderClone.transform.SetParent(Canvas);
        sliderClone.transform.localScale = Vector3.one;

        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy);
        sliderClone.GetComponent<EnemyHpPositionSetter>().Setup(enemy.gameObject);
    }

    // public void ReturnEnemyToPool(GameObject enemy)
    // {
    //     enemy.SetActive(false);
    //     // ActiveEnemy.Remove(enemy);
    //     enemypool.Enqueue(enemy);
    // }
}
