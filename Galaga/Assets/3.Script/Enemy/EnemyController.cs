using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Stage_Data stage_Data;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawn enemy;
    private Weapon weapon;


    //  HP 관련 변수들

    [SerializeField] private float MaxHP = 2f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

    [SerializeField] private SpriteRenderer renderer;

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    private bool isDead;

    /*
    private void Awake()
    {
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // 근데 이렇게하면 가비지값이 생기기 때문에 다른 방법을 사용

        // enemy = FindObjectOfType<EnemySpawn>();

        // GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        // GameObject.FindGameObjectWithTag("Enemy_spawner").TryGetComponent(out enemy);
        // 트라이겟컴포넌트 명령어가 일반 겟 컴포넌트보다 속도가 월등하게 빠르고 가비지값도 생기지 않는다.
     
        // 예외처리를 걸면서 동적 할당을 할 때 주로 사용한다.
        // if(!GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player));
        // {
        //     GameObject.FindGameObjectWithTag("Player").AddComponent<PlayerController>();
        //     GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        // }
    }
    */

    private void OnEnable()
    {
        weapon = GetComponent<Weapon>();
        currentHP = MaxHP; // hp초기화
        renderer = GetComponent<SpriteRenderer>();

        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        GameObject.FindGameObjectWithTag("Enemy_spawner").TryGetComponent(out enemy);

        renderer.color = Color.white;
        isDead = false;
        weapon.StartFire();
    }

    private void Update()
    {
        if(transform.position.y < stage_Data.Limit_Min.y - 2f)
        {
            onDie();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // 죽는 메소드 넣어주세요
            onDie();
            player.TakeDam(1);
        }
    }

    public void onDie()
    {
        // Enemy가 사망시 호출될 메소드
        // enemy.ReturnEnemyToPool(gameObject);
        enemy.Takein_enemy(gameObject);
        isDead = true;
    }

    public void Take_dam(float damage)
    {
        currentHP -= damage;

        if(!isDead)
        {
            // 효과를 넣어주세요
            StopCoroutine("Hitanimation_co");
            StartCoroutine("Hitanimation_co");
        }

        if(currentHP<=0)
        {
            onDie();
        }
    }

    private IEnumerator Hitanimation_co()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
    }
}
