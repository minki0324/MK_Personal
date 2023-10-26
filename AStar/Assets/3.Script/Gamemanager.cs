using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    /*
     > �� ��尡 ������
     > �θ� ���
     > x, y ��ǥ��
     > f = h+g 
     > h = ������, �� ���� ���� ��ֹ��� �����ϰ� ��ǥ������ �Ÿ�
     > g = �������κ��� �̵� �ߴ� �Ÿ�
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
    public GameObject Start_pos, End_pos; // ����, ������
    public GameObject BottomLeft_ob, TopRight_ob; // �� ũ��

    [SerializeField] private Player ply;
    [SerializeField] private Vector2Int bottomLeft, topRight, start_pos, end_pos; // ������ ��ġ�� �ʿ��� ����

    public List<Node> Final_nodeList; // ���� �������� ���� ���� ��带 ���� ����Ʈ

    // �밢���� �̿� �� ������?
    public bool AllowDigonal = true;

    // �ڳʸ� �������� ���� ���� ��� �̵� ��  ���� ���� ��ֹ��� �ִ��� �Ǵ�
    public bool DontCrossCorner = true;

    // ���� ũ��� ����� ũ�� ����
    private int SizeX, SizeY;
    Node[,] nodeArray;

    // ������ġ, ����ġ, ������ġ
    Node Startnode, Endnode, Curnode;

    // �˰��� ã�ư��鼭 ��������Ʈ, ��������Ʈ�� ���� ����Ʈ
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

        // ��带 ���� �迭
        nodeArray = new Node[SizeX, SizeY];

        // ��� ������ ��� ����
        for(int i = 0; i <SizeX; i++)
        {
            for(int j = 0; j < SizeY; j++)
            {
                bool iswall = false;
                //������ �ƴ��� Ȯ��
                foreach (Collider2D col in Physics2D.OverlapCircleAll(new Vector2(i+bottomLeft.x, j+bottomLeft.y), 0.4f))
                {
                    if(col.gameObject.layer.Equals(LayerMask.NameToLayer("Wall")))
                    {
                        iswall = true;
                    }
                }
                // node�� ����ݴϴ�.
                nodeArray[i, j] = new Node(iswall, i + bottomLeft.x, j + bottomLeft.y);
            }
        }

        //���۰� �� ���, ���� ����Ʈ, ���� ����Ʈ, ������� ����Ʈ �ʱ�ȭ �۾�

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
                // ���� ����Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� ���� ���� ���� ����
                if(OpenList[i].F <= Curnode.F && OpenList[i].H < Curnode.H)
                {
                    Curnode = OpenList[i];
                }

                // ���� ����Ʈ���� ���� ����Ʈ�� �ٲٱ�
                
                OpenList.Remove(Curnode);
                CloseList.Add(Curnode);

                // Curnode�� ��������� ����ó��
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
                // �밢������ �����̴� cost ���
                if(AllowDigonal)
                {
                    // �� �� �� �� �������
                    openListAdd(Curnode.x + 1, Curnode.y - 1);
                    openListAdd(Curnode.x - 1, Curnode.y + 1);
                    openListAdd(Curnode.x + 1, Curnode.y + 1);
                    openListAdd(Curnode.x - 1, Curnode.y - 1);
                }
                // �������� �����̴� cost ���
                // �� �� �� ��
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
            ����
          > �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ����� �Ѵ�.
        */

        if (CheckX >= bottomLeft.x && CheckX < topRight.x + 1 && // x�� bottomleft�� topright�ȿ� �ְ� 
             CheckY >= bottomLeft.y && CheckY < topRight.y + 1 && // y�� bottomleft�� topright�ȿ� �ְ�
             !nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y].isWall && // ���� �ƴϰ�
             !CloseList.Contains(nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y])) // closelist�� ���ٸ�
        {
            // �밢�� ���� (�� ���̷δ� ����� ���� ����)
            if (AllowDigonal)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, CheckY - bottomLeft.y].isWall &&
                    nodeArray[CheckX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }
            // �ڳʸ� �������� ���� ���� �� (�̵� �� ���� ���� ��ֹ��� ������ �ȵ�)
            if (DontCrossCorner)
            {
                if (nodeArray[Curnode.x - bottomLeft.x, CheckY - bottomLeft.y].isWall ||
                   nodeArray[CheckX - bottomLeft.x, Curnode.y - bottomLeft.y].isWall)
                {
                    return;
                }
            }

            // check�ϴ� ��带 �̿� ��忡 �ְ� ������ 10 �밢���� 14�� ���
            Node neightNode = nodeArray[CheckX - bottomLeft.x, CheckY - bottomLeft.y];

            // ���� ������ (������ �ΰ� �̰Ÿ� �̰� ���Ÿ� ���� ���� �ִ� ������ ?)
            int movecost = Curnode.G + (Curnode.x - CheckX == 0 || Curnode.y - CheckY == 0 ? 10 : 14);

            // �̵� ����� �̿���� G���� �۰ų�, �Ǵ� ���� ����Ʈ�� �̿���尡 ���ٸ� 
            if (movecost < neightNode.G || !OpenList.Contains(neightNode))
            {
                // G H parentnode�� ���� �� ���� ����Ʈ�� �߰�
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
        
        // �� ���� ����� �뵵�� �׸��� �׸��� ���
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
