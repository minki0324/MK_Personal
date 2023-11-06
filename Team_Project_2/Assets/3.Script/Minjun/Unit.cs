using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : LeaderState
{
   
    private bool isDie;
    private Ply_Controller player;
    //���� ������ ������

    //�������ΰ�?

  
   

    private int myLayer;
    protected int combinedMask;
    // ���� ��� ���̾�
    private LayerMask TeamLayer;
    private bool isSuccessAtk = true;
    //�׾����� �ڽ��ݶ��̴� Enable�ϱ����� �������� 
    [SerializeField] private BoxCollider HitBox_col;
    [SerializeField] private BoxCollider Ob_Weapon_col;

  
    //�׺���̼�
    protected NavMeshAgent navMeshAgent;

    [Header("����Ÿ�� Transform")]
    [SerializeField] protected Transform NearestTarget;
    public Transform GetNearestTarget()
    {
        return NearestTarget;
    }

    [Header("����Ÿ�� Layer")]
    [SerializeField] LayerMask target;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Ply_Controller>();
        navMeshAgent = GetComponent<NavMeshAgent>();
   
    }

    private void Start()
    {
        //�ڽ��� ���̾ ������ �������̾ ���� �迭 ����ϴ� �޼ҵ�

        myLayer = gameObject.layer;
        TeamLayer = LayerMask.NameToLayer("Team");
        combinedMask = TargetLayers();




        //
      


    }

    //���̾� ������ ����� Ÿ�� �����ϴ¸޼ҵ�
    public  Transform GetNearestTarget(RaycastHit[] hits)
    {
        Transform nearest = null;
        float closestDistance = float.MaxValue;

        
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("SpawnPoint"))
            {
                continue;
            }
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
    //�������������� �����ٶ󺸴� �޼ҵ�
    public  void LookatTarget(Transform target)
    {

        Vector3 AttackDir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(AttackDir);
    }
    //�������������� �����ϱ����� ������ �̵��ϴ¸޼ҵ�


    //�����ڷ�ƾ�޼ҵ�
 
    //��Ʈ �ڷ�ƾ�޼ҵ�


    //�̺�Ʈ���� ���� ����Ű�� �޼ҵ�
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
    //������ �޼ҵ�
   
    
   
    public int TargetLayers()
    {
        int[] combinedLayerMask;
        int myLayer = gameObject.layer;
        //�� 4������ ���̾� 
        int[] layerArray = new int[] { LayerMask.NameToLayer("Team"), LayerMask.NameToLayer("Enemy1"), LayerMask.NameToLayer("Enemy2"), LayerMask.NameToLayer("Enemy3") };
        //�츮���� ���̾ ������ ������ ���̾ ���� �迭
        combinedLayerMask = new int[3];
        int combinedIndex = 0;


        for (int i = 0; i < layerArray.Length; i++)
        {
            if (myLayer != layerArray[i])
            {
                combinedLayerMask[combinedIndex] = layerArray[i];
                combinedIndex++;
            }

        }
        int layerMask0 = 1 << combinedLayerMask[0];
        int layerMask1 = 1 << combinedLayerMask[1];
        int layerMask2 = 1 << combinedLayerMask[2];
        combinedMask = layerMask0 | layerMask1 | layerMask2;
        return combinedMask;
    }
    //�ڽ��� ������ �������������� ��������ϱ����Ѹ޼ҵ�

}
