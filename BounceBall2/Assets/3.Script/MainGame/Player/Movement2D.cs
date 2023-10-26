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

    // 레이캐스트 카운트에 따른 일정한 간격
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

    // ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

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

    // 점프
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

    // 공 브레이크
    public void MoveTo(float x)
    {
        // 왼쪽, 오른쪽 이동 상태일 때 좌우 방향키를 누른다면?
        if (x != 0 && moveType != MoveType.Updown)
        {
            moveType = MoveType.Updown;
        }
        velocity.x = x * Movespeed;
    }

    // straight 상태 Move
    public void SetupStraightMove(MoveType type, Vector3 position)
    {
        moveType = type;
        transform.position = position;
        velocity.y = 0;
    }

    // 스페이싱(간격) 계산
    private void CalculateRayCastSpacing()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(SkinWidth * -2);

        Horizontal_Spacing = bounds.size.y / (Horizontal_count - 1);
        Vertical_Spacing = bounds.size.x / (Vertical_count - 1);
    }

    // collider cornor 위치 갱신 메소드
    private void Update_colliderCorner()
    {
        Bounds bounds = collider2D.bounds;
        bounds.Expand(SkinWidth * -2);

        colliderCorner.Topleft = new Vector2(bounds.min.x, bounds.max.y);
        colliderCorner.Bottomleft = new Vector2(bounds.min.x, bounds.min.y);
        colliderCorner.Bottomright = new Vector2(bounds.max.x, bounds.min.y);
    }

    // 실제 플레이어의 움직임
    private void UpdateMovement()
    {
        if(moveType.Equals(MoveType.Updown))
        {
            //y축으로 움직입니다.
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.x = (int)moveType * Movespeed;
        }

        Vector3 currentVelocity = velocity * Time.deltaTime;

        // 좌 우로 움직일때
        if(currentVelocity.x != 0)
        {
            // RayCast 쏘는거 만들기
            RayCastHorizontal(ref currentVelocity);
        }
        if(currentVelocity.y != 0)
        {
            // RayCast 쏘는거 만들기
            RayCastVertical(ref currentVelocity);
        }
        transform.position += currentVelocity;
    }

    private void RayCastHorizontal(ref Vector3 velocity)
    {
        /*
            ref란 
         내부 메소드에서 적용된 값을 변경할 때 사용 (C++의 포인터랑 비슷한 개념)
        */

        // Mathf.Sign - 음수인지 양수인지 확인하는 메소드
        float direction = Mathf.Sign(velocity.x); // 이동 방향 오 : 1, 왼 : -1
        float distance = Mathf.Abs(velocity.x) + SkinWidth; // 광선의 길이
        Vector2 rayPosition = Vector2.zero;
        RaycastHit2D hit;

        for(int i = 0; i < Horizontal_count; ++i)
        {
            rayPosition = (direction == 1) ? colliderCorner.Bottomright : colliderCorner.Bottomleft;
            rayPosition += Vector2.up * (Horizontal_Spacing * i);

            hit = Physics2D.Raycast(rayPosition, Vector2.right * direction, distance, CollisionLayer);
            if(hit) // hit이 null값이냐 아니냐
            {
                // x축 속력을 광선과 오브젝트 사이의 거리로 설정 (거리가 0이면 속력도 0)
                velocity.x = (hit.distance * SkinWidth) * direction;

                // 다음에 발사되는 광선의 거리 설정
                distance = hit.distance;

                // 현재 진행방향, 부딪힌 방향 정보를 true로 변경
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
