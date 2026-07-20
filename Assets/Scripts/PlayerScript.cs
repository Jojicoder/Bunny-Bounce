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
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // 左右入力
        float horizontal = Input.GetAxisRaw("Horizontal");

        // 左右移動
        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed,
            rb.linearVelocity.y
        );

        // Playerの中心からColliderの下端より
        // 少し先までRayを飛ばす
        float rayDistance =
            playerCollider.bounds.extents.y + extraGroundDistance;

        RaycastHit2D hit = Physics2D.Raycast(
            playerCollider.bounds.center,
            Vector2.down,
            rayDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            // ジャンプ音
            if (jumpSound != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(
                    jumpSound,
                    Camera.main.transform.position,
                    jumpVolume
                );
            }
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