using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public NPCDialogue npcDialogue;

    private string[] lines;
    private bool isTyping = false;

    void Update()
    {
        if (Input.anyKeyDown && isTyping) // Detecta cualquier tecla
        {
            StopAllCoroutines();
            dialogueText.text = lines[lines.Length - 1]; // Muestra todo el texto
            isTyping = false;
            npcDialogue.OnDialogueEnd(); // Llama al método para mostrar el botón de salida
        }
    }

    public void StartTyping(string[] dialogueLines)
    {
        lines = dialogueLines;
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        foreach (string line in lines)
        {
            dialogueText.text = ""; // Limpia el texto
            foreach (char letter in line.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            isTyping = true;
            yield return new WaitUntil(() => !isTyping); // Espera a que se termine de escribir
        }
    }
}
