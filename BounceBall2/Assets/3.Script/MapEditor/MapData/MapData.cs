using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    ��ü ����ȭ��
  > ��ü�� ���¸� �޸𸮳� ���� ������ġ�� ���� ������ 0,1�� ������ �ٲٴ� ��
  > ����ȭ�� �ȵǾ� �ִٸ� ? �⺻ ������ ����(int, float, ...)�� ���� ������� ����������.
  > �� Ŭ���� ����ü ���� ������ �����̳� ���, ���յ����� �� ������ ���� ������� ���ؼ� ��� ������ �����ϴ� ������� �����Ͽ��� �Ѵ�.
*/
[System.Serializable] // ��ü ����ȭ

public class MapData
{
    // �� ������
    public Vector2Int Mapsize;
    // Ÿ�� �Ӽ���
    public int[] Mapdata;
    // �÷��̾� ��ġ
    public Vector2Int PlayerPosition;

    
}
