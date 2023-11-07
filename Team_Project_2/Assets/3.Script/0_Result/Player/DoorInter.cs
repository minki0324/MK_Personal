using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInter : MonoBehaviour
{
    // ���� ���� �ݴ� ��ũ��Ʈ

    [SerializeField] private Animator Door_Ani;
    [SerializeField] private BoxCollider boxcol;  // �� �ڽ���
    private bool isOpen = false;

    private void OnEnable()
    {
        Door_Ani.SetTrigger("OpenDoor");
        isOpen = true;
        boxcol.enabled = false;
    }

    public IEnumerator OpenDoor_co()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                if(isOpen)
                {
                    Debug.Log("�ݾ�");
                    Door_Ani.SetTrigger("CloseDoor");
                    isOpen = false;
                    boxcol.enabled = true;
                }
                else
                {
                    Debug.Log("����");
                    Door_Ani.SetTrigger("OpenDoor");
                    isOpen = true;
                    boxcol.enabled = false;
                }
            }
            yield return null;
        }
    }

}
