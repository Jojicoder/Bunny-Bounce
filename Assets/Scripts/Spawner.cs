using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Food Prefabs")]
    public GameObject cakePrefab;
    public GameObject donutPrefab;
    public GameObject pizzaPrefab;
    public GameObject hamburgerPrefab;

    [Header("Spawn Area")]
    public float minX = -7f;
    public float maxX = 7f;
    public float spawnY = 5f;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnFood();
            timer = 0f;
        }
    }

    private void SpawnFood()
    {
        float randomX = Random.Range(minX, maxX);

        Vector3 spawnPosition = new Vector3(
            randomX,
            spawnY,
            0f
        );

        float random = Random.value;

        GameObject foodPrefab;

        if (random < 0.35f)
        {
            // 35%
            foodPrefab = cakePrefab;
        }
        else if (random < 0.7f)
        {
            // 35%
            foodPrefab = donutPrefab;
        }
        else if (random < 0.9f)
        {
            // 20%
            foodPrefab = pizzaPrefab;
        }
        else
        {
            // 10%
            foodPrefab = hamburgerPrefab;
        }

        Instantiate(
            foodPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}