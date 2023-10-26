using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Destroy(gameObject);
        }
    }

    public int Score = 0;
    public bool isGameOver { get; private set; }

    private void Start()
    {
        FindObjectOfType<PlayerHealth>().onDead += EndGame;
    }

    public void EndGame()
    {
        isGameOver = true;
        HUD.Instance.SetActiveGameOver(true);
    }

    public void AddScore(int newScore)
    {
        if(!isGameOver)
        {
            Score += newScore;
            HUD.Instance.Update_Score(Score);
        }
    }
    
    public void ReStart()
    {
        isGameOver = false;
        Score = 0;
        HUD.Instance.Update_Score(Score);

    }
}
