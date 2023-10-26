using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public bool isGameOver = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("�� ���ӿ��� �̹� �Ŵ����� �־��");
            Destroy(gameObject);
        }
    }

    public void Player_Dead()
    {
        isGameOver = true;
    }

}
