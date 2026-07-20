using UnityEngine;

public class Cake : MonoBehaviour
{
    public int scoreValue = 1;
    public float destroyY = -8f;

    public GameObject getTextPrefab;

    [Header("Sound")]
    public AudioClip collectSound;

    [Range(0f, 1f)]
    public float collectVolume = 0.3f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
        }

        if (getTextPrefab != null)
        {
            Instantiate(
                getTextPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(
                collectSound,
                Camera.main.transform.position,
                collectVolume
            );
        }

        Destroy(gameObject);
    }
}