using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;


public class NPCInteraction : MonoBehaviour
{
    public GameObject dialogueUI;              // UI for dialogue boxes
    public TMP_Text dialogueText;              // TMP component for displaying dialogue
    public string[] dialogues;                 // Array of dialogue lines
    public float zoomInSize = 3f;              // Desired zoom size when interacting
    public float zoomDuration = 1f;            // How long the zoom transition should take
    public CinemachineVirtualCamera vCam;      // Cinemachine virtual camera
    public MonoBehaviour playerMovementScript; // Reference to the player's movement script
    public Rigidbody2D playerRb;               // Player's Rigidbody2D for movement control
    public GameObject interactIndicatorUI;     // UI for showing interaction prompt
    public TMP_Text interactIndicatorText;     // Text component for interaction prompt
    public float typingSpeed = 0.05f;          // Speed of the typing effect for dialogue

    private bool playerInRange;                // To check if player is near the NPC
    private int currentDialogueIndex = 0;      // Track the current dialogue line
    private bool isZoomedIn = false;           // Is the camera zoomed in?
    private float originalZoomSize;            // Store the camera's original zoom size
    private Coroutine typingCoroutine;         // For controlling the typing effect coroutine
    private bool dialogueComplete = false;     // Is the current dialogue fully displayed?
    private bool waitingForNext = false;       // Prevent skipping text too fast

    void Start()
    {
        // Save the original camera zoom size and hide dialogue/interaction UI at the start
        originalZoomSize = vCam.m_Lens.OrthographicSize;
        dialogueUI.SetActive(false);
        interactIndicatorUI.SetActive(false); // Hide interaction prompt at the start
    }

    void Update()
    {
        // Check if player is in range and presses any key
        if (playerInRange && Input.anyKeyDown)
        {
            // If the camera isn't zoomed in yet, start interaction
            if (!isZoomedIn)
            {
                StartInteraction();
            }
            else
            {
                // If the dialogue is complete, check if player wants to advance to the next line
                if (dialogueComplete)
                {
                    if (waitingForNext)
                    {
                        NextDialogue();
                        waitingForNext = false;
                    }
                    else
                    {
                        waitingForNext = true; // Wait for another key press to continue
                    }
                }
                else
                {
                    // If the dialogue isn't fully written, complete it instantly
                    if (typingCoroutine != null)
                    {
                        StopCoroutine(typingCoroutine); // Stop the typing effect
                        dialogueText.text = dialogues[currentDialogueIndex]; // Show full text
                        dialogueComplete = true; // Mark as fully displayed
                    }
                }
            }
        }
    }

    private void StartInteraction()
    {
        // Disable player movement script
        playerMovementScript.enabled = false;

        // Freeze player's movement by freezing Rigidbody2D
        if (playerRb != null)
        {
            playerRb.velocity = Vector2.zero;  // Stop current movement
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze all movement
        }

        // Start smooth zoom into the NPC
        StartCoroutine(SmoothZoom(zoomInSize));
        isZoomedIn = true;

        // Start showing the first line of dialogue with typing effect
        dialogueUI.SetActive(true);  // Activate the dialogue UI
        currentDialogueIndex = 0;    // Reset to the first line of dialogue
        typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));

        // Hide interaction prompt once dialogue starts
        interactIndicatorUI.SetActive(false);
    }

    private void NextDialogue()
    {
        // If typing effect is still running, stop it and show full text
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[currentDialogueIndex];
            typingCoroutine = null;
        }

        // Show the next dialogue line, or end interaction if no more lines
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
        {
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));
            waitingForNext = false; // Reset waiting state
        }
        else
        {
            EndInteraction();
        }
    }

    private void EndInteraction()
    {
        // Smoothly zoom back out to the original camera size
        StartCoroutine(SmoothZoom(originalZoomSize));
        isZoomedIn = false;

        // Re-enable player movement
        playerMovementScript.enabled = true;

        // Unfreeze player Rigidbody2D so they can move again
        if (playerRb != null)
        {
            playerRb.constraints = RigidbodyConstraints2D.None; // Remove all constraints
        }

        // Hide the dialogue UI when interaction ends
        dialogueUI.SetActive(false);
    }

    // Coroutine to smoothly zoom in or out the camera
    IEnumerator SmoothZoom(float targetSize)
    {
        float currentSize = vCam.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        // Gradually change the zoom over the duration
        while (elapsedTime < zoomDuration)
        {
            vCam.m_Lens.OrthographicSize = Mathf.Lerp(currentSize, targetSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final size matches the target
        vCam.m_Lens.OrthographicSize = targetSize;
    }

    // Coroutine to type out dialogue letter by letter
    IEnumerator TypeDialogue(string dialogue)
    {
        dialogueComplete = false; // Reset completion status
        dialogueText.text = "";   // Clear existing text

        // Loop through each character in the dialogue string
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed); // Wait before showing the next letter
        }

        // Mark the dialogue as fully written
        dialogueComplete = true;
        typingCoroutine = null;
    }

    // Trigger when player enters the NPC's range
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;  // Player is now in range
            interactIndicatorText.text = "Press any key to interact"; // Update prompt text
            interactIndicatorUI.SetActive(true); // Show interaction prompt
        }
    }

    // Trigger when player leaves the NPC's range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;  // Player is no longer in range
            EndInteraction(); // End interaction if player leaves
            interactIndicatorUI.SetActive(false); // Hide interaction prompt
        }
    }
}
