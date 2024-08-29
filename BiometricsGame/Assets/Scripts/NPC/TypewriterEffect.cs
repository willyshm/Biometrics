using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public Text dialogueText; // Asigna aqu� el componente Text
    public float typingSpeed = 0.05f; // Velocidad de escritura
    public string[] dialogues; // Array para varios textos de di�logo

    private int currentIndex = 0;
    private bool isTyping = false;

    private void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.text = ""; // Aseg�rate de que el texto est� vac�o al inicio
            StartCoroutine(DisplayDialogue());
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isTyping)
        {
            // Avanzar al siguiente texto al presionar cualquier tecla
            if (currentIndex < dialogues.Length - 1)
            {
                currentIndex++;
                StopAllCoroutines(); // Detener la animaci�n de escritura actual
                StartCoroutine(DisplayDialogue());
            }
            else
            {
                // Opcional: Hacer algo cuando se termina el di�logo
                // Por ejemplo, ocultar el Canvas o realizar otra acci�n
                dialogueText.gameObject.SetActive(false); // Ocultar el texto
            }
        }
    }

    private IEnumerator DisplayDialogue()
    {
        dialogueText.text = ""; // Limpiar el texto actual

        isTyping = true;
        string dialogue = dialogues[currentIndex]; // Obtener el texto del di�logo actual

        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
