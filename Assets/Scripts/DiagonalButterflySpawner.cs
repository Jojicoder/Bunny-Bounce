using UnityEngine;

public class DiagonalButterflySpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject butterflyPrefab;

    [Header("Spawn Area")]
    public float leftSpawnX = -8f;
    public float rightSpawnX = 8f;
    public float minY = -3f;
    public float maxY = 2f;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;

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

        float spawnX = spawnFromRight ? rightSpawnX : leftSpawnX;

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

        DiagonalButterfly butterfly =
            butterflyObject.GetComponent<DiagonalButterfly>();

        if (butterfly == null)
        {
            Debug.LogError(
                "Butterfly PrefabにDiagonalButterfly.csが付いていません"
            );

            Destroy(butterflyObject);
            return;
        }

        // 右から出たら左へ、左から出たら右へ進む
        butterfly.moveRight = !spawnFromRight;

        // ランダムで上りか下りか
        butterfly.moveUp = Random.value > 0.5f;
    }
}
