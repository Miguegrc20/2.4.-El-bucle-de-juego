using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Administrador del flujo de juego en la escena Colision.
/// Controla la meta (Victoria), el impacto con el obstáculo (Derrota) y los paneles de feedback visual.
/// </summary>
public class GameManagerColision : MonoBehaviour
{
    public static GameManagerColision Instance { get; private set; }

    [Header("Referencias de UI")]
    [Tooltip("Panel de victoria que se activa al alcanzar la meta")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoVictoria;

    [Tooltip("Panel de derrota que se activa al chocar con el obstáculo triangular")]
    public GameObject panelDerrota;
    public TextMeshProUGUI textoDerrota;

    [Header("Estado de la partida")]
    public bool juegoTerminado = false;
    public bool victoria = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (panelDerrota != null) panelDerrota.SetActive(false);
    }

    /// <summary>
    /// Se invoca cuando el jugador alcanza el área de meta.
    /// </summary>
    public void CompletarNivel()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        victoria = true;
        Debug.Log("¡Nivel Completado! Has alcanzado la meta.");

        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
        }
    }

    /// <summary>
    /// Se invoca cuando el proyectil triangular impacta al jugador.
    /// </summary>
    public void Derrota()
    {
        if (juegoTerminado) return;

        juegoTerminado = true;
        victoria = false;
        Debug.Log("¡Game Over! El obstáculo triangular ha golpeado al jugador.");

        if (panelDerrota != null)
        {
            panelDerrota.SetActive(true);
        }
    }
}