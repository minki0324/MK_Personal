using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpPositionSetter : MonoBehaviour
{
    [SerializeField] private Vector3 distance = Vector3.up * 35f;
    private GameObject Target; // �޾��� ��
    private RectTransform UItransform;

    public void Setup(GameObject target)
    {
        Target = target;
        UItransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!Target.activeSelf)
        {
            Destroy(gameObject);
            return;
        }

        // �� ������Ʈ�� ��ġ�� ���������� ������ �Ǵµ� �̰Ÿ� ��� ���󰡰� �ϴ���?
        // UI -> ĵ������ ����� �޴µ� ����� ������ �ٲٸ� �������� �ȵ�

        // �׷��� ä�ű�� ���
        // WorldToScreenPoint : �ش� ����Ʈ�� ������ ��� �� (���⼭�� ī�޶� ���� ������ �ش� ī�޶󿡼� �Ű������� ���� ����Ʈ�� ����) // (3D ȯ�濡���� ���콺�� ���� ����Ʈ�� �����ö� �ַ� ����)
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position);
        UItransform.position = screenPosition + distance;
    }
}
