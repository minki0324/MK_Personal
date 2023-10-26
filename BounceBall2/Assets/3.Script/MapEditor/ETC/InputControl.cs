using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera main;
    [SerializeField] private CameraControl cameraControl;

    // �� ������ ���콺 ������ �ʿ���
    private Vector2 previousMousePosition;
    private Vector2 CurrentMousePosition;

    // Ÿ���� ��Ƴ��� ����
    private Tile_Type current_Type = Tile_Type.Empty;
    
    // �÷��̾�� 1���ۿ� ����� �ϱ� ������ ��Ƴ��� ����
    private Tile Player_Tile;

    private void Update()
    {
        // ���� ���콺�� UI ĵ���� ���� �ִ°� üũ
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        UpdateCamera();
        RaycastHit hit;
        if(Input.GetMouseButton(0))
        {
            /*
                RayCast
              > ��� ������ ������ ����, �ǰݵ� ������Ʈ�� ������ �ҷ��´�.
                
                ScreenPointToRay
              > ī�޶�� ���� ȭ���� ���콺 ������ ��ġ�� �����ϴ� ������ ����
            */

            Ray ray = main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin,ray.direction * 100f, Color.red);
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Tile tile = hit.transform.GetComponent<Tile>();
                if(tile != null)
                {
                    if(current_Type.Equals(Tile_Type.Player))
                    {
                        if(Player_Tile != null)
                        {
                            Player_Tile.Tiletype = Tile_Type.Empty;
                        }
                        Player_Tile = tile;
                    }

                    tile.Tiletype = current_Type;
                }
            }
        }
    }

    public void UpdateCamera()
    {
        // Ű���� �Է����� ī�޶� �̵�
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        cameraControl.SetPosition(x, y);

        // ���콺 �� ��ư�� �̿��Ͽ� ī�޶� �̵�
        if(Input.GetMouseButtonDown(2))
        {
            CurrentMousePosition = previousMousePosition = Input.mousePosition;
        }
        else if(Input.GetMouseButton(2))
        {
            CurrentMousePosition = Input.mousePosition;
            if(previousMousePosition != CurrentMousePosition)
            {
                Vector2 move = (previousMousePosition - CurrentMousePosition) * 0.5f;
                cameraControl.SetPosition(move.x, move.y);
            }
        }
        previousMousePosition = CurrentMousePosition;

        // ����, �ܾƿ� Mouse ScrollWheel
        float distance = Input.GetAxisRaw("Mouse ScrollWheel");

        cameraControl.SetOrthographicSize(-distance);
    }

    public void SetTileType(int tiletype)
    {
        current_Type = (Tile_Type)tiletype;
    }
}
