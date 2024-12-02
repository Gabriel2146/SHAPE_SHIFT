using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibilityController : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player; // Referencia al jugador
    public float visionRadius = 10f; // Radio de visión del enemigo
    public LayerMask visibilityLayers; // Capas que bloquean la visión (obstáculos, terreno, etc.)

    private bool isIgnoringPlayer = false;

    private void Update()
    {
        // Verificar visibilidad del jugador en cada frame
        CheckPlayerVisibility();
    }

    private void CheckPlayerVisibility()
    {
        // Calcular dirección y distancia hacia el jugador
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Si el jugador está fuera del radio de visión, ignorarlo
        if (distanceToPlayer > visionRadius)
        {
            SetIgnorePlayer(true);
            return;
        }

        // Si hay una línea directa al jugador (sin obstáculos), el enemigo lo detecta
        if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, visibilityLayers))
        {
            SetIgnorePlayer(false);
        }
        else
        {
            // Si la visión está bloqueada, el enemigo ignora al jugador
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
                Debug.Log($"{gameObject.name} está ignorando al jugador.");
                // Lógica para ignorar al jugador (desactivar ataques, cambiar estado de patrulla, etc.)
            }
            else
            {
                Debug.Log($"{gameObject.name} ha detectado al jugador.");
                // Lógica para volver a detectar al jugador (activar ataques, cambiar a modo de persecución, etc.)
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Visualizar el radio de visión en el editor
        Gizmos.color = isIgnoringPlayer ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
