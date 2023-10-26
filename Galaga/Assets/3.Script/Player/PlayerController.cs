using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Player HP 관련 변수들

    private float Max_HP = 3f;
    private float currentHP;

    [SerializeField] private Player_Score score;
    /*
        프로퍼티 중에 Get 메소드를 구현하지 않고도 => 이걸로 구현할 수 있다. 
    */
    public float MAXHP => Max_HP;
    public float currenthp => currentHP;

    public SpriteRenderer renderer;

    // ------------------------------

    private Movement2D movement2D;

    [SerializeField]private Stage_Data stagedata;
    [SerializeField] private Weapon weapon;

    private void Awake()
    {
        movement2D = transform.GetComponent<Movement2D>();
        weapon = transform.GetComponent<Weapon>();
        score = GetComponent<Player_Score>();

        currentHP = Max_HP;
        TryGetComponent(out renderer);
    }

    private void Start()
    {
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            weapon.StartFire();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            weapon.StopFire();
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

    // 데미지하고 Hit액션 만들기

    public void TakeDam(float damage)
    {
        currentHP -= damage;
        Debug.Log("Player HP : " + currentHP);
        // HitColor_Action_co

        StopCoroutine("HitColor_Action_co");
        StartCoroutine("HitColor_Action_co");

        if (currentHP <= 0)
        {
            // 죽는 메소드
            onDie();
        }
    }

    private void onDie()
    {
        score.Savescore();
        Destroy(gameObject);
        SceneManager.LoadScene("Gameover");
    }

    private IEnumerator HitColor_Action_co()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.color = Color.white;
    }
   
}
