using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [Header("Hazard Prefab")]
    public GameObject hazardPrefab;

    [Header("Spawn Area")]
    public float minX = -7f;
    public float maxX = 7f;
    public float minSpawnY = 4f;
    public float maxSpawnY = 6f;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;

    private float timer;

    private void Update()
    {
        if (hazardPrefab == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnHazard();
            timer = 0f;
        }
    }

    private void SpawnHazard()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minSpawnY, maxSpawnY);

        Vector3 spawnPosition = new Vector3(
            randomX,
            randomY,
            0f
        );

        Instantiate(
            hazardPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}
