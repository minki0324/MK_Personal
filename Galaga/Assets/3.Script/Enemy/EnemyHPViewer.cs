using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPViewer : MonoBehaviour
{
    private EnemyController enemy;
    [SerializeField]private Slider slider;

    public void Setup(EnemyController enemy)
    {
        this.enemy = enemy;
        TryGetComponent(out slider);
    }

    private void Update()
    {
        slider.value = enemy.CurrentHP / enemy.MAXHP;
    }
}
