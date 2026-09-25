using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Controlador de animación y movimiento horizontal del personaje.
/// Maneja la transición entre los estados Idle y Moviendo en el Animator.
/// Soporta tanto el Nuevo Sistema de Input como el clásico.
/// </summary>
public class ControladorAnimacion : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de desplazamiento horizontal del personaje")]
    [SerializeField] private float velocidadMovimiento = 4f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string PARAM_MOVIENDO = "Moviendo";

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float inputHorizontal = 0f;

        // Lectura de entrada utilizando el Nuevo Sistema de Input (New Input System)
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                inputHorizontal += 1f;
            }
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            {
                inputHorizontal -= 1f;
            }
        }
#endif

        // Soporte de compatibilidad con Input Manager tradicional
#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            inputHorizontal += 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            inputHorizontal -= 1f;
        }
#endif

        bool estaMoviendo = Mathf.Abs(inputHorizontal) > 0.01f;

        // Actualizar parámetro en el Animator para transicionar entre Idle y Moviendo
        if (animator != null)
        {
            animator.SetBool(PARAM_MOVIENDO, estaMoviendo);
        }

        // Aplicar movimiento horizontal y orientación del sprite
        if (estaMoviendo)
        {
            transform.Translate(Vector3.right * (inputHorizontal * velocidadMovimiento * Time.deltaTime));

            if (spriteRenderer != null)
            {
                // Voltear sprite según la dirección hacia la que se mueve
                spriteRenderer.flipX = inputHorizontal < 0f;
            }
        }
    }
}