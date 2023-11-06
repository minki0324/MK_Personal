using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttack : Minion_Controller
{
    //[SerializeField] private Ply_Controller player;
    /*
     �̴Ͼ��� �߽������� ���̾ �����ϴ� �� ����
    ���̾��� ���尡������� Ÿ������ ������
    Ÿ�ٰ����� �̴Ͼ��� ���� �ٶ󺸰� (Lookat�޼ҵ�) ������ �̵�  -> ����� Lerp�� �̵� ���� �׺���̼����� ����
    �̵��� �̴Ͼ��� ���ݹ��� �ݶ��̴�(�̴Ͼ�տ� ���� �ڽ��ݶ��̴� (���񼭹��̹�ó��))�� ������ ������ ����

     
     
     
     */
    //�ӽ� �̴Ͼ�ü��
    private int HP = 3;
    private bool isDie;
    [SerializeField] private Ply_Controller player;


    // ���� ���ݰ�������
    public float scanRange = 13f;
    public float AttackRange = 1.5f;

    //�̵��� ���������� ���ݹ����ݶ��̴��� ��Ҵ°�?
    [SerializeField] private bool isdetecting = false;
    //�������ΰ�?
    private bool isAttacking = false;
    private bool isHitting = false;
    private bool isSuccessAtk = true;
    private Animator ani;
    private Coroutine attackCoroutine;
    // ���� ��� ���̾�
    [SerializeField] private LayerMask targetLayer;
    //�׾����� �ڽ��ݶ��̴� Enable�ϱ����� �������� 
    [SerializeField] private BoxCollider HitBox_col;
    [SerializeField] private BoxCollider Ob_Weapon_col;
    //Ÿ�ٷ��̾�
    private int targetLayer_Index;
    //����Ÿ��
    public Transform nearestTarget;
    //����, ��Ʈ ������
    private WaitForSeconds attackDelay;
    private WaitForSeconds hitDelay = new WaitForSeconds(0.2f);
    //�׺���̼�
    private NavMeshAgent navMeshAgent;
    private void Start()
    {
        //���ݵ����� ����
        TryGetComponent(out player);

        navMeshAgent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();



        //GameObject.FindGameObjectWithTag("Player").TryGetComponent<Ply_Controller>(out player);
        if (gameObject.layer == LayerMask.NameToLayer("Team"))
        {
            targetLayer_Index = LayerMask.NameToLayer("Enemy");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            targetLayer_Index = LayerMask.NameToLayer("Team");
        }
        targetLayer = 1 << targetLayer_Index;

    }


    private void Update()
    {

        //MinionAttack();




        if (HP <= 0)
        {
            //�������� ,�̵����� 
            if (!isDie)
            {
                Die();
            }
            isDie = true;
        }
    }
    //���̾� ������
    Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
    private void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(AttackDir);
    }
    void AttackMoving(Transform target)
    {
        //���� �̴Ͼ���Ʈ�ѷ����� �����ҿ���.(�ִϸ��̼�)
        // ���� ������ ����
        //Debug.Log("����Ÿ�� : " + target.name);
        ani.SetBool("Move", true);
        navMeshAgent.isStopped = false;
        //���� �׺���̼����� �̵����� 
        navMeshAgent.SetDestination(target.transform.position);
        //transform.position = Vector3.Lerp(transform.position, target.position, moveSpeed * Time.deltaTime);



    }
    //�������� �׸��¸޼ҵ�
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, scanRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, AttackRange);
    //}

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Weapon") && other.gameObject.layer == targetLayer_Index && !isHitting)
        {
            StartCoroutine(Hit_co());
        }

    }

    private IEnumerator Attack_co()
    {
        //������Ÿ��
        float d = Random.Range(2f, 2.1f);
        attackDelay = new WaitForSeconds(d);

        //���� ���������� ����
        isAttacking = true;

        isSuccessAtk = false;
        ani.SetTrigger("Attack");
        yield return attackDelay;


        isAttacking = false;
    }

    private IEnumerator Hit_co()
    {
        isHitting = true;
        //��Ʈ�� ������ޱ�
        HP -= 1;


        //���ݵ��� ĵ���� ������Ÿ�� �ʱ�ȭ
        if (!isSuccessAtk)
        {


            StopCoroutine(attackCoroutine);
            isAttacking = false;
        }

        ani.SetTrigger("Hit");
        yield return hitDelay;
        isHitting = false;


    }

    public void WeaponActive()
    {
        isSuccessAtk = true;
        Ob_Weapon_col.enabled = true;
        Invoke("WeaponFalse", 0.1f);

    }
    private void WeaponFalse()
    {
        Ob_Weapon_col.enabled = false;
    }
    public void Die()
    {
        ani.SetTrigger("Dead");  // �״¸�����
        gameObject.layer = 9;   // ���̾� DIe�� �����ؼ� Ÿ������ �ȵǰ�
        HitBox_col.enabled = false;    //�ε������ʰ� �ݶ��̴� false
        //StopCoroutine(attackCoroutine);   //���ݵ����̶�� ���ݵ� ����
        Destroy(gameObject, 3f);  // �װ��� 3���� ��Ʈ����
    }
    public void MinionAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, targetLayer);
        nearestTarget = GetNearestTarget(hits);


        if (nearestTarget != null && !isDie)
        {
            float attackDistance = Vector3.Distance(transform.position, nearestTarget.position);
            if (attackDistance <= AttackRange)
            {
                isdetecting = true;
            }
            else
            {
                isdetecting = false;
            }
            //Ÿ�ٰ����� Ÿ�������� �ٶ󺸱�
            LookatTarget(nearestTarget);
            // ������ ���ݹ����� �������� Ÿ�ٿ��� �̵�
            if (!isdetecting)
            {
                AttackMoving(nearestTarget);
            }
            // Ÿ���� ���ݹ����� �������� ����
            else
            {
                navMeshAgent.isStopped = true;
                ani.SetBool("Move", false);

                // 
                if (!isAttacking)
                {
                    attackCoroutine = StartCoroutine(Attack_co());
                    //StartCoroutine(Attack_co());
                }

            }
        }
    }
}