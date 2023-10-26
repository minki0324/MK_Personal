using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private TileMap2D tileMap;
    private Camera main;

    // 카메라 이동속도
    [SerializeField] private float Movespeed;
    // 카메라 줌 속도
    [SerializeField] private float Zoomspeed;
    // 2D 한정 카메라 시야 최소크기
    [SerializeField] private float minViewsize = 2;
    // 2D 한정 카메라 시야 최대크기
    [SerializeField] private float maxViewsize;

    private float wDelta = 0.4f;
    private float hDelta = 0.6f;

    private void Awake()
    {
        main = GetComponent<Camera>();
    }

    public void SetupCamera() // 카메라 초기 설정
    {
        // 맵 크기 정보
        int width = tileMap.width;
        int height = tileMap.Height;

        // 카메라 시야 설정
        float size = (width > height) ? width * wDelta : height * hDelta;
        main.orthographicSize = size;

        if(height > width)
        {
            Vector3 position = new Vector3(0, 0.05f, -10f); // 카메라 한 타일의 단위.
            position.y *= height;
            transform.position = position;
        }
        maxViewsize = main.orthographicSize;
    }

    public void SetPosition(float x, float y)
    {
        transform.position += new Vector3(x, y, 0) * Movespeed * Time.deltaTime;
    }

    public void SetOrthographicSize(float size)
    {
        if (size == 0)
        {
            return;
        }
        main.orthographicSize += size * Zoomspeed * Time.deltaTime;
        main.orthographicSize = Mathf.Clamp(main.orthographicSize, minViewsize, maxViewsize);
    }

}
