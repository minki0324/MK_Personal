using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    /*
     > 이 노드가 벽인지
     > 부모 노드
     > x, y 좌표값
     > f = h+g 
     > h = 추정값, 즉 가로 세로 장애물을 무시하고 목표까지의 거리
     > g = 시작으로부터 이동 했던 거리
    */

    public bool isWall;
    public Node Parentnode;
    public int x, y;
    public int G;
    public int H;
    public int F
    {
        get 
        {
            return G + H;
        }
    }

    public Node(bool iswall, int x, int y)
    {
        this.isWall = iswall;
        this.x = x;
        this.y = y;
    }
}

public class Gamemanager : MonoBehaviour
{
    public GameObject Start_pos, End_pos; // 시작, 끝지점
    public GameObject BottomLeft_ob, TopRight_ob; // 맵 크기

    [SerializeField] private Player ply;
    [SerializeField] private Vector2Int bottomLeft, topRight, start_pos, end_pos; // 조정할 위치에 필요한 변수

    public List<Node> Final_nodeList; // 최종 목적지점 까지 가는 노드를 담을 리스트

    // 대각선을 이용 할 것인지?
    public bool AllowDigonal = true;

    // 코너를 가로질러 가지 않을 경우 이동 중  수직 수평 장애물이 있는지 판단
    public bool DontCrossCorner = true;

    // 맵의 크기와 노드의 크기 개수
    private int SizeX, SizeY;
    Node[,] nodeArray;

    // 시작위치, 끝위치, 현재위치
    Node Startnode, Endnode, Curnode;

    // 알고리즘 찾아가면서 열린리스트, 닫힌리스트를 만들 리스트
    List<Node> OpenList, CloseList;


    public void SetPosition()
    {
        bottomLeft = new Vector2Int((int)BottomLeft_ob.transform.position.x, (int)BottomLeft_ob.transform.position.y);
        topRight = new Vector2Int((int)TopRight_ob.transform.position.x, (int)TopRight_ob.transform.position.y);
        start_pos = new Vector2Int((int)Start_pos.transform.position.x, (int)Start_pos.transform.position.y);
        end_pos = new Vector2Int((int)End_pos.transform.position.x, (int)End_pos.transform.position.y);
    }

    public void PathFinding()
    {
        SetPosition();
        SizeX = topRight.x - bottomLeft.x + 1;
        SizeY = topRight.y - bottomLeft.y + 1;

        // 노드를 담을 배열
        nodeArray = new Node[SizeX, SizeY];

        // 모든 노드들을 담는 과정
        for(int i = 0; i <SizeX; i++)
        {
            for(int j = 0; j < SizeY; j++)
            {
                bool iswall = false;
                //벽인지 아닌지 확인
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i+bottomLeft.x, j+bottomLeft.y), 0.4f))
                {
                    if(col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                    {
                        iswall = true;
                    }
                }
                // node를 담아줍니다.
                nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        //시작과 끝 노드, 열린 리스트, 닫힌 리스트, 최종경로 리스트 초기화 작업

        Startnode = nodeArray[start_pos.x - bottomLeft.x, start_pos.y - bottomLeft.y];
        Endnode = nodeArray[end_pos.x - bottomLeft.x, end_pos.y - bottomLeft.y];

        OpenList = new List<Node>();
        CloseList = new List<Node>();
        Final_nodeList = new List<Node>();

        OpenList.Add(Startnode);
        
        while(OpenList.Count > 0)
        {
            Curnode = OpenList[0];
            for(int i = 0; i < OpenList.Count; i++)
            {
                // 열린 리스트 중 가장 F가 작고 F가 같다면 H가 작은 것을 현재 노드로 설정
                if(OpenList[i].F <= Curnode.F && OpenList[i].H < Curnode.H)
                {
                    Curnode = OpenList[i];
                }

                // 열린 리스트에서 닫힌 리스트로 바꾸기
                
                OpenList.Remove(Curnode);
                CloseList.Add(Curnode);

                // Curnode가 도착지라면 예외처리
                if(Curnode == Endnode)
                {
                    Node Targetnode = Endnode;
                    while(Targetnode!=Startnode)
                    {
                        Final_nodeList.Add(Targetnode);
                        Targetnode = Targetnode.Parentnode;
                    }
                    Final_nodeList.Add(Startnode);

                    Final_nodeList.Reverse();
                    return;
                }
                // 대각선으로 움직이는 cost 계산
                if(AllowDigonal)
                {
                    // ↗ ↖ ↙ ↘ 순서대로
                    openListAdd(Curnode.x + 1, Curnode.y - 1);
                    openListAdd(Curnode.x - 1, Curnode.y + 1);
                    openListAdd(Curnode.x + 1, Curnode.y + 1);
                    openListAdd(Curnode.x - 1, Curnode.y - 1);
                }
                // 직선으로 움직이는 cost 계산
                // ↑ → ↓ ←
                    openListAdd(Curnode.x - 1, Curnode.y);
                    openListAdd(Curnode.x + 1, Curnode.y);
                    openListAdd(Curnode.x, Curnode.y + 1);
                    openListAdd(Curnode.x, Curnode.y - 1);
            }
        }
    }

    public void openListAdd(int CheckX, int CheckY)
    {
        /*
            조건
          > 상하좌우 범위를 벗어나지 않고, 벽도 아니면서, 닫힌리스트에 없어야 한다.
        */

        if (CheckX >= bottomLeft.x && CheckX < topRight.x + 1 && // x가 bottomleft와 topright안에 있고 
             CheckY >= bottomLeft.y && CheckY < topRight.y + 1 && // y가 bottomleft와 topright안에 있고
             !nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y].isWall && // 벽이 아니고
             !CloseList.Contains(nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y])) // closelist에 없다면
        {
            // 대각선 허용시 (벽 사이로는 통과가 되지 않음)
            if (AllowDigonal)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, CheckY - bottomLeft.y].isWall &&
                    nodeArray[CheckX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            // 코너를 가로질러 가지 않을 시 (이동 중 수직 수평 장애물이 있으면 안됨)
            if (DontCrossCorner)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, CheckY - bottomLeft.y].isWall ||
                   nodeArray[CheckX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }

            // check하는 노드를 이웃 노드에 넣고 직선은 10 대각선은 14로 계산
            Node neightNode = nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y];

            // 삼항 연산자 (조건을 두고 이거면 이거 저거면 저거 값을 주는 연산자 ?)
            int movecost = Curnode.G + (Curnode.x - CheckX == 0 || Curnode.y - CheckY == 0 ? 10 : 14);

            // 이동 비용이 이웃노드 G보다 작거나, 또는 열린 리스트에 이웃노드가 없다면 
            if (movecost < neightNode.G || !OpenList.Contains(neightNode))
            {
                // G H parentnode를 설정 후 열린 리스트에 추가
                neightNode.G = movecost;
                neightNode.H = (
                    Mathf.Abs(neightNode.x - Endnode.x) + Mathf.Abs(neightNode.y - Endnode.y)) * 10;

                neightNode.Parentnode = Curnode;
                
                OpenList.Add(neightNode);
            }
        }
    }

    

    private void OnDrawGizmos()
    {
        
        // 씬 뷰의 디버그 용도로 그림을 그릴때 사용
        if (Final_nodeList != null)
        {
            for(int i = 0; i < Final_nodeList.Count-1; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(new Vector2(Final_nodeList[i].x, Final_nodeList[i].y), new Vector2(Final_nodeList[i + 1].x, Final_nodeList[i + 1].y));
                
            }
        }
    }

   
}
