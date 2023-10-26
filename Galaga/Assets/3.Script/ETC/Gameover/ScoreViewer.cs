using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour
{
    private Text score_text;

    private void Start()
    {
        TryGetComponent(out score_text);

        score_text.text = $"Score : {PlayerPrefs.GetInt("Score")}";
    }
}
