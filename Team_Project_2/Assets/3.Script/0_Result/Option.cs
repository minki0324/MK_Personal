using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Slider[] slider;
    [SerializeField] private Text[] Textarray;

    private void Update()
    {
        slider[0].value = 0 / 2f;
        slider[1].value = 0 / 2f;
        Textarray[0].text = "1.00";  // ���콺 ���� ����
        Textarray[1].text = "1.00";  // ������ ���� ����        
    }

    public void ToTitle()
    {

    }
    public void EndGame()
    {

    }
}
