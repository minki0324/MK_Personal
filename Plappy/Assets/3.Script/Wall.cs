using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Transform upper;
    [SerializeField] private Transform bottom;

    [SerializeField] private Rigidbody rb;

    private float Move_Speed = 3f;
    private float hole;
    private int level;
    public float GameTime = 0f;
    private float height = 15f;
    public float deactivateDelay = 14f;

    private void Update()
    {
        GameTime += Time.deltaTime;

        level = Mathf.FloorToInt(GameTime / 10f);
        Vector3 value = new Vector3(1, 0, 0) * Move_Speed;
        gameObject.transform.position += value * Time.deltaTime;

        // 14초 뒤에 비활성화 로직 구성하기
    }

    private void OnEnable()
    {
        hole = Random.Range(2f-(level*0.1f), 4f-(level*0.1f));
        float upscale = Random.Range(1, height - hole - 1);
        float botscale = height - hole - upscale;

        upper.localScale = new Vector3(1, upscale, 1);
        upper.localPosition = new Vector3(0, upscale / 2 + hole + botscale, 0);

        bottom.localScale = new Vector3(1, botscale, 1);
        bottom.localPosition = new Vector3(0, botscale / 2, 0);
        StartCoroutine(Clear());
    }

    private IEnumerator Clear()
    {
        yield return new WaitForSeconds(deactivateDelay);
        gameObject.SetActive(false);
    }
}
