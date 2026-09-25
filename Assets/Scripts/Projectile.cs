using UnityEngine;

/// <summary>
/// Controla el obstáculo triangular en la escena de juego.
/// Se desplaza horizontalmente hacia el jugador a una velocidad controlada y visible,
/// permitiendo al jugador saltar por encima para esquivarlo.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Configuración de Velocidad")]
    [Tooltip("Velocidad de avance horizontal del proyectil/obstáculo")]
    public float speed = 2.2f;

    [Tooltip("Dirección de avance (-1 para izquierda, 1 para derecha)")]
    public float direccionX = -1f;

    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void FixedUpdate()
    {
        // Si la partida terminó (victoria o derrota), detener el movimiento
        if (GameManagerColision.Instance != null && GameManagerColision.Instance.juegoTerminado)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        // Movimiento horizontal continuo y predecible hacia la izquierda (hacia el jugador)
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direccionX * speed, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡El proyectil triangular golpeó al Player!");

            if (GameManagerColision.Instance != null)
            {
                GameManagerColision.Instance.Derrota();
            }

            Destroy(gameObject);
        }
    }
}