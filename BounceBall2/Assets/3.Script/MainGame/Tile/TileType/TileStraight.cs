using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStraight : Tile_G
{
    [SerializeField] private MoveType movetype;
    private BoxCollider2D collider;
    
    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    public override void collsition(collisionDirection direction)
    {
        // �÷��̾��� ��ġ�� ���� Ÿ�� ��ġ���� �̵����⿡ 1��ŭ ������
        Vector3 position = collider.bounds.center + Vector3.right * (int)movetype;

        // �÷��̾ ���� ������ �̵��� �� �ֵ��� �޼ҵ� ȣ��
        movement2D.SetupStraightMove(movetype, position);
    }
}
