using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Player HP ���� ������

    private float Max_HP = 3f;
    private float currentHP;

    [SerializeField] private Player_Score score;
    /*
        ������Ƽ �߿� Get �޼ҵ带 �������� �ʰ� => �̰ɷ� ������ �� �ִ�. 
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
        // �÷��̾ ȭ�� ���� �ٱ����� ������ ���ϵ��� ����
        transform.position = new Vector3
        (
            Mathf.Clamp(transform.position.x, stagedata.Limit_Min.x, stagedata.Limit_Max.x),
            Mathf.Clamp(transform.position.y, stagedata.Limit_Min.y, stagedata.Limit_Max.y),
            0
        ); 
    }

    // �������ϰ� Hit�׼� �����

    public void TakeDam(float damage)
    {
        currentHP -= damage;
        Debug.Log("Player HP : " + currentHP);
        // HitColor_Action_co

        StopCoroutine("HitColor_Action_co");
        StartCoroutine("HitColor_Action_co");

        if (currentHP <= 0)
        {
            // �״� �޼ҵ�
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
