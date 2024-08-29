using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueCanvas; // Asigna aquí el Canvas que contiene las cajas de texto
    public CinemachineVirtualCamera virtualCamera; // Asigna aquí la Cinemachine Virtual Camera
    public float zoomAmount = 3f; // Ajusta este valor para el zoom (Orthographic Size)
    public float zoomSpeed = 2f; // Velocidad del zoom
    public TypewriterEffect typewriterEffect; // Asigna aquí el componente TypewriterEffect

    private float originalSize;

    private void Start()
    {
        // Asegúrate de que el Canvas esté desactivado al inicio
        if (dialogueCanvas != null)
        {
            dialogueCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("No se ha asignado el Canvas de diálogo.");
        }

        // Guardar el tamaño ortográfico original de la cámara
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
                    typewriterEffect.StartCoroutine("DisplayDialogue"); // Inicia el diálogo
                }
            }

            // Hacer zoom en la cámara
            if (virtualCamera != null)
            {
                StopAllCoroutines(); // Detener cualquier otra animación de zoom en curso
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

            // Restaurar el tamaño ortográfico original de la cámara
            if (virtualCamera != null)
            {
                StopAllCoroutines(); // Detener cualquier otra animación de zoom en curso
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

