using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Controlador de físicas de plataformas para el Player.
/// Gestiona movimiento horizontal suave, salto con verificación de suelo,
/// animaciones del Animator y detección de estados de juego.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerPlatformerController : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de movimiento horizontal")]
    public float moveSpeed = 4.5f;

    [Tooltip("Fuerza de impulso para el salto")]
    public float jumpForce = 8.5f;

    [Header("Verificación de Suelo (Ground Check)")]
    [Tooltip("Capas consideradas suelo (Plataforma)")]
    public LayerMask groundLayer = ~0;

    [Header("Límites de Nivel")]
    [Tooltip("Límite horizontal mínimo (lado izquierdo de la plataforma)")]
    public float minX = -8f;

    [Tooltip("Límite horizontal máximo (lado derecho de la plataforma)")]
    public float maxX = 8f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput = 0f;
    private bool jumpRequested = false;
    private bool isGrounded = false;

    private const string PARAM_MOVIENDO = "Moviendo";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Si el Animator o SpriteRenderer están en el objeto o en un objeto hijo
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        // Si el juego ha terminado (victoria o derrota), detener inputs
        if (GameManagerColision.Instance != null && GameManagerColision.Instance.juegoTerminado)
        {
            horizontalInput = 0f;
            jumpRequested = false;
            if (animator != null) animator.SetBool(PARAM_MOVIENDO, false);
            return;
        }

        // 1. Lectura de Input Horizontal y Salto
        horizontalInput = 0f;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                horizontalInput += 1f;
            }
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                horizontalInput -= 1f;
            }

            if (Keyboard.current.upArrowKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpRequested = true;
            }
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            horizontalInput += 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            horizontalInput -= 1f;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
#endif

        // 2. Control de animación y orientación del sprite
        bool moving = Mathf.Abs(horizontalInput) > 0.01f;
        if (animator != null)
        {
            animator.SetBool(PARAM_MOVIENDO, moving);
        }

        if (moving)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = horizontalInput < 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // 3. Verificación de Suelo robusta usando los bounds del collider del jugador
        CheckGrounded();

        // 4. Movimiento Horizontal
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 5. Salto (solo si está tocando la plataforma)
        if (jumpRequested)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            jumpRequested = false;
        }

        // 6. Clamp horizontal dentro de los límites de la plataforma
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        if (Mathf.Abs(clampedX - transform.position.x) > 0.001f)
        {
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
        }
    }

    private void CheckGrounded()
    {
        if (playerCollider != null)
        {
            Bounds b = playerCollider.bounds;
            Vector2 checkCenter = new Vector2(b.center.x, b.min.y - 0.05f);
            Vector2 checkSize = new Vector2(b.size.x * 0.7f, 0.12f);

            Collider2D[] hits = Physics2D.OverlapBoxAll(checkCenter, checkSize, 0f, groundLayer);
            isGrounded = false;
            foreach (var h in hits)
            {
                if (h != null && !h.isTrigger && h.gameObject != gameObject && !h.transform.IsChildOf(transform))
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        else
        {
            // Fallback raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, groundLayer);
            isGrounded = hit.collider != null && !hit.collider.isTrigger && hit.collider.gameObject != gameObject;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerCollider != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Bounds b = playerCollider.bounds;
            Vector2 checkCenter = new Vector2(b.center.x, b.min.y - 0.05f);
            Vector2 checkSize = new Vector2(b.size.x * 0.7f, 0.12f);
            Gizmos.DrawWireCube(checkCenter, checkSize);
        }
    }
}