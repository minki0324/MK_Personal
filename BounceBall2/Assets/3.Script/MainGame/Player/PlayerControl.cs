using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Movement2D movement;
    [SerializeField] private TileMap2D tilemap2d;

    private float DeadLimitY;

    public void Setup(Vector2Int position, int mapsizeY)
    {
        movement = GetComponent<Movement2D>();
        transform.position = new Vector3(position.x, position.y, 0);

        DeadLimitY = -mapsizeY / 2;
    }

    private void Update()
    {
        if(transform.position.y <= DeadLimitY)
        {
            SceneLoader.LoadScene();
        }
        UpdateMove();
        UpdateCollition();
    }

    private void UpdateCollition()
    {
        if(movement.isCollisionChecker.Up)
         {
            CollitionToTile(collisionDirection.up);
         }
        else if(movement.isCollisionChecker.Down)
         {
            CollitionToTile(collisionDirection.down);
        }
    }

    private void CollitionToTile(collisionDirection direction)
    {
        Tile_G tile = movement.Hittransform.GetComponent<Tile_G>();
        if (tile != null)
        {
            tile.collsition(direction);
        }
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        movement.MoveTo(x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            tilemap2d.GetCoin(collision.gameObject);
            // Destroy(collision.gameObject);
        }
    }
}
