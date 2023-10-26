using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ZombieData", fileName = "ZombieData")]
public class ZombieData : ScriptableObject
{
    /*
         체력
         이동속도
         공격력
         피부색
    */

    public float Health = 100f;
    public float Damage = 20f;
    public float Speed = 2f;
    public Color Skincolor = Color.white;
}
