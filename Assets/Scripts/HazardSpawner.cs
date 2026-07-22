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

    [Header("Bouncing Butterflies (spawn extra butterflies that bounce off screen edges)")]
    public bool spawnBouncingButterflies = false;
    public GameObject bouncingButterflyPrefab;
    public float butterflySpawnInterval = 4f;
    public float butterflyMinY = -2f;
    public float butterflyMaxY = 3f;
    public float butterflyBounceMinX = -7f;
    public float butterflyBounceMaxX = 7f;
    public bool butterflyUseCircularMovement = false;
    public float butterflyCircleRadius = 1.6f;
    public float butterflyCircleAngularSpeed = 2.2f;

    [Header("Zigzag Butterflies (spawn extra butterflies that move up and down)")]
    public bool spawnZigzagButterflies = false;
    public GameObject zigzagButterflyPrefab;
    public float zigzagButterflySpawnInterval = 4f;
    public float zigzagButterflyMinY = -2f;
    public float zigzagButterflyMaxY = 3f;
    public float zigzagButterflyDestroyX = 10f;
    public float zigzagAmplitude = 1.5f;
    public float zigzagFrequency = 2f;

    private float timer;
    private float butterflyTimer;
    private float zigzagButterflyTimer;

    private void Update()
    {
        if (hazardPrefab != null)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                SpawnHazard();
                timer = 0f;
            }
        }

        if (spawnBouncingButterflies && bouncingButterflyPrefab != null)
        {
            butterflyTimer += Time.deltaTime;

            if (butterflyTimer >= butterflySpawnInterval)
            {
                SpawnBouncingButterfly();
                butterflyTimer = 0f;
            }
        }

        if (spawnZigzagButterflies && zigzagButterflyPrefab != null)
        {
            zigzagButterflyTimer += Time.deltaTime;

            if (zigzagButterflyTimer >= zigzagButterflySpawnInterval)
            {
                SpawnZigzagButterfly();
                zigzagButterflyTimer = 0f;
            }
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

    private void SpawnBouncingButterfly()
    {
        float randomY = Random.Range(butterflyMinY, butterflyMaxY);

        bool spawnFromRight = Random.value > 0.5f;

        float spawnX = spawnFromRight
            ? butterflyBounceMaxX
            : butterflyBounceMinX;

        Vector3 spawnPosition = new Vector3(
            spawnX,
            randomY,
            0f
        );

        GameObject butterflyObject = Instantiate(
            bouncingButterflyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Butterfly butterfly = butterflyObject.GetComponent<Butterfly>();

        if (butterfly == null)
        {
            Debug.LogError(
                "Bouncing Butterfly Prefab is missing Butterfly.cs"
            );

            Destroy(butterflyObject);
            return;
        }

        butterfly.InitializeBouncing(
            !spawnFromRight,
            butterflyBounceMinX,
            butterflyBounceMaxX,
            butterflyUseCircularMovement,
            butterflyCircleRadius,
            butterflyCircleAngularSpeed
        );
    }

    private void SpawnZigzagButterfly()
    {
        float randomY = Random.Range(zigzagButterflyMinY, zigzagButterflyMaxY);

        bool spawnFromRight = Random.value > 0.5f;

        float spawnX = spawnFromRight ? zigzagButterflyDestroyX : -zigzagButterflyDestroyX;

        Vector3 spawnPosition = new Vector3(
            spawnX,
            randomY,
            0f
        );

        GameObject butterflyObject = Instantiate(
            zigzagButterflyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Butterfly butterfly = butterflyObject.GetComponent<Butterfly>();

        if (butterfly == null)
        {
            Debug.LogError(
                "Zigzag Butterfly Prefab is missing Butterfly.cs"
            );

            Destroy(butterflyObject);
            return;
        }

        butterfly.InitializeZigzag(
            !spawnFromRight,
            zigzagAmplitude,
            zigzagFrequency
        );

        butterfly.destroyX = Mathf.Max(
            butterfly.destroyX,
            zigzagButterflyDestroyX
        );
    }
}
