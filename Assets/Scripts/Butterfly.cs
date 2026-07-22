using UnityEngine;

public class Butterfly : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Circular Movement")]
    public bool useCircularMovement = false;
    public float circleRadius = 1.6f;
    public float circleAngularSpeed = 2.2f;

    [Header("Destroy")]
    public float destroyX = 10f;
    public float destroyMargin = 1.5f;

    [Header("Bounce (bounce off screen edges)")]
    public bool bounceOffScreenEdges = false;
    public float bounceMinX = -7f;
    public float bounceMaxX = 7f;

    [Header("Zigzag (move up and down while flying)")]
    public bool zigzagMovement = false;
    public float zigzagAmplitude = 1.5f;
    public float zigzagFrequency = 2f;

    [HideInInspector]
    public bool moveRight;

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Vector3 circleCenter;
    private float circleAngle;
    private float zigzagBaseY;
    private float zigzagTime;
    private bool movementInitialized;
    private bool hasEnteredScreen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found");
        }

        if (!movementInitialized)
        {
            InitializeMovementState();
        }
    }

    public void Initialize(
        bool shouldMoveRight,
        bool shouldUseCircularMovement,
        float newCircleRadius,
        float newCircleAngularSpeed
    )
    {
        moveRight = shouldMoveRight;
        useCircularMovement = shouldUseCircularMovement;
        circleRadius = newCircleRadius;
        circleAngularSpeed = newCircleAngularSpeed;

        InitializeMovementState();
    }

    public void InitializeZigzag(
        bool shouldMoveRight,
        float newZigzagAmplitude,
        float newZigzagFrequency
    )
    {
        moveRight = shouldMoveRight;
        useCircularMovement = false;
        zigzagMovement = true;
        zigzagAmplitude = Mathf.Max(0.1f, newZigzagAmplitude);
        zigzagFrequency = Mathf.Max(0.1f, newZigzagFrequency);

        InitializeMovementState();
    }

    public void InitializeBouncing(
        bool shouldMoveRight,
        float newBounceMinX,
        float newBounceMaxX,
        bool shouldUseCircularMovement,
        float newCircleRadius,
        float newCircleAngularSpeed
    )
    {
        moveRight = shouldMoveRight;
        bounceOffScreenEdges = true;
        bounceMinX = newBounceMinX;
        bounceMaxX = newBounceMaxX;
        useCircularMovement = shouldUseCircularMovement;
        circleRadius = newCircleRadius;
        circleAngularSpeed = newCircleAngularSpeed;

        InitializeMovementState();
    }

    private void InitializeMovementState()
    {
        UpdateFacingDirection();

        if (useCircularMovement)
        {
            circleAngle = moveRight ? Mathf.PI : 0f;

            Vector3 initialCircleOffset = new Vector3(
                Mathf.Cos(circleAngle) * circleRadius,
                Mathf.Sin(circleAngle) * circleRadius,
                0f
            );

            circleCenter = transform.position - initialCircleOffset;
        }
        else
        {
            circleCenter = transform.position;
            circleAngle = moveRight ? Mathf.PI : 0f;
        }

        zigzagBaseY = transform.position.y;
        zigzagTime = 0f;
        hasEnteredScreen = false;
        movementInitialized = true;
    }

    private void Update()
    {
        MoveButterfly();

        if (bounceOffScreenEdges)
        {
            CheckBounceOffScreen();
        }
        else
        {
            CheckOutsideScreen();
        }
    }

    private void MoveButterfly()
    {
        if (useCircularMovement)
        {
            MoveButterflyInCircle();
            return;
        }

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

        if (zigzagMovement)
        {
            ApplyZigzagMovement();
        }
    }

    private void ApplyZigzagMovement()
    {
        zigzagTime += zigzagFrequency * Time.deltaTime;

        Vector3 position = transform.position;
        position.y = zigzagBaseY + Mathf.Sin(zigzagTime) * zigzagAmplitude;
        transform.position = position;
    }

    private void MoveButterflyInCircle()
    {
        float directionX = moveRight ? 1f : -1f;

        circleCenter += Vector3.right * directionX * moveSpeed * Time.deltaTime;
        circleAngle += circleAngularSpeed * Time.deltaTime;

        Vector3 circleOffset = new Vector3(
            Mathf.Cos(circleAngle) * circleRadius,
            Mathf.Sin(circleAngle) * circleRadius,
            0f
        );

        transform.position = circleCenter + circleOffset;
    }

    private void UpdateFacingDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x);
        transform.localScale = scale;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = moveRight;
        }
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
                    return;
                }
            }

            if (moveRight && checkX > rightDestroyBound)
            {
                Destroy(gameObject);
            }
            else if (!moveRight && checkX < leftDestroyBound)
            {
                Destroy(gameObject);
            }

            return;
        }

        float safeDestroyX = Mathf.Max(Mathf.Abs(destroyX), 10f);

        if (moveRight && checkX > safeDestroyX)
        {
            Destroy(gameObject);
        }
        else if (!moveRight && checkX < -safeDestroyX)
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

    private void CheckBounceOffScreen()
    {
        float checkX = useCircularMovement ? circleCenter.x : transform.position.x;

        if (checkX > bounceMaxX || checkX < bounceMinX)
        {
            moveRight = !moveRight;
            UpdateFacingDirection();

            if (useCircularMovement)
            {
                circleCenter.x = Mathf.Clamp(circleCenter.x, bounceMinX, bounceMaxX);
            }
            else
            {
                Vector3 position = transform.position;
                position.x = Mathf.Clamp(position.x, bounceMinX, bounceMaxX);
                transform.position = position;
            }
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
