using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    /*
        공격력                     => float
        연사력                     => float => 코루틴
        재장전시간                 => float
        처음 주어질 전체 총알량    => int
        총소리                     => audio clip
        재장전소리                 => audio clip
        탄창용량                   => int
    */

    public float Damage = 25f;
    public float TimebetFire = 0.12f;
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;        // 탄창용량
    public int StartAmmoRemain = 100;   // 처음 주어질 총알

    public AudioClip Shot_Clip;
    public AudioClip Reload_Clip;
}
