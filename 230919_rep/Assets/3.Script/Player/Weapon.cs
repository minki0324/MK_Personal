using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*
        총알은 나갈 때 딜레이가 있어야 됨
        코르틴을 하는 순간 유니티에서 제어권을 오브젝트로 가져오게 된다.
        즉 유니티 라이프 스타일을 잠시 멈출 수 있는것
    */
    [SerializeField] private GameObject Player_bullet;
    [SerializeField] private float Attack_Rate = 0.5f;
    

    public void TryAtk()
    {
        Instantiate(Player_bullet, transform.position, Quaternion.identity);
    }

    private IEnumerator TryAtk_co()
    {
        while(true)
        {
            Instantiate(Player_bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Attack_Rate);
        }
    }

    public void StartFire()
    {
        StartCoroutine("TryAtk_co");
    }
    public void StopFire()
    {
        StopCoroutine("TryAtk_co");
    }
}
