using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Move_Speed = 5f;
    [SerializeField] private float Rotate_Speed = 180f;

    [SerializeField] private PlayerInput player_input;

    public Camera camera_;
    private Rigidbody player_rb;
    private Animator player_ani;
    private void Start()
    {
        player_rb = GetComponent<Rigidbody>();
        player_input = GetComponent<PlayerInput>();
        player_ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        player_ani.SetFloat("Move", player_input.Move_Value);
    }

    private void Move()
    {
        Vector3 moveDir = player_input.Move_Value * transform.forward * Move_Speed * Time.deltaTime;
        player_rb.MovePosition(player_rb.position + moveDir);
    }

    private void Rotate()
    {
        Vector3 lookDir = LookAtMouse();
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir);
            player_rb.rotation = Quaternion.Slerp(player_rb.rotation, targetRotation, Rotate_Speed * Time.deltaTime);
        }
    }

    private Vector3 LookAtMouse()
    {
        Ray ray = camera_.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;

        Debug.DrawRay(ray.origin, ray.origin + ray.direction * 100f, Color.red);
        if (Physics.Raycast(ray, out hitResult))
        {
            Vector3 mouseDir = new Vector3(hitResult.point.x, transform.position.y, hitResult.point.z) - transform.position;
            return mouseDir;
        }
        return Vector3.zero;
    }
}
