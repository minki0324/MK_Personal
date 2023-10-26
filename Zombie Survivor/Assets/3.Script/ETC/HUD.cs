using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    /*
        탄약 표시용 텍스트
        점수 표시용 텍스트 -> gamemanager에서 관리
        적 웨이브 표시용 텍스트
        게임 오버 오브젝트
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

    // 탄약 업데이트
    public void Update_Ammo(int magAmmo, int Remain)
    {
        Ammo.text = string.Format("{0} / {1}", magAmmo, Remain);
    }

    // 스코어 업데이트
    public void Update_Score(int newScore)
    {
        Score.text = string.Format("Score : {0}", newScore);
    }

    // 웨이브 업데이트
    public void Update_Wave(int wave, int count)
    {
        Wave.text = string.Format("Wave : {0}\nZombie Left : {1}", wave, count);
    }

    // 게임오버 UI
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
