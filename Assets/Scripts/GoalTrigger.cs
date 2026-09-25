using UnityEngine;

/// <summary>
/// Detector de área de meta al final de la plataforma.
/// Al entrar el jugador en contacto, notifica al GameManagerColision.
/// </summary>
public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManagerColision.Instance != null)
            {
                GameManagerColision.Instance.CompletarNivel();
            }
        }
    }
}