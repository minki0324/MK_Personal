using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    public static int MaxStageCount;
    // Tilemap 2D 컴포넌트를 맵을 직접적으로 만드는 곳
    [Header("타일맵 생성")]
    [SerializeField] TileMap2D tileMap2D;

    [Header("플레이어, 카메라 컨트롤")]
    [SerializeField] PlayerControl playercontrol;
    [SerializeField] CameraControl_G cameraControl_G;

    [Header("스테이지 UI 컨트롤")]
    [SerializeField] private Stage_UI Stage_UI;

    private void Awake()
    {
        MapData_Load load = new MapData_Load();
        int index = PlayerPrefs.GetInt("StageIndex") + 1;
        string current_stage = index < 10 ? $"Stage0{index}" : $"Stage{index}"; 
        MapData map = load.Load(current_stage);
        tileMap2D.Generate_tilemap(map);

        playercontrol.Setup(map.PlayerPosition, map.Mapsize.y);
        cameraControl_G.Setup(map.Mapsize.x, map.Mapsize.y);
        Stage_UI.UpdateTextStage(current_stage);

    }

    public void GameClear ()
    {
        int index = PlayerPrefs.GetInt("StageIndex");

        if(index < MaxStageCount-1)
        {
            index++;
            PlayerPrefs.SetInt("StageIndex", index);
            SceneLoader.LoadScene();
        }

    }
}
