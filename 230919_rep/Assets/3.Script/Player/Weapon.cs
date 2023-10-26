using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /*
        �Ѿ��� ���� �� �����̰� �־�� ��
        �ڸ�ƾ�� �ϴ� ���� ����Ƽ���� ������� ������Ʈ�� �������� �ȴ�.
        �� ����Ƽ ������ ��Ÿ���� ��� ���� �� �ִ°�
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
