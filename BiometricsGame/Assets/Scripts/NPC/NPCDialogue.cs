using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText;
    public Button interactButton;
    public Button exitButton; // Botón para salir de la interacción
    public CinemachineVirtualCamera virtualCamera;
    public TypewriterEffect typewriterEffect;

    public float zoomedSize = 3f;
    public float normalSize = 5f;
    public float zoomSpeed = 2f;

    private bool isPlayerNearby = false;
    private bool isZoomingIn = false;
    private bool isZoomingOut = false;

    public string[] dialogueLines;
    private GameObject player;

    private float originalCameraSize;

    void Start()
    {
        if (dialogueCanvas != null)
        {
            dialogueCanvas.enabled = false;
        }

        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
            interactButton.onClick.AddListener(OnInteract);
        }

        if (exitButton != null)
        {
            exitButton.gameObject.SetActive(false);
            exitButton.onClick.AddListener(OnExitDialogue);
        }

        player = GameObject.FindGameObjectWithTag("Player");

        if (virtualCamera != null)
        {
            originalCameraSize = virtualCamera.m_Lens.OrthographicSize;
        }
    }

    void Update()
    {
        // Controlar el zoom con suavizado
        if (isZoomingIn)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                zoomedSize,
                Time.deltaTime * zoomSpeed
            );

            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - zoomedSize) < 0.01f)
            {
                virtualCamera.m_Lens.OrthographicSize = zoomedSize;
                isZoomingIn = false;
            }
        }
        else if (isZoomingOut)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(
                virtualCamera.m_Lens.OrthographicSize,
                normalSize,
                Time.deltaTime * zoomSpeed
            );

            if (Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - normalSize) < 0.01f)
            {
                virtualCamera.m_Lens.OrthographicSize = normalSize;
                isZoomingOut = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactButton.gameObject.SetActive(false);
            if (!dialogueCanvas.enabled) // Solo inicia el zoom out si el diálogo no está activo
            {
                isZoomingOut = true;
            }
        }
    }

    private void OnInteract()
    {
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false; // Desactiva el script de movimiento
        }

        dialogueCanvas.enabled = true;
        isZoomingIn = true; // Inicia el zoom in cuando se presiona el botón

        typewriterEffect.StartTyping(dialogueLines);
        interactButton.gameObject.SetActive(false);
    }

    public void OnDialogueEnd()
    {
        exitButton.gameObject.SetActive(true); // Muestra el botón de salida cuando termine el diálogo
    }

    private void OnExitDialogue()
    {
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
        }

        dialogueCanvas.enabled = false;
        exitButton.gameObject.SetActive(false);
        isZoomingOut = true; // Inicia el zoom out al salir del diálogo
    }
}

