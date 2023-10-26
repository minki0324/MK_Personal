using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField] private Slider sliderHP;
    [SerializeField] private PlayerController player;

    private void Start()
    {
        TryGetComponent(out sliderHP);
        GameObject.FindGameObjectWithTag("Player").TryGetComponent(out player);
    }

    private void Update()
    {
        sliderHP.value = player.currenthp / player.MAXHP;
    }
}
