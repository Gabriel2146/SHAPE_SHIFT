using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransformation : MonoBehaviour
{
    [Header("Transform Settings")]
    public GameObject[] transformableObjects; // Lista de prefabs para transformarse
    public float transformationDuration = 10f; // Tiempo l�mite en la forma transformada

    [Header("Enemy Detection")]
    public LayerMask enemyLayer; // Capa de los enemigos
    public float detectionDisableRadius = 5f; // Radio en el que se desactiva la detecci�n

    private GameObject originalForm; // Referencia al modelo original
    private GameObject currentForm; // Referencia al modelo actual
    private bool isTransformed = false;

    private void Start()
    {
        // Guardar la forma original
        originalForm = Instantiate(this.gameObject, transform.position, transform.rotation);
        originalForm.SetActive(false); // Ocultar la copia original
    }

    private void Update()
    {
        // Transformaci�n con tecla (Ejemplo: presionar "T")
        if (Input.GetKeyDown(KeyCode.T) && !isTransformed)
        {
            TransformIntoObject(0); // Cambiar al primer objeto de la lista
        }

        // Revertir transformaci�n con tecla (Ejemplo: presionar "Y")
        if (Input.GetKeyDown(KeyCode.Y) && isTransformed)
        {
            RevertTransformation();
        }
    }

    public void TransformIntoObject(int objectIndex)
    {
        if (isTransformed || objectIndex >= transformableObjects.Length) return;

        // Instanciar la nueva forma
        currentForm = Instantiate(transformableObjects[objectIndex], transform.position, transform.rotation);
        currentForm.transform.parent = this.transform;
        this.GetComponent<MeshRenderer>().enabled = false; // Desactivar el modelo original
        isTransformed = true;

        // Desactivar detecci�n de enemigos
        DisableEnemyDetection();

        // Revertir despu�s de cierto tiempo
        Invoke(nameof(RevertTransformation), transformationDuration);
    }

    private void RevertTransformation()
    {
        if (!isTransformed) return;

        // Destruir la forma actual
        Destroy(currentForm);
        this.GetComponent<MeshRenderer>().enabled = true; // Reactivar el modelo original
        isTransformed = false;

        // Reactivar detecci�n de enemigos
        EnableEnemyDetection();
    }

    private void DisableEnemyDetection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionDisableRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            // L�gica para desactivar detecci�n (ejemplo: cambiar estado en el script de IA)
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null) enemyAI.SetDetection(false);
        }
    }

    private void EnableEnemyDetection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionDisableRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            // L�gica para reactivar detecci�n
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null) enemyAI.SetDetection(true);
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujar el radio de desactivaci�n de detecci�n
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDisableRadius);
    }
}
