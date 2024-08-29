using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas; // Asigna aqu� el Canvas que contiene las cajas de texto
    public CinemachineVirtualCamera virtualCamera; // Asigna aqu� la Cinemachine Virtual Camera
    public float zoomAmount = 3f; // Ajusta este valor para el zoom (Orthographic Size)
    public float zoomSpeed = 2f; // Velocidad del zoom
    public TypewriterEffect typewriterEffect; // Asigna aqu� el componente TypewriterEffect

    private float originalSize;

    private void Start()
    {
        // Aseg�rate de que el Canvas est� desactivado al inicio
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("No se ha asignado el Canvas de di�logo.");
        }

        // Guardar el tama�o ortogr�fico original de la c�mara
        if (virtualCamera != null)
        {
            originalSize = virtualCamera.m_Lens.OrthographicSize;
        }
        else
        {
            Debug.LogError("No se ha asignado la Cinemachine Virtual Camera.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Mostrar el Canvas cuando el jugador entra en la zona del NPC
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(true);
                if (typewriterEffect != null)
                {
                    typewriterEffect.gameObject.SetActive(true);
                    typewriterEffect.StartCoroutine("DisplayDialogue"); // Inicia el di�logo
                }
            }

            // Hacer zoom en la c�mara
            if (virtualCamera != null)
            {
                StopAllCoroutines(); // Detener cualquier otra animaci�n de zoom en curso
                StartCoroutine(ZoomCamera(zoomAmount));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ocultar el Canvas cuando el jugador sale de la zona del NPC
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(false);
                if (typewriterEffect != null)
                {
                    typewriterEffect.gameObject.SetActive(false); // Oculta el texto
                }
            }

            // Restaurar el tama�o ortogr�fico original de la c�mara
            if (virtualCamera != null)
            {
                StopAllCoroutines(); // Detener cualquier otra animaci�n de zoom en curso
                StartCoroutine(ZoomCamera(originalSize));
            }
        }
    }

    private IEnumerator ZoomCamera(float targetSize)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomSpeed)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / zoomSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        virtualCamera.m_Lens.OrthographicSize = targetSize;
    }
}

