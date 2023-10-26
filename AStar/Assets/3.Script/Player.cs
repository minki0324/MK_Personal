using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float Move_Speed = 0f;
    private Gamemanager gamemanager;
    public int currentIndex;

    private void Start()
    {
        gamemanager = FindObjectOfType<Gamemanager>();
    }

    public IEnumerator Move_co()
    {
        while (true)
        {
            if (gamemanager.Final_nodeList != null && gamemanager.Final_nodeList.Count > 0)
            {
                Vector3 Position = new Vector3(gamemanager.Final_nodeList[currentIndex].x, gamemanager.Final_nodeList[currentIndex].y, 0);
                
                while(Vector3.Magnitude(transform.position - Position) >= 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Position, Move_Speed * Time.deltaTime);
                    yield return null;
                }
                
                currentIndex++;
                
                if (currentIndex >= gamemanager.Final_nodeList.Count)
                {
                    currentIndex = 0;
                    gamemanager.Final_nodeList.Clear();
                    yield break;
                }
            }
            yield return null;
        }
    }
    public void startmove()
    {
        StopCoroutine("Move_co");
        StartCoroutine("Move_co");
    }
}
