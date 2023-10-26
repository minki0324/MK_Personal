using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Score : MonoBehaviour
{
    private int Player_score = 0;
    public int player_Score => Player_score;

    [SerializeField] private Text Score;

    private void Awake()
    {
        Player_score = 0;    
    }

    public void set_Score(int score)
    {
        Player_score += score;
        Score.text = $"Score : {Player_score}";
    }

    public void Savescore()
    {
        /*
            PlayerPrefs?
         > 씬 이동을 하기 위해서 변수를 static으로 선언하는 것이 아니라
           PlayerPrefs라는 것을 사용하여서 키-Value값으로 저장 한다.
           만약 키가 존재한다면, 다시 덮어쓰는 방식

        [불러올때]
        PlayerPrefs.GetInt("Score");
        
        [저장할때]
        PlayerPrefs.SetInt("Score", Player_score);
        */

        PlayerPrefs.SetInt("Score", Player_score);
    }
}
