using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
public class LeaderAI : Unit
{
    private float scanRange = 10f;
    private LayerMask targetLayer;
    private int invertedLayerMask;
    private NavMeshAgent navMesh;
    private Animator ani;
    private GameObject[] flag;
    [SerializeField] private GameObject targetFlag;
   
    //public Transform nearestTarget;
    private float AttackRange = 5f;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        combinedMask = TargetLayers();
        navMesh = GetComponent<NavMeshAgent>();
        bat_State = BattleState.Follow;
        flag = GameObject.FindGameObjectsWithTag("Flag");
    }
    private void Update()
    {
        for (int i = 0; i < flag.Length; i++)
        {
            Debug.Log(flag[i]);
        }

        // �׻� �ֺ��� �����ִ��� Ž��
        EnemyDitect();
        switch (bat_State)
        {
            case BattleState.Follow:
                //if(targetFlag.transform.position != null) { 
                //navMesh.SetDestination(targetFlag.transform.position);
                //}
                //else
                //{
                //    return;
                //}
                //navMesh.isStopped = true;
                break;
            case BattleState.Attack:
                break;
            case BattleState.Detect:

                //�ִϸ��̼� ���е��
                ani_State = AniState.shild;
                //õõ�� ������ ����
                //Debug.Log()
                ani.SetBool("Move", true);
                navMesh.SetDestination(NearestTarget.position);
                break;

        }

        switch (jud_State)
        {
            case JudgmentState.Ready:
                ani_State = AniState.Idle;
                Debug.Log("Ÿ�ٱ������");
                targetFlag= TargetFlag();
                navMesh.SetDestination(targetFlag.transform.position);
                jud_State = JudgmentState.Going;
                //navMesh.SetDestination()
                break;
            case JudgmentState.Wait:
                break;
            case JudgmentState.Going:
                //�װų� 
                break;
           

                //float originalSpeed = navMeshAgent.speed; // ���� �ӵ� ����
                //navMeshAgent.speed = originalSpeed / 4; // 1/4�� ���� �ӵ� ����



        }



    }

    private void EnemyDitect()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, scanRange, Vector3.forward, 0, combinedMask);
        NearestTarget = GetNearestTarget(hits);
        if (NearestTarget != null)
        {
            float attackDistance = Vector3.Distance(transform.position, NearestTarget.position);
            bat_State = BattleState.Detect;

            //DItect �����϶� ���и� ��� õõ�� ����
            if (attackDistance <= AttackRange)
            {
                bat_State = BattleState.Attack;
            }
        }
        else
        {
            if(bat_State == BattleState.Attack) { 
            if(targetFlag == null)
            {
                jud_State = JudgmentState.Ready;
            }
            else
            {
                jud_State = JudgmentState.Going;
            }
           
            bat_State = BattleState.Follow;
            }
        }


        //�������� ���ݶ��̴��� ������ Ditect ���·� ����
    }

    private GameObject TargetFlag()
    {
        GameObject[] defaultFlags = flag.Where(_flag => _flag.layer != gameObject.layer).ToArray();
        if (defaultFlags.Length > 0)
        {
            int randomIndex = Random.Range(0, defaultFlags.Length);
            GameObject selected1Flag = defaultFlags[randomIndex];

            return selected1Flag;
            // ���õ� ��ü(selectedFlag)�� ����ϼ���.
        }

        GameObject selectedFlag = null;
        int minTouchingCount = int.MaxValue;
        int layerMask = (1 << LayerMask.NameToLayer("Team")) | (1 << LayerMask.NameToLayer("Enemy1")) | (1 << LayerMask.NameToLayer("Enemy2")) | (1 << LayerMask.NameToLayer("Enemy3"));
        int radius = 10;
        foreach (GameObject _flag in flag)
        {
            // _flag �ֺ����� trigger�� ��� �ִ� ��ü ����
            Collider[] colliders = Physics.OverlapSphere(_flag.transform.position, radius, layerMask, QueryTriggerInteraction.Collide);

            // �ּ� ī��Ʈ ����
            if (colliders.Length < minTouchingCount)
            {
                minTouchingCount = colliders.Length;
                selectedFlag = _flag;
            }
        }

        if (selectedFlag != null)
        {
            return selectedFlag;
        }
        else
        {
            Debug.Log("��߸�ã��");
            return null;

        }
       

    }
}