using UnityEngine;

public class DiagonalButterfly : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Destroy Bounds")]
    public float destroyX = 10f;
    public float destroyYTop = 6f;
    public float destroyYBottom = -8f;
    public float destroyMargin = 1.5f;

    [HideInInspector]
    public bool moveRight;

    [HideInInspector]
    public bool moveUp;

    private GameManager gameManager;
    private Vector2 direction;
    private bool hasEnteredScreen;

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
        float checkX = transform.position.x;

        if (TryGetCameraHorizontalBounds(out float leftBound, out float rightBound))
        {
            float safeDestroyMargin = Mathf.Max(destroyMargin, 1.5f);
            float leftDestroyBound = leftBound - safeDestroyMargin;
            float rightDestroyBound = rightBound + safeDestroyMargin;

            if (!hasEnteredScreen)
            {
                hasEnteredScreen =
                    checkX >= leftDestroyBound &&
                    checkX <= rightDestroyBound;

                if (!hasEnteredScreen)
                {
                    CheckOutsideVerticalBounds();
                    return;
                }
            }

            if (moveRight && checkX > rightDestroyBound)
            {
                Destroy(gameObject);
                return;
            }

            if (!moveRight && checkX < leftDestroyBound)
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            float safeDestroyX = Mathf.Max(Mathf.Abs(destroyX), 10f);

            if (moveRight && checkX > safeDestroyX)
            {
                Destroy(gameObject);
                return;
            }

            if (!moveRight && checkX < -safeDestroyX)
            {
                Destroy(gameObject);
                return;
            }
        }

        CheckOutsideVerticalBounds();
    }

    private void CheckOutsideVerticalBounds()
    {
        if (transform.position.y > destroyYTop)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y < destroyYBottom)
        {
            Destroy(gameObject);
        }
    }

    private bool TryGetCameraHorizontalBounds(out float leftBound, out float rightBound)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            leftBound = 0f;
            rightBound = 0f;
            return false;
        }

        float cameraDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 leftWorld = mainCamera.ViewportToWorldPoint(
            new Vector3(0f, 0.5f, cameraDistance)
        );
        Vector3 rightWorld = mainCamera.ViewportToWorldPoint(
            new Vector3(1f, 0.5f, cameraDistance)
        );

        leftBound = Mathf.Min(leftWorld.x, rightWorld.x);
        rightBound = Mathf.Max(leftWorld.x, rightWorld.x);
        return true;
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
