using UnityEngine;

public class PlayerFall : MonoBehaviour
{
    [Header("Fall Game Over")]
    public float gameOverY = -5f;

    private GameManager gameManager;
    private bool gameOverCalled;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません");
        }
    }

    private void Update()
    {
        if (
            !gameOverCalled &&
            transform.position.y < gameOverY
        )
        {
            gameOverCalled = true;

            if (gameManager != null)
            {
                gameManager.GameOver("GAME OVER!!");
            }
        }
    }
}