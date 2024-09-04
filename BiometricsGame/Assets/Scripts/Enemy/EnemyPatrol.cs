using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA; // Primer punto de patrullaje
    public Transform pointB; // Segundo punto de patrullaje
    public float patrolSpeed = 2f; // Velocidad de patrullaje
    public float chaseSpeed = 4f; // Velocidad de persecución
    public float detectionRangeWidth = 5f; // Ancho del rango de detección
    public float detectionRangeHeight = 3f; // Alto del rango de detección

    private Vector3 targetPoint; // Punto de destino actual
    private Transform player; // Referencia al personaje
    private bool isChasing = false; // Indica si el enemigo está persiguiendo al personaje

    void Start()
    {
        targetPoint = pointA.position;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Asumiendo que el jugador tiene el tag "Player"
    }

    void Update()
    {
        // Verifica si el personaje está dentro del rango de detección
        if (Vector3.Distance(transform.position, player.position) < Mathf.Max(detectionRangeWidth, detectionRangeHeight))
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // Mover al enemigo según el estado actual
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            if (targetPoint == pointA.position)
            {
                targetPoint = pointB.position;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                targetPoint = pointA.position;
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    void ChasePlayer()
    {
        // Solo moverse en el eje X hacia la posición del personaje
        Vector3 newPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, chaseSpeed * Time.deltaTime);

        // Asegurarse de que el enemigo mire hacia el personaje
        if (transform.position.x < player.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;

        // Definir el tamaño del rectángulo (ancho y alto)
        Vector3 detectionSize = new Vector3(detectionRangeWidth, detectionRangeHeight, 1);

        // Dibujar un rectángulo que representa el rango de detección
        Gizmos.DrawWireCube(transform.position, detectionSize);
    }
}

