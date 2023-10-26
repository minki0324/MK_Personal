using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spwan : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private float Spawn_Cool = 0.7f;

    private IEnumerable Spawn_co()
    {
        int randomX = Random.Range(0, 7);
        Vector3 SpawnPosition = new Vector3(randomX, 0, 0);

        while(true)
        {
            Instantiate(Enemy, SpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Spawn_Cool);
        }
    }

    public void StartSpawn()
    {
        StartCoroutine("Spawn_co");
    }
    public void StopSpawn()
    {
        StopCoroutine("Spawn_co");
    }
}
