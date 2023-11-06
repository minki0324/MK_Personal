using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderState : MonoBehaviour
{

    public enum BattleState {
    
        Follow, //������ �̵��Ҷ� 
        Attack,  // AI�� ���� �����ϰ� �����ð� �Ǵ� �Ÿ��������� 
        Detect,
        
    }
    public enum JudgmentState
    {
        Ready, //��� �����º��� ������ ������ �̵�
        Wait, //��������º��� ������ �������� ����ϸ� �ֵ�̱�
        Going

    }
    public enum AniState 
    {
        Idle,
        Attack,
        shild,
        Order
    }





    [Header("��� ����")]
    public float Gold = 500; // ��差
    // private float Magnifi = 2f;  // �⺻ ��� ���� (������Ʈ�� ������ 60 x 2f�� �⺻ ȹ�� ��差�� �д� 120)

    [Header("AI ����")]
    public bool isLive = true;
    // private bool Ready =true;
    public float Current_HP = 150f;
    public float Max_Hp = 150f;
    public float Regeneration = 0.5f;
    public int maxUnitCount = 19;
    public int currentUnitCount = 0;
    public int unitValue = 0;
    public float unitCost =16f;
    public bool canSpawn;
    public bool isDead;
    public bool isMoving;
    public BattleState bat_State;
    public JudgmentState jud_State;
    public AniState ani_State;

    public List<GameObject> UnitList = new List<GameObject>();

  
    //AI �ൿ �켱����
    /*
     1. �߸������� ������
     2. �߸������� �ƴ����� �ƹ��� ������ 
     3. 
     
     1.����
     2. �ƹ��������� ����
     
     */

}
