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
            Debug.Log("이 게임에는 이미 매니저가 있어요");
            Destroy(gameObject);
        }
    }

    public void Player_Dead()
    {
        isGameOver = true;
    }

}
