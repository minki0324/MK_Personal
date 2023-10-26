using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Stage_Data stage_Data;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemySpawn enemy;
    private Weapon weapon;


    //  HP ���� ������

    [SerializeField] private float MaxHP = 2f;
    private float currentHP;

    public float MAXHP => MaxHP;
    public float CurrentHP => currentHP;

    [SerializeField] private SpriteRenderer renderer;

    // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    private bool isDead;

    /*
    private void Awake()
    {
        // player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // �ٵ� �̷����ϸ� ���������� ����� ������ �ٸ� ����� ���

        // enemy = FindObjectOfType<EnemySpawn>();

        // GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
        // GameObject.FindGameObjectWithTag("Enemy_spawner").TryGetComponent(out enemy);
        // Ʈ���̰�������Ʈ ��ɾ �Ϲ� �� ������Ʈ���� �ӵ��� �����ϰ� ������ ���������� ������ �ʴ´�.
     
        // ����ó���� �ɸ鼭 ���� �Ҵ��� �� �� �ַ� ����Ѵ�.
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
        currentHP = MaxHP; // hp�ʱ�ȭ
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
            // �״� �޼ҵ� �־��ּ���
            onDie();
            player.TakeDam(1);
        }
    }

    public void onDie()
    {
        // Enemy�� ����� ȣ��� �޼ҵ�
        // enemy.ReturnEnemyToPool(gameObject);
        enemy.Takein_enemy(gameObject);
        isDead = true;
    }

    public void Take_dam(float damage)
    {
        currentHP -= damage;

        if(!isDead)
        {
            // ȿ���� �־��ּ���
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
