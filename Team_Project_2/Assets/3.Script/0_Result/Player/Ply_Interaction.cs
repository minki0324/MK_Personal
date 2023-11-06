using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ply_Interaction : MonoBehaviour
{
    // �÷��̾� ��ȣ�ۿ�

    // ����
    // 1. ���� �� ��� �� ����
    // 2. ���� �� ����Ʈ�� �ӵ� ����
    // 3. �ֺ� ���� ���� ���� ���� �����̴� ����
    // �Ϸ����� ���ڼ� �˼��մϴ�,,

    [SerializeField] private Occupation occupation; // ���� ��ũ��Ʈ
    [SerializeField] private DoorInter Doorinter;   // �� ���� ��ũ��Ʈ
    [SerializeField] private Text Doorui; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {
            occupation.ObjEnable(true);

            StopCoroutine(occupation.UnOccu_co());
            StartCoroutine(occupation.Occu_co());
        }
        if (other.gameObject.CompareTag("Door"))
        {
            Doorui.gameObject.SetActive(true);
            StartCoroutine(Doorinter.OpenDoor_co());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Flag"))
        {

            occupation.ObjEnable(false);
            StopCoroutine(occupation.Occu_co());
            StartCoroutine(occupation.UnOccu_co());
        }
        if (other.gameObject.CompareTag("Door"))
        {
            Doorui.gameObject.SetActive(false); 
            StopCoroutine(Doorinter.OpenDoor_co());
        }
    }
    
}
