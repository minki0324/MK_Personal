using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ColliderCorner
{
    public Vector2 Topleft;
    public Vector2 Bottomleft;
    public Vector2 Bottomright;
}

public struct ColliderChecker
{
    public bool Up;
    public bool Down;
    public bool Left;
    public bool Right;

    public void Reset()
    {
        Up = false;
        Down = false;
        Left = false;
        Right = false;
    }
}

public enum MoveType 
{
    Left = -1,
    Updown = 0,
    Right = 1
}

public class Movement2D : MonoBehaviour
{
    [Header("Raycast Collision")]
    [SerializeField] private LayerMask CollisionLayer;

    [Header("Raycast Count")]
    [SerializeField] private int Horizontal_count = 4;
    [SerializeField] private int Vertical_count = 4;

    // ����ĳ��Ʈ ī��Ʈ�� ���� ������ ����
    private float Horizontal_Spacing; 
    private float Vertical_Spacing;

    [Header("Raycast Count")]
    [SerializeField] private float Movespeed;
    [SerializeField] private float JumpForce = 10;
    private float gravity = -20.0f;

    private Vector3 velocity;
    private readonly float SkinWidth = 0.015f;

    private Collider2D collider2D;
    private ColliderCorner colliderCorner;
    private ColliderChecker colliderChecker;

    public MoveType moveType { get; private set; }

    // �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    public ColliderChecker isCollisionChecker => colliderChecker;
    public Transform Hittransform { get; private set; }

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        moveType = MoveType.Updown;
    }

    private void Update()
    {
        CalculateRayCastSpacing();
        Update_colliderCorner();
        colliderChecker.Reset();
        UpdateMovement();
        if(colliderChecker.Up || colliderChecker.Down)
        {
            velocity.y = 0;
        }
        // JumpTo();
    }

    // ����
    public void JumpTo(float jumpforce = 0)
    {
         if (jumpforce != 0)
         {
             velocity.y = jumpforce;
             return;
         }
        if(colliderChecker.Down)
        {
            velocity.y = this.JumpForce;
        }
    }

    // �� �극��ũ
    public void MoveTo(float x)
    {
        // ����, ������ �̵� ������ �� �¿� ����Ű�� �����ٸ�?
        if (x != 0 && moveType != MoveType.Updown)
        {
            moveType = MoveType.Updown;
        }
        velocity.x = x * Movespeed;
    }

    // straight ���� Move
    public void SetupStraightMove(MoveType type, Vector3 position)
    {
        moveType = type;
        transform.position = position;
        velocity.y = 0;
    }

    // �����̽�(����) ���
    private void CalculateRayCastSpacing()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(SkinWidth * -2);

        Horizontal_Spacing = bounds.size.y / (Horizontal_count - 1);
        Vertical_Spacing = bounds.size.x / (Vertical_count - 1);
    }

    // collider cornor ��ġ ���� �޼ҵ�
    private void Update_colliderCorner()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(SkinWidth * -2);

        colliderCorner.Topleft = new Vector2(bounds.min.x, bounds.max.y);
        colliderCorner.Bottomleft = new Vector2(bounds.min.x, bounds.min.y);
        colliderCorner.Bottomright = new Vector2(bounds.max.x, bounds.min.y);
    }

    // ���� �÷��̾��� ������
    private void UpdateMovement()
    {
        if(moveType.Equals(MoveType.Updown))
        {
            //y������ �����Դϴ�.
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.x = (int)moveType * Movespeed;
        }

        Vector3 currentVelocity = velocity * Time.deltaTime;

        // �� ��� �����϶�
        if(currentVelocity.x != 0)
        {
            // RayCast ��°� �����
            RayCastHorizontal(ref currentVelocity);
        }
        if(currentVelocity.y != 0)
        {
            // RayCast ��°� �����
            RayCastVertical(ref currentVelocity);
        }
        transform.position += currentVelocity;
    }

    private void RayCastHorizontal(ref Vector3 velocity)
    {
        /*
            ref�� 
         ���� �޼ҵ忡�� ����� ���� ������ �� ��� (C++�� �����Ͷ� ����� ����)
        */

        // Mathf.Sign - �������� ������� Ȯ���ϴ� �޼ҵ�
        float direction = Mathf.Sign(velocity.x); // �̵� ���� �� : 1, �� : -1
        float distance = Mathf.Abs(velocity.x) + SkinWidth; // ������ ����
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;

        for(int i = 0; i < Horizontal_count; ++i)
        {
            rayPosition = (direction == 1) ? colliderCorner.Bottomright : colliderCorner.Bottomleft;
            rayPosition += Vector2.up * (Horizontal_Spacing * i);

            hit = Physics2D.Raycast(rayPosition, Vector2.right * direction, distance, CollisionLayer);
            if(hit) // hit�� null���̳� �ƴϳ�
            {
                // x�� �ӷ��� ������ ������Ʈ ������ �Ÿ��� ���� (�Ÿ��� 0�̸� �ӷµ� 0)
                velocity.x = (hit.distance * SkinWidth) * direction;

                // ������ �߻�Ǵ� ������ �Ÿ� ����
                distance = hit.distance;

                // ���� �������, �ε��� ���� ������ true�� ����
                colliderChecker.Left = (direction == -1);
                colliderChecker.Right = (direction == 1);
            }
            Debug.DrawRay(rayPosition, rayPosition + Vector2.right * direction * distance, Color.blue);
        }
    }
    private void RayCastVertical(ref Vector3 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float distance = Mathf.Abs(velocity.y) + SkinWidth;
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;

        for (int i = 0; i < Vertical_count; ++i)
        {
            rayPosition = (direction == 1) ? colliderCorner.Topleft : colliderCorner.Bottomleft;
            rayPosition += Vector2.right * (Vertical_Spacing * i + velocity.x);
            hit = Physics2D.Raycast(rayPosition, Vector2.up * direction, distance, CollisionLayer);
            if(hit)
            {
                velocity.y = (hit.distance - SkinWidth) * direction;
                distance = hit.distance;
                colliderChecker.Down = (direction == -1);
                colliderChecker.Up = (direction == 1);
                Hittransform = hit.transform;
            }
            Debug.DrawRay(rayPosition, rayPosition * Vector2.up * direction * distance, Color.yellow);
        }
    }

}
