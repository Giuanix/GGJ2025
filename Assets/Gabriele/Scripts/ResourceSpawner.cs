using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject[] objectsToSpawn;  // Array of objects to spawn
    public float minSpawnTime = 10f;     // Minimum spawn interval
    public float maxSpawnTime = 20f;     // Maximum spawn interval

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            SpawnRandomObject();
        }
    }

    private void SpawnRandomObject()
    {
        if (objectsToSpawn.Length == 0)
            return;

        GameObject randomObject = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        Instantiate(randomObject, transform.position, Quaternion.identity);
    }
}
