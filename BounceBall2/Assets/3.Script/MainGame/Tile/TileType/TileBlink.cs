using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlink : Tile_G
{
    private List<TileBlink> blinks;

    public void SetupBlinkTile(List<TileBlink> blink)
    {
        blinks = new List<TileBlink>();
        for(int i = 0; i < blink.Count; i++)
        {
            if(blink[i] != this)
            {
                blinks.Add(blink[i]);
            }
        }
    }

    public override void collsition(collisionDirection direction)
    {
        if(direction == collisionDirection.down)
        {
            int index = Random.Range(0, blinks.Count);
            movement2D.transform.position = blinks[index].transform.position + Vector3.up;
            movement2D.JumpTo();
        }
    }
}
