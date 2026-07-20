using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Destroy")]
    public float destroyX = 10f;

    [HideInInspector]
    public bool moveRight;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません");
        }

        UpdateFacingDirection();
    }

    private void Update()
    {
        MoveButterfly();
        CheckOutsideScreen();
    }

    private void MoveButterfly()
    {
        Vector2 direction;

        if (moveRight)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        transform.Translate(
            direction * moveSpeed * Time.deltaTime
        );
    }

    private void UpdateFacingDirection()
    {
        Vector3 scale = transform.localScale;

        if (moveRight)
        {
            scale.x = -Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }

        transform.localScale = scale;
    }

    private void CheckOutsideScreen()
    {
        if (moveRight && transform.position.x > destroyX)
        {
            Destroy(gameObject);
        }
        else if (!moveRight && transform.position.x < -destroyX)
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