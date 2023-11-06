using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ply_Movement_Fix : MonoBehaviour
{
    /*
        1. �̵� ����
        2. ���� ����
        3. �ִϸ��̼� ���� ����

        ����

        ī�޶�
        �ִϸ��̼�
        ������ٵ�
        �ӵ�
        ������
        ���� ���� üũ
        ���� �������� üũ
    */
    Camera camera_;
    [SerializeField] private Animator ani;
    [SerializeField] private Rigidbody rb;

    [Header("�̵�")]
    [SerializeField] private float MoveSpeed = 5f;

    [Header("����")]
    [SerializeField] private float JumpForce = 10f;
    [SerializeField] private bool isGrounded = true;

    public bool isPlayerMove { get; private set; }

    public Vector3 CurrentPos { get; private set; }

    //�߰��� ����-�̼���

    public bool isAttacking_1 = false;  //���ݸ�� 1�� �������ΰ� �Ǵ�
    public bool isPossible_Attack_2 = false;    //���ݸ�� 2�� ���డ���� �����ΰ� (��� 1�� �߰��̻� ����Ǿ��°�) �Ǵ�

    public bool isAttacking_2 = false;   //���ݸ�� 2�� �������ΰ� �Ǵ�
    public bool isPossible_Attack_1 = true;     //���ݸ�� 1�� ���డ���� �����ΰ� (��� 1�� �߰��̻� ����Ǿ��°�) �Ǵ�

    public float groundCheckRadius = 0.2f;  // OverlapSphere ������
    public string groundTag = "Ground";  // ���� �±�

    private Vector3 playerPosition;

    private float Min = -210f;
    private float Max = 210f;

    private void Start()
    {
        camera_ = Camera.main;
        isPossible_Attack_1 = true;
        playerPosition = gameObject.transform.position;
    }

    private void Update()
    {

   
        CurrentPos = transform.position;
        InputMovment();
        Jump();
        Ground_Check();

        if (Input.GetKeyDown(KeyCode.H))
        {

            if (isPossible_Attack_1)
            {
                //��� 1 ���� ����

                ani.SetTrigger("Attack");
                //ani.SetBool("Attack1", true);

                isAttacking_1 = true;   //������

                isAttacking_2 = false;
                isPossible_Attack_1 = false;
                isPossible_Attack_2 = false;

            }

            if (isPossible_Attack_2)
            {

                //ani.SetBool("ContinualAttack", true);
                ani.SetTrigger("Continual_Attack");

                isAttacking_2 = true;
                isPossible_Attack_2 = false;

                isAttacking_1 = false;
                isPossible_Attack_1 = false;

            }
            // isLive = false;
        }


        if (Input.GetMouseButton(1))
        {
            ani.SetBool("Shield", true);
            ani.SetFloat("MoveSpeed", 0.5f);
            MoveSpeed = 2f;
        }
        else
        {
            ani.SetBool("Shield", false);
            ani.SetFloat("MoveSpeed", 1f);
            MoveSpeed = 5f;
        }
    

    }





    private void InputMovment()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);

        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 playerRotate = Vector3.Scale(camera_.transform.forward, new Vector3(1, 0, 1));

        Vector3 moveDirection = playerRotate * Input.GetAxis("Vertical") + camera_.transform.right * Input.GetAxis("Horizontal");


        if (moveDirection != Vector3.zero)
        {
            // ȸ��
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // �̵�
            transform.position += (moveDirection.normalized * MoveSpeed * Time.deltaTime);
            transform.position = new Vector3(
Mathf.Clamp(transform.position.x, Min, Max),
transform.position.y,
Mathf.Clamp(transform.position.z, Min, Max));
            ani.SetBool("Move", true);
            ani.SetBool("Idle", false);
            isPlayerMove = true;
        }
        else
        {
            ani.SetBool("Idle", true);
            ani.SetBool("Move", false);
            isPlayerMove = false;
        }
     

       
    }

    private void Jump()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce);
            // Ply_rb.AddForce(Vector3.up * JumpForce);
        }
    }

    public void Check_Ground()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        RaycastHit hit;
        Vector3 HitPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        Debug.DrawRay(HitPos, Vector3.down, Color.red, 1.1f);
        if (Physics.Raycast(HitPos, Vector3.down, out hit, 1.1f))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }
    private void Ground_Check()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, groundCheckRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(groundTag))
            {
                // �� �±׸� ���� ������Ʈ�� �浹�� ���
                isGrounded = true;
                break;
            }
            else
            {
                isGrounded = false;
            }
        }

        if (isGrounded)
        {
            // ĳ���ʹ� ���� ��� ����
            // ���⿡�� ������ ����ϰų� �ٸ� ������ ������ �� ����
        }
    }
}