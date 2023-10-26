using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private int LifeCount = 3;

    [SerializeField] private Stage_Data stagedata;
    [SerializeField] private Weapon weapon;

    public GameObject Life1UI;
    public GameObject Life2UI;
    public GameObject Life3UI;

    private Animator animator;

    private bool isDead = false;

    private void Awake()
    {
        movement2D = transform.GetComponent<Movement2D>();
        weapon = transform.GetComponent<Weapon>();
        animator = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.tag = "EnemyBullet";
        if(movement2D.Move_Speed <= 0f)
        {
            movement2D.Move_Speed = 5f;
        }
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        movement2D.MoveTo(new Vector3(x, y, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            weapon.StartFire();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            weapon.StopFire();
        }
        if (GameManager.Instance.isGameOver)
        {
            return;
        }
        
    }

    private void LateUpdate()
    {
        // 플레이어가 화면 범위 바깥으로 나가지 못하도록 설정
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, stagedata.Limit_Min.x, stagedata.Limit_Max.x),
            Mathf.Clamp(transform.position.y, stagedata.Limit_Min.y, stagedata.Limit_Max.y),
            0
        ); 
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        movement2D.Move_Speed = 0f;
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.CompareTag("Enemy") && LifeCount == 3)
        {
            LifeCount--;
            Life3UI.SetActive(false);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Enemy") && LifeCount == 2)
        {
            LifeCount--;
            Life2UI.SetActive(false);
            Destroy(col.gameObject);
        }
        else if (col.gameObject.CompareTag("Enemy") && LifeCount == 1)
        {
            LifeCount--;
            Life1UI.SetActive(false);
            Destroy(col.gameObject);
            Die();
        }
    }

    
}
