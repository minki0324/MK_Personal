using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonWeapon : MonoBehaviour
{
    /*
      �Ѿ��� ���� �� �����̰� �־�� ��
      �ڸ�ƾ�� �ϴ� ���� ����Ƽ���� ������� ������Ʈ�� �������� �ȴ�.
      �� ����Ƽ ������ ��Ÿ���� ��� ���� �� �ִ°�
  */
    [SerializeField] private GameObject Mon_bullet;
    [SerializeField] private float Attack_Rate = 0.5f;


    public void TryAtk()
    {
        Instantiate(Mon_bullet, transform.position, Quaternion.identity);
    }

    private IEnumerator TryMonAtk_co()
    {
        while (true)
        {
            Instantiate(Mon_bullet, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(Attack_Rate);
        }
    }

    public void StartFire()
    {
        StartCoroutine("TryMonAtk_co");
    }
    public void StopFire()
    {
        StopCoroutine("TryMonAtk_co");
    }
}
