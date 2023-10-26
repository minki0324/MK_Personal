using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMap2D : MonoBehaviour
{
    [Header("Game의 경우는 Check")]
    public bool isGame = false;

    [Header("Game의 경우")]
    [SerializeField] private GameObject[] Tile_Prefabs_G;
    [SerializeField] private GameObject Item_Prefabs_G;

    private int Maxcoin = 0;
    private int Currentcoin = 0;
    [SerializeField] private Stage_UI stage_UI;
    [SerializeField] private StageControl stageControl;
    [SerializeField] private List<TileBlink> blinkTiles;
    [SerializeField] private Movement2D movement;

    [Header("MapEditor의 경우")]                             // 인스펙터창 단위 나누는 명령어
    [SerializeField] private GameObject TilePrefabs;

    [Header("InputField")]
    [SerializeField] private InputField input_Width;
    [SerializeField] private InputField input_Height;

    public int width { get; private set; } = 10;
    public int Height { get; private set; } = 10;

    public List<Tile> tilelist { get; private set; }
    private MapData mapData;

    private void Awake()
    {
        if (isGame) return;
        input_Width.text = width.ToString();
        input_Height.text = Height.ToString();
        mapData = new MapData();
        tilelist = new List<Tile>();
    }

    #region 맵 에디터 스크립트
    // 버튼 이벤트로 사용될 거라 퍼블릭으로 사용
    public void Generate_tilemap()
    {
        if(int.TryParse(input_Width.text, out int _width) && int.TryParse(input_Height.text, out int _height)) // 받아온 스트링 값을 인트값으로 변환
        {
            width = _width;
            Height = _height;
        }
        // 맵 만드는 이중for문
        for (int y = 0; y < Height; y++) 
        {
            for(int x = 0; x < width; x++)
            {
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (Height * 0.5f - 0.5f) - y, 0);

                Spawn_Tile(Tile_Type.Empty, position);
            }
        }
        mapData.Mapsize.x = width;
        mapData.Mapsize.y = Height;
        mapData.Mapdata = new int[tilelist.Count];
    }

    private void Spawn_Tile(Tile_Type type, Vector3 position)
    {
        GameObject clone = Instantiate(TilePrefabs, position, Quaternion.identity);

        clone.name = "tile";
        clone.transform.SetParent(transform); // 타일맵 오브젝트에 상속시키기

        Tile tile = clone.GetComponent<Tile>();
        tile.Setup(type);

        tilelist.Add(tile);
    }

    public MapData GetMapData()
    {
        for (int i = 0; i < tilelist.Count; i++) 
        {
            if(tilelist[i].Tiletype != Tile_Type.Player) // 플레이어가 아니라면
            {
                mapData.Mapdata[i] = (int)tilelist[i].Tiletype;
            }
            else // 플레이어 라면
            {
                mapData.Mapdata[i] = (int)Tile_Type.Empty;

                int x = (int)tilelist[i].transform.position.x;
                int y = (int)tilelist[i].transform.position.y;

                mapData.PlayerPosition = new Vector2Int(x, y);
            }
        }

        return mapData;
    }
    #endregion

    #region 메인 게임 스크립트
    public void Generate_tilemap(MapData map)
    {
        blinkTiles = new List<TileBlink>();

        int width = map.Mapsize.x;
        int height = map.Mapsize.y;

        for (int y = 0; y < height; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;

                if(map.Mapdata[index].Equals((int)TileType_G.Empty))
                {
                    continue;
                }

                // 생성되는 타일맵 중앙이 0, 0, 0인 위치
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (height * 0.5f - 0.5f) - y, 0);

                // 예외처리
                if (map.Mapdata[index] > (int)TileType_G.Empty && map.Mapdata[index] < (int)TileType_G.LastIndex)
                {
                    // 타일 만드는 메소드 추가
                    Spawn_Tile((TileType_G)map.Mapdata[index], position);
                }
                else if (map.Mapdata[index] >= (int)Item_Type.Coin) 
                {
                    // 아이템 만드는 메소드 추가
                    Spawn_Item(position);
                }
            }
        }
        stage_UI.UpdateTextCoin(Currentcoin, Maxcoin);

        // TileBlink blink 객체 하나하나 접근하는데 인덱스로 접근 하는것이 아닌 자료구조의 객체에게 접근하는 방식
        foreach (TileBlink blink in blinkTiles)
        {
            //blink 타일들한테 각각 타일 알려주는 메소드 호출
            blink.SetupBlinkTile(blinkTiles);
        }
    }
    public void Spawn_Tile(TileType_G type, Vector3 position)
    {
        // 나중에 타일 타입에 따른 것들이 추가가 되면 변경 되어야 할 부분 [완료]
        GameObject clone = Instantiate(Tile_Prefabs_G[(int)type-1], position, Quaternion.identity);

        clone.transform.SetParent(transform);
        clone.transform.name = "Tile";
        Tile_G tile = clone.GetComponent<Tile_G>();
        tile.Setup(movement);

        if(type.Equals(TileType_G.Blink))
        {
            blinkTiles.Add(clone.GetComponent<TileBlink>());
        }
    }

    public void Spawn_Item(Vector3 position)
    {
        GameObject clone = Instantiate(Item_Prefabs_G, position, Quaternion.identity);

        clone.transform.SetParent(transform);
        clone.transform.name = "Item";
        Maxcoin++;
    }

    public void GetCoin(GameObject coin)
    {
        Currentcoin++;

        stage_UI.UpdateTextCoin(Currentcoin, Maxcoin);

        coin.GetComponent<Item>().Exit();

        if(Currentcoin == Maxcoin)
        {
            stageControl.GameClear();
        }
    }
    #endregion
}
