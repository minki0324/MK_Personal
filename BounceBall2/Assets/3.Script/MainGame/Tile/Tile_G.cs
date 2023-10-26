using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType_G
{
    Empty = 0,
    Base,
    Broken,
    Boom,
    Jump,
    Straight_Left,
    Straight_Right,
    Blink,
    LastIndex
}

public enum collisionDirection { up=1, down}

/*

public class Tile_G : MonoBehaviour
{
    [SerializeField] private Sprite[] Images;
    private SpriteRenderer renderer;
    private TileType_G tileType;

    public TileType_G tiletype
    {
        get => tileType;
        set
        {
            tileType = value;
            renderer.sprite = Images[(int)tileType - 1];
        }
    }

    public void SetUp (TileType_G tile)
    {
        renderer = GetComponent<SpriteRenderer>();
        tileType = tile;
    }

}
*/

public abstract class Tile_G : MonoBehaviour
{
    protected Movement2D movement2D;

    public virtual void Setup(Movement2D movement) // 구현을 선택해서
    {
        movement2D = movement;
    }

    public abstract void collsition(collisionDirection direction); // 무조건 구현
    

}
