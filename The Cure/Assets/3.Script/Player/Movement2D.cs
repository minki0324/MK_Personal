using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // ĳ���� �̵� �ӵ�, ��ǥ
    public float Speed;
    private Vector3 vector;

    // �ȼ� ������ �����̱� ���� ���� Speed * Walkcount �ؼ� �ȼ����� ��ŭ �̵�
    // currentwalkcount�� 1�� �����ϸ鼭 ��ǥ�� ��ġ�� ������ while������ ��������
    public int walkcount;
    private int currentWalkcount;
    
    // �ڸ�ƾ�� �ߺ����� ���Ǵ� ���� �����ϱ� ����
    private bool canmove = true;

    // �ʹ� ������ �����̴°� �����ϱ� ���ؼ� �ڸ�ƾ ���
    IEnumerator Movement_co()
    {
        vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z); // ��ǥ���� �Է� ���������� ���� (z���� 0�����ص��ǰ� ������ �ص� ��)

        while (currentWalkcount < walkcount)
        {
            if (vector.x != 0)
            {
                transform.Translate(vector.x * Speed, 0, 0); // x ��ǥ�� �ٲ��ٸ� �̵�
            }
            else if (vector.y != 0)
            {
                transform.Translate(0, vector.y * Speed, 0); // y ��ǥ�� �ٲ��ٸ� �̵�
            }
            currentWalkcount++; // ���� walkcount�� ������ walkcount�� ������ �� ���� 1�� ���ؼ� �������� while�� false���� �Ǹ鼭 ���������� ��
        }
        currentWalkcount = 0; // �ʱ�ȭ

        yield return new WaitForSeconds(0.2f); // 1�ʵ��� ���

        canmove = true; // �ٽ� �ڸ�ƾ�� Ȱ��ȭ �ϱ� ���� Ʈ�簪���� ����
    }

    private void Update()
    {
        if (canmove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // ȭ��ǥ ������ �Է����� �� (horizontal, vertical�� �Է� ���⿡ ���� 1, -1�� ���ϵ�
            {
                canmove = false; // �ߺ����� ��� �Ǵ� ���� �����ϱ� ���� �޽������� ����
                StartCoroutine(Movement_co());
            }
        }
    }
}
