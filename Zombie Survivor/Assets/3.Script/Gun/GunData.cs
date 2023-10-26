using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/GunData", fileName = "GunData")]
public class GunData : ScriptableObject
{
    /*
        ���ݷ�                     => float
        �����                     => float => �ڷ�ƾ
        �������ð�                 => float
        ó�� �־��� ��ü �Ѿ˷�    => int
        �ѼҸ�                     => audio clip
        �������Ҹ�                 => audio clip
        źâ�뷮                   => int
    */

    public float Damage = 25f;
    public float TimebetFire = 0.12f;
    public float ReloadTime = 1.8f;
    public int MagCapacity = 30;        // źâ�뷮
    public int StartAmmoRemain = 100;   // ó�� �־��� �Ѿ�

    public AudioClip Shot_Clip;
    public AudioClip Reload_Clip;
}
