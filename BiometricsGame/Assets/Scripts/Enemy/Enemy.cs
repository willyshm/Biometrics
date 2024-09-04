using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float pushBackForce = 5f; // Fuerza para empujar al jugador
    public int damage = 1; // Daño que inflige el enemigo
    public float attackCooldown = 1.5f; // Tiempo de espera entre ataques

    private bool canAttack = true; // Controla si el enemigo puede atacar
    private float cooldownTimer = 0f; // Temporizador de cooldown

    private void Update()
    {
        // Controla el cooldown del ataque
        if (!canAttack)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCooldown)
            {
                canAttack = true;
                cooldownTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            // Calcula la dirección para empujar al jugador
            Vector2 pushBackDirection = (collision.transform.position - transform.position).normalized;
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.AddForce(pushBackDirection * pushBackForce, ForceMode2D.Impulse);

                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }

                canAttack = false;
            }
        }
    }
}
