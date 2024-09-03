using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float pushBackForce = 5f; // Force applied to push the player back
    public int damage = 1; // Damage dealt by the enemy

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            // Get the Rigidbody2D component of the player
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Calculate direction to push the player back
                Vector2 pushBackDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushBackDirection * pushBackForce, ForceMode2D.Impulse);

                // Get the PlayerHealth component of the player
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    // Apply damage to the player
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }
}
