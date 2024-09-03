using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Vector3 checkpointPosition; // Store checkpoint position
    private bool hasCheckpoint; // Flag to check if a checkpoint exists

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this instance across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    public void SetCheckpoint(Vector3 position)
    {
        // Set the checkpoint position and mark as existing
        checkpointPosition = position;
        hasCheckpoint = true;
    }

    public Vector3 GetCheckpoint()
    {
        // Return the checkpoint position if it exists, otherwise return default position
        if (hasCheckpoint)
        {
            return checkpointPosition;
        }
        else
        {
            return Vector3.zero; // Default position when no checkpoint
        }
    }

    public void ResetCheckpoint()
    {
        // Clear the checkpoint data
        hasCheckpoint = false;
    }
}
