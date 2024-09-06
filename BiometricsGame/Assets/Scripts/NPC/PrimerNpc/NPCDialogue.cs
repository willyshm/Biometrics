using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{

    // UI elements
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Button interactButton;
    public Button exitButton;

    // Camera settings
    public CinemachineVirtualCamera virtualCamera;
    public TypewriterEffect typewriterEffect;
    public float zoomedSize = 3f;
    public float normalSize = 5f;
    public float zoomSpeed = 2f;

    // States
    private bool isPlayerNearby = false;
    private bool isZoomingIn = false;
    private bool isZoomingOut = false;
    private bool isDialogueActive = false;
    private bool isDialogueComplete = false;

    // Dialogue data
    public string[] dialogueLines;
    private GameObject player;
    private float originalCameraSize;

    void Start()
    {
        // Disable dialogue canvas if it's assigned
        if (dialogueCanvas != null)
        {
            dialogueCanvas.enabled = false;
        }

        // Hide interact button and add listener if assigned
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteract);
        }

        // Hide exit button and add listener if assigned
        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(false);
            exitButton.onClick.AddListener(OnExitDialogue);
        }

        // Find and assign player object by tag
        player = GameObject.FindGameObjectWithTag("Player");


        // Store the original camera size if virtual camera is assigned
        if (virtualCamera != null)
        {
            originalCameraSize = virtualCamera.m_Lens.OrthographicSize;
        }

        // Subscribe to typewriter effect completion event
        typewriterEffect.OnTypingComplete += HandleTypingComplete; // Suscribirse al evento
    }

    void Update()
    {
        
        // Zoom in if isZoomingIn is true
        if (isZoomingIn)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                zoomedSize,
                Time.deltaTime * zoomSpeed
            );

            // Stop zooming in when close enough to target size
            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - zoomedSize) < 0.01f)
            {
                virtualCamera.m_Lens.OrthographicSize = zoomedSize;
                isZoomingIn = false;
            }
        }

        // Zoom out if isZoomingOut is true
        else if (isZoomingOut)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                originalCameraSize,
                Time.deltaTime * zoomSpeed
            );
            
            // Stop zooming out when close enough to original size
            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - originalCameraSize) < 0.01f)
            {
                virtualCamera.m_Lens.OrthographicSize = originalCameraSize;
                isZoomingOut = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Show interact button when player enters trigger zone
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (!isDialogueActive && !isDialogueComplete)
            {
                interactButton.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Hide interact button and start zooming out when player exits trigger zone
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactButton.gameObject.SetActive(false);
            if (!isDialogueActive)
            {
                isZoomingOut = true;
            }
        }
    }

    private void OnInteract()
    {
        // Disable player movement and start dialogue
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
        }

        isDialogueActive = true;
        isDialogueComplete = false;
        dialogueCanvas.enabled = true;
        isZoomingIn = true;

        typewriterEffect.StartTyping(dialogueLines);
        interactButton.gameObject.SetActive(false); // Hide interact button
    }

    private void HandleTypingComplete()
    {
        // Mark dialogue as complete when typing finishes
        isDialogueComplete = true;
        OnDialogueEnd();
    }

    public void OnDialogueEnd()
    {
        // Show exit button when dialogue is complete
        if (isDialogueComplete)
        {
            exitButton.gameObject.SetActive(true);
        }
    }

    private void OnExitDialogue()
    {
        // Enable player movement and hide dialogue UI
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
        }

        dialogueCanvas.enabled = false;
        exitButton.gameObject.SetActive(false);
        isZoomingOut = true;
        isDialogueActive = false;

        // Show interact button again if player is still nearby and dialogue is incomplete
        if (isPlayerNearby && !isDialogueComplete) 
        {
            interactButton.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the typing complete event
        typewriterEffect.OnTypingComplete -= HandleTypingComplete; // Desuscribirse del evento
    }
}

