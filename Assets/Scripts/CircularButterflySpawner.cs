using UnityEngine;

public class CircularButterflySpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject circularButterflyPrefab;

    [Header("Spawn Area")]
    public float leftSpawnX = -8f;
    public float rightSpawnX = 8f;
    public float minY = -2f;
    public float maxY = 3f;

    [Header("Spawn Settings")]
    public float spawnInterval = 4f;

    [Header("Circular Movement")]
    public float circleRadius = 1.6f;
    public float circleAngularSpeed = 2.2f;

    private float timer;

    private void Start()
    {
        timer = 0f;

        if (circularButterflyPrefab == null)
        {
            Debug.LogError("Circular Butterfly Prefab is not set");
        }
    }

    private void Update()
    {
        if (circularButterflyPrefab == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCircularButterfly();
            timer = 0f;
        }
    }

    private void SpawnCircularButterfly()
    {
        float randomY = Random.Range(minY, maxY);
        bool spawnFromRight = Random.value > 0.5f;
        float spawnX = spawnFromRight ? rightSpawnX : leftSpawnX;
        bool shouldMoveRight = !spawnFromRight;

        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);
        GameObject butterflyObject = Instantiate(
            circularButterflyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        CircularButterfly butterfly = butterflyObject.GetComponent<CircularButterfly>();

        if (butterfly == null)
        {
            Debug.LogError("Circular Butterfly Prefab is missing CircularButterfly.cs");
            Destroy(butterflyObject);
            return;
        }

        butterfly.destroyX = Mathf.Max(
            butterfly.destroyX,
            MaxSpawnDistance() + circleRadius
        );

        butterfly.InitializeCircular(
            shouldMoveRight,
            circleRadius,
            circleAngularSpeed
        );
    }

    private float MaxSpawnDistance()
    {
        return Mathf.Max(Mathf.Abs(leftSpawnX), Mathf.Abs(rightSpawnX)) + 1f;
    }
}
