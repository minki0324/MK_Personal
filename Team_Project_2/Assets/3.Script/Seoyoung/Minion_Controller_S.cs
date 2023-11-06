/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_Controller_S : MonoBehaviour
{
    *//*
    �̴Ͼ� ��Ʈ�ѷ�
      1. �÷��̾��� Ű �Է¿� ���� ��庯��
      2. ���¿� ���� �ִϸ��̼� ����
  *//*

    public enum Type
    {
        //�̴Ͼ� ����
        //���ڴ� ���� ���� :)
        SwordMan = 15,
        HeavyInfantry = 20,
        Archer = 25,
    }


    private Ply_Controller playerController;
    private Animator ani;

    private CapsuleCollider MeshCollider;  //������ �Ǳ��̴� ������ �ݶ��̴�
    private CapsuleCollider DetectCollider; //���� ������ �ݶ��̴�
    [SerializeField] private UnitAttack UnitAtk;

    private float MaxHP;
    public float CurrentHP { get; private set; }

    private float Damage;
    public bool isDead { get; private set; } = false;

    public Type Human_type;





    public bool isClose = false;







    private void Awake()
    {
        UnitAtk = GetComponent<UnitAttack>();
        playerController = FindObjectOfType<Ply_Controller>();
        MeshCollider = GetComponent<CapsuleCollider>();
        DetectCollider = transform.GetChild(0).GetComponent<CapsuleCollider>();
        TryGetComponent(out ani);
    }

    private void Start()
    {
        Get_HumanType();
    }

    private void Update()
    {
        Behavior_Mode();
        if (isClose == true)
        {
            ani.SetBool("Move", false);

        }
        else
        {

            ani.SetBool("Move", true);
        }
    }

    public void Get_HumanType()
    {
        int num = playerController.Human_num;
        switch (num)
        {
            case 1:
                Human_type = Type.SwordMan;
                MaxHP = 80f;
                Damage = 10f;
                Debug.Log("SwordMan");
                break;

            case 2:
                Human_type = Type.HeavyInfantry;
                MaxHP = 90f;
                Damage = 15f;
                Debug.Log("Infantry");
                break;

            case 3:
                Human_type = Type.Archer;
                MaxHP = 80f;
                Damage = 20f;
                Debug.Log("Archer");
                break;
        }


    }

    private void Behavior_Mode()
    {
        switch (playerController.CurrentMode)
        {
            case Ply_Controller.Mode.Follow:



                // �÷��̾�or�뿭�� �� ����� ��������� �� üũ�ϴ� �޼ҵ� �־ ��������� Bool�� false�� ����, �־����� �ٽ� true�� �����ؼ� ���󰡱�
                break;


            case Ply_Controller.Mode.Attack:
                // ���¿� ���� �ִϸ��̼� �����ؾ���.
                *//*
                    1. �÷��̾��� ������ �̵��Ҷ��� Move�� true�ٲ㼭 �޷����� ���
                    2. ������ �����Ÿ� ���� ������ ���� Move�� false�� �ٲٰ� ��ü�� Idle���� ��ü�� ��Ÿ�ӿ����� Attack Trigger�� �Ѽ� ���� ��� ���ϵ���
                *//*
                UnitAtk.MinionAttack();
                break;
        }
    }




}
*/