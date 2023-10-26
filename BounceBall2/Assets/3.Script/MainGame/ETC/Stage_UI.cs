using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_UI : MonoBehaviour
{
    [SerializeField] private Text TextStage;
    [SerializeField] private Text TextCoin;

    public void UpdateTextStage(string stage)
    {
        TextStage.text = stage;
    }

    public void UpdateTextCoin(int current, int max)
    {
        TextCoin.text = $"Coin {current} / {max}";
    }
}
