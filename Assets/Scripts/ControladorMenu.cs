using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Controlador principal de navegación entre escenas y gestión del menú.
/// Utiliza UnityEngine.SceneManagement para transiciones limpias y robustas.
/// </summary>
public class ControladorMenu : MonoBehaviour
{
    [Header("Nombres de Escenas del Proyecto")]
    [Tooltip("Nombre de la escena de Menú Principal")]
    public string escenaMenu = "SampleScene";

    [Tooltip("Nombre de la escena de Animación de Personaje")]
    public string escenaAnimacion = "Escena1";

    [Tooltip("Nombre de la escena de Colisiones y Proyectiles")]
    public string escenaColision = "Colision";

    [Header("Atajos de Teclado para Navegación")]
    [Tooltip("Permite cambiar de escena con las teclas numéricas 1, 2, M (Menú) y R (Reiniciar)")]
    public bool habilitarAtajosTeclado = true;

    void Update()
    {
        if (!habilitarAtajosTeclado) return;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            // Tecla 1: Escena 1 (Animación)
            if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
            {
                CargarEscena1();
            }
            // Tecla 2: Escena 2 (Colisión)
            else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
            {
                CargarColision();
            }
            // Tecla M o Escape: Menú Principal
            else if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                VolverAlMenu();
            }
            // Tecla R: Reiniciar escena actual
            else if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                ReiniciarEscena();
            }
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            CargarEscena1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            CargarColision();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            VolverAlMenu();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            ReiniciarEscena();
        }
#endif
    }

    /// <summary>
    /// Inicia el juego cargando la primera escena de juego (Escena1).
    /// Mantiene la firma original requerida por el botón Jugar existente.
    /// </summary>
    public void playGame()
    {
        CargarEscena(escenaAnimacion);
    }

    /// <summary>
    /// Carga la escena de animación de personaje (Escena1).
    /// </summary>
    public void CargarEscena1()
    {
        CargarEscena(escenaAnimacion);
    }

    /// <summary>
    /// Carga la escena del sistema de colisión y proyectil.
    /// </summary>
    public void CargarColision()
    {
        CargarEscena(escenaColision);
    }

    /// <summary>
    /// Regresa a la escena del menú principal (SampleScene).
    /// </summary>
    public void VolverAlMenu()
    {
        CargarEscena(escenaMenu);
    }

    /// <summary>
    /// Carga la siguiente escena en la lista de Build Settings de manera cíclica.
    /// </summary>
    public void CargarSiguienteEscena()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (totalScenes > 0)
        {
            int nextIndex = (currentIndex + 1) % totalScenes;
            SceneManager.LoadScene(nextIndex);
        }
    }

    /// <summary>
    /// Carga la escena anterior en la lista de Build Settings de manera cíclica.
    /// </summary>
    public void CargarEscenaAnterior()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (totalScenes > 0)
        {
            int prevIndex = (currentIndex - 1 + totalScenes) % totalScenes;
            SceneManager.LoadScene(prevIndex);
        }
    }

    /// <summary>
    /// Reinicia la escena actualmente activa.
    /// </summary>
    public void ReiniciarEscena()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(activeScene.buildIndex);
    }

    /// <summary>
    /// Carga una escena específica por su nombre.
    /// </summary>
    /// <param name="nombreEscena">Nombre de la escena a cargar</param>
    public void CargarEscena(string nombreEscena)
    {
        if (!string.IsNullOrEmpty(nombreEscena))
        {
            SceneManager.LoadScene(nombreEscena);
        }
        else
        {
            Debug.LogWarning("ControladorMenu: El nombre de la escena especificado está vacío.");
        }
    }

    /// <summary>
    /// Carga una escena específica por su índice en Build Settings.
    /// </summary>
    /// <param name="indiceEscena">Índice numérico de la escena</param>
    public void CargarEscenaPorIndice(int indiceEscena)
    {
        if (indiceEscena >= 0 && indiceEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(indiceEscena);
        }
        else
        {
            Debug.LogWarning($"ControladorMenu: Índice de escena {indiceEscena} fuera de rango.");
        }
    }

    /// <summary>
    /// Cierra la aplicación o detiene el modo Play en el editor.
    /// </summary>
    public void SalirJuego()
    {
        Debug.Log("Saliendo de la aplicación...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
