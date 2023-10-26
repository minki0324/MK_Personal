using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilebase : Tile_G
{
    public override void collsition(collisionDirection direction)
    {
        if(direction == collisionDirection.down)
        {
            movement2D.JumpTo();
        }
    }
}
