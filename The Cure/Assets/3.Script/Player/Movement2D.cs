using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // 캐릭터 이동 속도, 좌표
    public float Speed;
    private Vector3 vector;

    // 픽셀 단위로 움직이기 위한 단위 Speed * Walkcount 해서 픽셀단위 만큼 이동
    // currentwalkcount가 1씩 증가하면서 목표한 위치가 됐을때 while문에서 빠져나감
    public int walkcount;
    private int currentWalkcount;
    
    // 코르틴이 중복으로 사용되는 것을 방지하기 위해
    private bool canmove = true;

    // 너무 빠르게 움직이는걸 방지하기 위해서 코르틴 사용
    IEnumerator Movement_co()
    {
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); // 좌표값을 입력 받은것으로 변경 (z축은 0으로해도되고 저렇게 해도 됨)

        while (currentWalkcount < walkcount)
        {
            if (vector.x != 0)
            {
                transform.Translate(vector.x * Speed, 0, 0); // x 좌표가 바꼈다면 이동
            }
            else if (vector.y != 0)
            {
                transform.Translate(0, vector.y * Speed, 0); // y 좌표가 바꼈다면 이동
            }
            currentWalkcount++; // 현재 walkcount가 설정한 walkcount와 같아질 때 까지 1씩 더해서 같아지면 while이 false값이 되면서 빠져나오게 됨
        }
        currentWalkcount = 0; // 초기화

        yield return new WaitForSeconds(0.2f); // 1초동안 대기

        canmove = true; // 다시 코르틴을 활성화 하기 위해 트루값으로 설정
    }

    private void Update()
    {
        if (canmove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // 화살표 방향을 입력했을 때 (horizontal, vertical은 입력 방향에 따라 1, -1로 리턴됨
            {
                canmove = false; // 중복으로 계산 되는 것을 방지하기 위해 펄스값으로 설정
                StartCoroutine(Movement_co());
            }
        }
    }
}
