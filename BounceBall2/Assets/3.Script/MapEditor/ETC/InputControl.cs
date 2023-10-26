using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera main;
    [SerializeField] private CameraControl cameraControl;

    // 휠 돌릴때 마우스 포지션 필요함
    private Vector2 previousMousePosition;
    private Vector2 CurrentMousePosition;

    // 타일을 담아놓을 변수
    private Tile_Type current_Type = Tile_Type.Empty;
    
    // 플레이어는 1개밖에 없어야 하기 때문에 담아놓을 변수
    private Tile Player_Tile;

    private void Update()
    {
        // 현재 마우스가 UI 캔버스 위에 있는가 체크
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        UpdateCamera();
        RaycastHit hit;
        if(Input.GetMouseButton(0))
        {
            /*
                RayCast
              > 어떠한 기준의 광선을 쏴서, 피격된 오브젝트의 정보를 불러온다.
                
                ScreenPointToRay
              > 카메라로 부터 화면의 마우스 포지션 위치를 관통하는 광선을 생성
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
        // 키보드 입력으로 카메라 이동
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        cameraControl.SetPosition(x, y);

        // 마우스 휠 버튼을 이용하여 카메라 이동
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

        // 줌인, 줌아웃 Mouse ScrollWheel
        float distance = Input.GetAxisRaw("Mouse ScrollWheel");

        cameraControl.SetOrthographicSize(-distance);
    }

    public void SetTileType(int tiletype)
    {
        current_Type = (Tile_Type)tiletype;
    }
}
