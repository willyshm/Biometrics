using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA; // Primer punto de patrullaje
    public Transform pointB; // Segundo punto de patrullaje
    public float patrolSpeed = 2f; // Velocidad de patrullaje
    public float chaseSpeed = 4f; // Velocidad de persecución
    public float detectionRangeWidth = 5f; // Ancho del rango de detección
    public float detectionRangeHeight = 3f; // Alto del rango de detección
    public float groundDetectionDistance = 2f; // Distancia para detectar suelo
    public Transform groundDetectionPoint; // Punto en el que se lanza el Raycast (frente del enemigo)

    private Vector3 targetPoint; // Punto de destino actual
    private Transform player; // Referencia al personaje
    private bool isChasing = false; // Indica si el enemigo está persiguiendo al personaje
    private bool isGroundAhead = true; // Verifica si hay suelo adelante del enemigo

    void Start()
    {
        targetPoint = pointA.position;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Asumiendo que el jugador tiene el tag "Player"
    }

    void Update()
    {
        // Verifica si hay suelo delante del enemigo antes de realizar cualquier movimiento
        CheckForGround();

        // Verifica si el personaje está dentro del rango de detección
        if (Vector3.Distance(transform.position, player.position) < Mathf.Max(detectionRangeWidth, detectionRangeHeight) && isGroundAhead)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        // Mover al enemigo según el estado actual
        if (isChasing && isGroundAhead)
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

    void CheckForGround()
    {
        // Lanza un Raycast desde el centro hacia abajo para verificar si hay suelo bajo el enemigo
        RaycastHit2D groundInfoCenter = Physics2D.Raycast(transform.position, Vector2.down, groundDetectionDistance);

        // Lanza otro Raycast desde el frente hacia abajo para detectar si hay un vacío delante
        RaycastHit2D groundInfoAhead = Physics2D.Raycast(groundDetectionPoint.position, Vector2.down, groundDetectionDistance);

        // Si no hay suelo debajo o delante del enemigo, deja de perseguir al jugador
        if (groundInfoCenter.collider == null || groundInfoAhead.collider == null)
        {
            isGroundAhead = false;
        }
        else
        {
            isGroundAhead = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar el Raycast hacia abajo desde el centro del enemigo
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDetectionDistance);

        // Dibujar el Raycast hacia abajo desde el frente del enemigo
        if (groundDetectionPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundDetectionPoint.position, groundDetectionPoint.position + Vector3.down * groundDetectionDistance);
        }
    }
}

