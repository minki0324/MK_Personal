using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ZombieData", fileName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    /*
         ü��
         �̵��ӵ�
         ���ݷ�
         �Ǻλ�
    */

    public float Health = 100f;
    public float Damage = 20f;
    public float Speed = 2f;
    public Color Skincolor = Color.white;
}
