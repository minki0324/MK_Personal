using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text text;

    private float GameTime;

    private void Update()
    {
        GameTime += Time.deltaTime;

        int sec = Mathf.FloorToInt(GameTime % 60f);
        int min = Mathf.FloorToInt(GameTime / 60f);
        text.text = $"{min:D2} : {sec:D2}";

    }
}
