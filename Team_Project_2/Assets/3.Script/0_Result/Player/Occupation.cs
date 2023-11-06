using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Occupation : MonoBehaviour
{
    // ����
    // 1. ���� �� ��� �� ����
    // 2. ���� �� ����Ʈ�� �ӵ� ����
    // 3. �ֺ� ���� ���� ���� ���� �����̴� ����


    [Header("���")]
    [SerializeField] SkinnedMeshRenderer skinnedmesh;

    [Header("�� ����")]
    [SerializeField] private Material[] Flag_Color; // ��� ���ٲ� Marterial
    [SerializeField] private Image[] Occu_Back; // ���� ���� �� ��
    [SerializeField] private ColorSet color;
    [SerializeField] private Transform player;

    public Slider OccuValue; // ���� ������

    private float Num_Soldier = 1.03f; // ��� ���� ���� ����
    public float occu_Speed = 15f; // ���� �ӵ�
    private float Total_Gauge = 100f; // ��ü ���� ������
    private float Current_Gauge = 0;  // ���� ���� ������

    public bool isOccupating = false; // ���� ������
    [SerializeField] private bool isOccupied = false; // ������ ��������

    private Ply_Controller ply_Con;
    private void Awake()
    {
        Occu_Back = GetComponentsInChildren<Image>();
        ply_Con = FindObjectOfType<Ply_Controller>();
        for (int i = 0; i < Occu_Back.Length * 0.5f; i++) 
        {
            Occu_Back[i * 2 + 1].transform.parent.gameObject.SetActive(false);

        }
    }

    private void Update()
    {
        if (OccuValue.value >= 1 && !isOccupied) 
        {
            Change_Color();
            GameManager.instance.Occupied_Area++;
            isOccupied = true;
        }
      
    }

    public void ObjEnable(bool act)
    {
        Occu_Back[1].transform.parent.gameObject.SetActive(act);
        OccuValue.gameObject.SetActive(act);
        isOccupating = act;
    }
    
      
    

    private void Change_Color()
    {
        // ���߿� �÷����� ����
        skinnedmesh.material = Flag_Color[GameManager.instance.Color_Index];
        color.RecursiveSearchAndSetTexture(player, GameManager.instance.Color_Index);
        Occu_Back[0].color = new Color32(255, 0, 0, 110);
        Occu_Back[1].color = new Color32(255, 0, 0, 110);
    }
    


    public IEnumerator Occu_co()
    {

        // ���� ��
        while (isOccupating && Current_Gauge <= 100f)
        {
            Current_Gauge += Time.deltaTime * occu_Speed * Num_Soldier * (ply_Con.Current_MinionCount + 1); // ���߿� �ο����� ���� ���� �־���ؿ�
            Debug.Log(Current_Gauge);
            OccuValue.value = Current_Gauge / Total_Gauge;
            yield return null;
        }

    }
    public IEnumerator UnOccu_co()
    {
        //OccuValue.gameObject.SetActive(false);
        yield return new WaitForSeconds(3.0f);
        while (!isOccupied && !isOccupating && Current_Gauge >= 0f)
        {
            Current_Gauge -= Time.deltaTime * occu_Speed;
            OccuValue.value = Current_Gauge / Total_Gauge;



            yield return null;
        }
    }



}
