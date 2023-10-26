using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tile_Type
{
    Empty = 0,
    Base,
    Broken,
    Boom,
    Jump,
    Straight_Left,
    Straight_Right,
    Blink,
    Item_Coin = 10, // ������
    Player = 100 // �÷��̾�
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite[] TileImage; // �̹��� �迭
    [SerializeField] private Sprite[] ItemImage; // ������ �̹���
    [SerializeField] private Sprite Player; // �÷��̾� �̹���

    private Tile_Type type;
    private SpriteRenderer renderer;

    public void Setup(Tile_Type type)
    {
        renderer = GetComponent<SpriteRenderer>();
        this.type = type;
    }

    public Tile_Type Tiletype
    {
        get => type;
        set
        {
            type = value;
            //Ÿ�Կ� ���� �̹��� ����
            if((int)type<(int)Tile_Type.Item_Coin)
            {
                renderer.sprite = TileImage[(int)type];
            }
            else if((int)type<(int)Tile_Type.Player)
            {
                renderer.sprite = ItemImage[(int)type - (int)Tile_Type.Item_Coin];
            }
            else if((int)type==(int)Tile_Type.Player)
            {
                renderer.sprite = Player;
            }
        }
    }
}
