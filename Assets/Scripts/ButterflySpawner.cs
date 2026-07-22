using UnityEngine;

public class ButterflySpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject butterflyPrefab;

    [Header("Spawn Area")]
    public float leftSpawnX = -8f;
    public float rightSpawnX = 8f;
    public float minY = -2f;
    public float maxY = 3f;

    [Header("Spawn Settings")]
    public float spawnInterval = 4f;

    [Header("Circular Movement")]
    public bool useCircularMovement = false;
    public float circleRadius = 1.6f;
    public float circleAngularSpeed = 2.2f;

    private float timer;

    private void Start()
    {
        timer = 0f;

        if (butterflyPrefab == null)
        {
            Debug.LogError(
                "Butterfly Prefabが設定されていません"
            );
        }
    }

    private void Update()
    {
        if (butterflyPrefab == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnButterfly();
            timer = 0f;
        }
    }

    private void SpawnButterfly()
    {
        float randomY = Random.Range(minY, maxY);

        bool spawnFromRight = Random.value > 0.5f;

        float spawnX;

        if (spawnFromRight)
        {
            spawnX = rightSpawnX;
        }
        else
        {
            spawnX = leftSpawnX;
        }

        Vector3 spawnPosition = new Vector3(
            spawnX,
            randomY,
            0f
        );

        GameObject butterflyObject = Instantiate(
            butterflyPrefab,
            spawnPosition,
            Quaternion.identity
        );

        Butterfly butterfly =
            butterflyObject.GetComponent<Butterfly>();

        if (butterfly == null)
        {
            Debug.LogError(
                "Butterfly PrefabにButterfly.csが付いていません"
            );

            Destroy(butterflyObject);
            return;
        }

        // 右から出たら左へ、左から出たら右へ進む
        butterfly.destroyX = Mathf.Max(
            butterfly.destroyX,
            Mathf.Max(Mathf.Abs(leftSpawnX), Mathf.Abs(rightSpawnX)) + circleRadius + 1f
        );

        butterfly.Initialize(
            !spawnFromRight,
            useCircularMovement,
            circleRadius,
            circleAngularSpeed
        );
    }
}
