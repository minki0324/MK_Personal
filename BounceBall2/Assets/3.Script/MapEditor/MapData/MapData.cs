using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    객체 직렬화란
  > 객체의 상태를 메모리나 영구 저장장치에 저장 가능한 0,1로 순서를 바꾸는 것
  > 직렬화가 안되어 있다면 ? 기본 게이터 형식(int, float, ...)만 파일 입출력이 가능해진다.
  > 즉 클래스 구조체 같은 데이터 컨테이너 방식, 복합데이터 등 형식의 바일 입출력을 위해서 모든 변수를 저장하는 방법으로 정의하여야 한다.
*/
[System.Serializable] // 객체 직렬화

public class MapData
{
    // 맵 사이즈
    public Vector2Int Mapsize;
    // 타일 속성값
    public int[] Mapdata;
    // 플레이어 위치
    public Vector2Int PlayerPosition;

    
}
