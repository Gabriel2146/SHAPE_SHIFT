using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibilityController : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player; // Referencia al jugador
    public float visionRadius = 10f; // Radio de visi�n del enemigo
    public LayerMask visibilityLayers; // Capas que bloquean la visi�n (obst�culos, terreno, etc.)

    private bool isIgnoringPlayer = false;

    private void Update()
    {
        // Verificar visibilidad del jugador en cada frame
        CheckPlayerVisibility();
    }

    private void CheckPlayerVisibility()
    {
        // Calcular direcci�n y distancia hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Si el jugador est� fuera del radio de visi�n, ignorarlo
        if (distanceToPlayer > visionRadius)
        {
            SetIgnorePlayer(true);
            return;
        }

        // Si hay una l�nea directa al jugador (sin obst�culos), el enemigo lo detecta
        if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, visibilityLayers))
        {
            SetIgnorePlayer(false);
        }
        else
        {
            // Si la visi�n est� bloqueada, el enemigo ignora al jugador
            SetIgnorePlayer(true);
        }
    }

    private void SetIgnorePlayer(bool state)
    {
        if (isIgnoringPlayer != state)
        {
            isIgnoringPlayer = state;

            if (isIgnoringPlayer)
            {
                Debug.Log($"{gameObject.name} est� ignorando al jugador.");
                // L�gica para ignorar al jugador (desactivar ataques, cambiar estado de patrulla, etc.)
            }
            else
            {
                Debug.Log($"{gameObject.name} ha detectado al jugador.");
                // L�gica para volver a detectar al jugador (activar ataques, cambiar a modo de persecuci�n, etc.)
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualizar el radio de visi�n en el editor
        Gizmos.color = isIgnoringPlayer ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
