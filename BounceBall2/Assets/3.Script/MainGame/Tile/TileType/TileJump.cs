using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileJump : Tile_G
{
    [SerializeField] private float Jumpforce;
    public override void collsition(collisionDirection direction)
    {
        if(direction == collisionDirection.down)
        {
            movement2D.JumpTo(Jumpforce);
        }
    }
}
