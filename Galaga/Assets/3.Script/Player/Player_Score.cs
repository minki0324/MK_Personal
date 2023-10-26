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
         > �� �̵��� �ϱ� ���ؼ� ������ static���� �����ϴ� ���� �ƴ϶�
           PlayerPrefs��� ���� ����Ͽ��� Ű-Value������ ���� �Ѵ�.
           ���� Ű�� �����Ѵٸ�, �ٽ� ����� ���

        [�ҷ��ö�]
        PlayerPrefs.GetInt("Score");
        
        [�����Ҷ�]
        PlayerPrefs.SetInt("Score", Player_score);
        */

        PlayerPrefs.SetInt("Score", Player_score);
    }
}
