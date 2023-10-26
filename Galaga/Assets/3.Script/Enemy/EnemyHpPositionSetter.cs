using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpPositionSetter : MonoBehaviour
{
    [SerializeField] private Vector3 distance = Vector3.up * 35f;
    private GameObject Target; // 달아줄 적
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

        // 적 오브젝트는 위치가 지속적으로 갱신이 되는데 이거를 어떻게 따라가게 하느냐?
        // UI -> 캔버스를 상속을 받는데 상속을 적으로 바꾸면 렌더링이 안됨

        // 그래서 채신기술 사용
        // WorldToScreenPoint : 해당 포인트의 지점을 찍는 것 (여기서는 카메라가 보고 있으니 해당 카메라에서 매개변수의 현재 포인트를 찍음) // (3D 환경에서는 마우스가 찍은 포인트를 가져올때 주로 사용됨)
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(Target.transform.position);
        UItransform.position = screenPosition + distance;
    }
}
