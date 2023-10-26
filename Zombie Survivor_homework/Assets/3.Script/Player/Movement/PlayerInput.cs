using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private string MoveAxis_name = "Vertical";
    [SerializeField] private string Rotate_name = "Horizontal";
    [SerializeField] private string Fire = "Fire1";
    [SerializeField] private string Reload = "Reload";

    // GetAxis => ��ȯ�� float
    public float Move_Value { get; private set; }
    public float Rotate_Value { get; private set; }

    // GetButton => ��ȯ�� bool
    public bool isFire { get; private set; }
    public bool isReload { get; private set; }

    private void Update()
    {
        // GameOver�� ����ٸ� �� �����̵��� �����ϱ�

        Move_Value = Input.GetAxis(MoveAxis_name);
        Rotate_Value = Input.GetAxis(Rotate_name);

        isFire = Input.GetButton(Fire);
        isReload = Input.GetButton(Reload);
    }
}
