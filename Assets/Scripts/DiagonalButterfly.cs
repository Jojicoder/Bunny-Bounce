using UnityEngine;

public class DiagonalButterfly : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Destroy Bounds")]
    public float destroyX = 10f;
    public float destroyYTop = 6f;
    public float destroyYBottom = -8f;

    [HideInInspector]
    public bool moveRight;

    [HideInInspector]
    public bool moveUp;

    private GameManager gameManager;
    private Vector2 direction;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManagerが見つかりません");
        }

        float xDir = moveRight ? 1f : -1f;
        float yDir = moveUp ? 1f : -1f;

        direction = new Vector2(xDir, yDir).normalized;

        UpdateFacingDirection();
    }

    private void Update()
    {
        MoveButterfly();
        CheckOutsideScreen();
    }

    private void MoveButterfly()
    {
        transform.Translate(
            direction * moveSpeed * Time.deltaTime,
            Space.World
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
        else if (transform.position.y > destroyYTop)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y < destroyYBottom)
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
