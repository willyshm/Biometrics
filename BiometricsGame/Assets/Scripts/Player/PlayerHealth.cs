using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [HideInInspector]
    public int health = 3; // Player's health, visible in Inspector but hidden in other parts

    private bool isDead; // Flag to check if the player is dead

    private void Start()
    {
        isDead = false; // Initialize isDead to false at start
    }

    public void TakeDamage(int damage)
    {
        // Decrease health by damage amount and check for death
        health -= damage;
        if (health <= 0)
        {
            Die(); // Call Die method if health is zero or below
        }
    }

    private void Die()
    {
        isDead = true; // Mark player as dead
        Vector3 respawnPosition = GameManager.Instance.GetCheckpoint(); // Get checkpoint position

        if (respawnPosition == Vector3.zero)
        {
            // No checkpoint, reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Respawn at checkpoint and reset health
            transform.position = respawnPosition;
            health = 3; // Reset health to initial value
            isDead = false; // Mark player as not dead
        }
    }
}
