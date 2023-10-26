using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileMap2D : MonoBehaviour
{
    [Header("Game�� ���� Check")]
    public bool isGame = false;

    [Header("Game�� ���")]
    [SerializeField] private GameObject[] Tile_Prefabs_G;
    [SerializeField] private GameObject Item_Prefabs_G;

    private int Maxcoin = 0;
    private int Currentcoin = 0;
    [SerializeField] private Stage_UI stage_UI;
    [SerializeField] private StageControl stageControl;
    [SerializeField] private List<TileBlink> blinkTiles;
    [SerializeField] private Movement2D movement;

    [Header("MapEditor�� ���")]                             // �ν�����â ���� ������ ��ɾ�
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

    #region �� ������ ��ũ��Ʈ
    // ��ư �̺�Ʈ�� ���� �Ŷ� �ۺ����� ���
    public void Generate_tilemap()
    {
        if(int.TryParse(input_Width.text, out int _width) && int.TryParse(input_Height.text, out int _height)) // �޾ƿ� ��Ʈ�� ���� ��Ʈ������ ��ȯ
        {
            width = _width;
            Height = _height;
        }
        // �� ����� ����for��
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
        clone.transform.SetParent(transform); // Ÿ�ϸ� ������Ʈ�� ��ӽ�Ű��

        Tile tile = clone.GetComponent<Tile>();
        tile.Setup(type);

        tilelist.Add(tile);
    }

    public MapData GetMapData()
    {
        for (int i = 0; i < tilelist.Count; i++) 
        {
            if(tilelist[i].Tiletype != Tile_Type.Player) // �÷��̾ �ƴ϶��
            {
                mapData.Mapdata[i] = (int)tilelist[i].Tiletype;
            }
            else // �÷��̾� ���
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

    #region ���� ���� ��ũ��Ʈ
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

                // �����Ǵ� Ÿ�ϸ� �߾��� 0, 0, 0�� ��ġ
                Vector3 position = new Vector3((-width * 0.5f + 0.5f) + x, (height * 0.5f - 0.5f) - y, 0);

                // ����ó��
                if (map.Mapdata[index] > (int)TileType_G.Empty && map.Mapdata[index] < (int)TileType_G.LastIndex)
                {
                    // Ÿ�� ����� �޼ҵ� �߰�
                    Spawn_Tile((TileType_G)map.Mapdata[index], position);
                }
                else if (map.Mapdata[index] >= (int)Item_Type.Coin) 
                {
                    // ������ ����� �޼ҵ� �߰�
                    Spawn_Item(position);
                }
            }
        }
        stage_UI.UpdateTextCoin(Currentcoin, Maxcoin);

        // TileBlink blink ��ü �ϳ��ϳ� �����ϴµ� �ε����� ���� �ϴ°��� �ƴ� �ڷᱸ���� ��ü���� �����ϴ� ���
        foreach (TileBlink blink in blinkTiles)
        {
            //blink Ÿ�ϵ����� ���� Ÿ�� �˷��ִ� �޼ҵ� ȣ��
            blink.SetupBlinkTile(blinkTiles);
        }
    }
    public void Spawn_Tile(TileType_G type, Vector3 position)
    {
        // ���߿� Ÿ�� Ÿ�Կ� ���� �͵��� �߰��� �Ǹ� ���� �Ǿ�� �� �κ� [�Ϸ�]
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
