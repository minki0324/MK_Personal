using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle : MonoBehaviour
{
    [SerializeField] private float fade_time;
    private Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
        StartCoroutine(Twinkle_co());
    }

    private IEnumerator Twinkle_co()
    {
        while(true)
        {
            yield return StartCoroutine(Fade(1, 0));
            // 이 코르틴이 끝날때 까지 다음 코르틴으로 넘어가지 않는다.
            yield return StartCoroutine(Fade(0, 1));

        }
    }

    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;
        while(percent<1)
        {
            current += Time.deltaTime;
            percent = current / fade_time;

            Color color = text.color;
            color.a = Mathf.Lerp(start, end, percent);
            text.color = color;

            yield return null; // 한프레임씩;
        }
    }
}
