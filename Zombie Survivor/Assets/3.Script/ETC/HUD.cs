using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    /*
        ź�� ǥ�ÿ� �ؽ�Ʈ
        ���� ǥ�ÿ� �ؽ�Ʈ -> gamemanager���� ����
        �� ���̺� ǥ�ÿ� �ؽ�Ʈ
        ���� ���� ������Ʈ
    */

    [SerializeField] private Text Ammo;
    [SerializeField] private Text Score;
    [SerializeField] private Text Wave;
    [SerializeField] private GameObject Gameover;
 
    public static HUD Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // ź�� ������Ʈ
    public void Update_Ammo(int magAmmo, int Remain)
    {
        Ammo.text = string.Format("{0} / {1}", magAmmo, Remain);
    }

    // ���ھ� ������Ʈ
    public void Update_Score(int newScore)
    {
        Score.text = string.Format("Score : {0}", newScore);
    }

    // ���̺� ������Ʈ
    public void Update_Wave(int wave, int count)
    {
        Wave.text = string.Format("Wave : {0}\nZombie Left : {1}", wave, count);
    }

    // ���ӿ��� UI
    public void SetActiveGameOver(bool isActive)
    {
        Gameover.SetActive(isActive);
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(0);
        SetActiveGameOver(false);
        GameManager.Instance.ReStart();
    }
}
