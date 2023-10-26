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
        // while문은 지속적으로 반복이 되는데  계속 waitForSeconds를 쓰게 되면 모조리 다 가비지 컬렉터의 수집 대상이 됨
        // 그래서 waitForSeconds 캐싱이라는 것을 사용하여서 가비지 컬렉터가 수집하지 않도록 만듬
        // 캐싱이란 컴퓨터에서 오랜시간 걸리는 작업의 결과를 저장해서 시간과 비용을 절감하는 기법


        while (true)
        {
            float positionX = Random.Range(stage_Data.Limit_Min.x, stage_Data.Limit_Max.x);
            Vector3 position = new Vector3(positionX, stage_Data.Limit_Max.y + 1f, 0);
            Takeout_enemy(position);

            // 내가 한거
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
        // 부모 설정을 위한 메소드 SetParent()
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
