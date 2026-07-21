using UnityEngine;

public class FallingHazard : MonoBehaviour
{
    public float destroyY = -8f;

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
            gameManager.GameOver("GAME OVER!!");
        }

        Destroy(gameObject);
    }
}
