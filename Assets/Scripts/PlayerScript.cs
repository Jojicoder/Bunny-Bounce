using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float extraGroundDistance = 0.1f;

    [Header("Jump Sound")]
    public AudioClip jumpSound;

    [Range(0f, 1f)]
    public float jumpVolume = 0.5f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private float horizontalInput;
    private float mobileInput;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // PCのキーボード入力
        float keyboardInput = Input.GetAxisRaw("Horizontal");

        // キーボード入力がある場合はキーボードを優先
        if (keyboardInput != 0f)
        {
            horizontalInput = keyboardInput;
        }
        else
        {
            horizontalInput = mobileInput;
        }

        CheckGrounded();

        // PC用ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );
    }

    private void CheckGrounded()
    {
        float rayDistance =
            playerCollider.bounds.extents.y + extraGroundDistance;

        RaycastHit2D hit = Physics2D.Raycast(
            playerCollider.bounds.center,
            Vector2.down,
            rayDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;
    }

    // 左ボタンを押している間
    public void MoveLeft()
    {
        mobileInput = -1f;
    }

    // 右ボタンを押している間
    public void MoveRight()
    {
        mobileInput = 1f;
    }

    // 左右ボタンから指を離した/外れたとき
    public void StopMoving()
    {
        mobileInput = 0f;
    }

    // PC・スマホ共通ジャンプ
    public void Jump()
    {
        if (!isGrounded)
        {
            return;
        }

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        if (jumpSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(
                jumpSound,
                Camera.main.transform.position,
                jumpVolume
            );
        }
    }

    private void OnDrawGizmosSelected()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col == null)
        {
            return;
        }

        float rayDistance =
            col.bounds.extents.y + extraGroundDistance;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            col.bounds.center,
            col.bounds.center + Vector3.down * rayDistance
        );
    }
}
