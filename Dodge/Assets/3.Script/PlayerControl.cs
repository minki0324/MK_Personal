using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Rigidbody player_r;
    [SerializeField] private float Speed = 8f;

    private void Start()
    {
        player_r = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // addforce vs velocity
        /*
            addforce�� �������� ���� �����Ͽ� �ӵ��� ������Ű�� �̵�, ����, ����, ������ ������ �޴� �޼ҵ�
            velocity�� �ӵ��� ��Ÿ���ִ� ���������� ����, ������ �����ϰ� �־��� �ӵ��� �̵��Ѵ�.
        */

        /*if(Input.GetKey(KeyCode.W))
        {
            player_r.AddForce(0, 0, Speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player_r.AddForce(0, 0, -Speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player_r.AddForce(-Speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player_r.AddForce(Speed, 0, 0);
        }*/

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 value = new Vector3(x, 0f, z) * Speed;
        player_r.velocity = value;
    }

    public void Die()
    {
        gameObject.SetActive(false);
        if(GameObject.FindObjectOfType<GameManager>().TryGetComponent(out GameManager gm))
        {
            gm.EndGame();
        }
    }
}
