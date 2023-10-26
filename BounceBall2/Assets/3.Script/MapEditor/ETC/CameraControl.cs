using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private TileMap2D tileMap;
    private Camera main;

    // ī�޶� �̵��ӵ�
    [SerializeField] private float Movespeed;
    // ī�޶� �� �ӵ�
    [SerializeField] private float Zoomspeed;
    // 2D ���� ī�޶� �þ� �ּ�ũ��
    [SerializeField] private float minViewsize = 2;
    // 2D ���� ī�޶� �þ� �ִ�ũ��
    [SerializeField] private float maxViewsize;

    private float wDelta = 0.4f;
    private float hDelta = 0.6f;

    private void Awake()
    {
        main = GetComponent<Camera>();
    }

    public void SetupCamera() // ī�޶� �ʱ� ����
    {
        // �� ũ�� ����
        int width = tileMap.width;
        int height = tileMap.Height;

        // ī�޶� �þ� ����
        float size = (width > height) ? width * wDelta : height * hDelta;
        main.orthographicSize = size;

        if(height > width)
        {
            Vector3 position = new Vector3(0, 0.05f, -10f); // ī�޶� �� Ÿ���� ����.
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
