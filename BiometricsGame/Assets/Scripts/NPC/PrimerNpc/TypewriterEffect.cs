using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    // UI element for displaying dialogue
    public TextMeshProUGUI dialogueText;

    // Typing speed (seconds per letter)
    public float typingSpeed = 0.05f;
    
    // Dialogue data
    private string[] dialogueLines; 
    private int currentLineIndex;

    // Typing state
    private bool isTyping; 
    private bool cancelTyping;

    // Event triggered when dialogue finishes
    public delegate void TypingCompleteHandler();
    public event TypingCompleteHandler OnTypingComplete;


    // Start typing dialogue lines
    public void StartTyping(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        StartCoroutine(DisplayDialogue());
    }

    // Display each dialogue line
    private IEnumerator DisplayDialogue()
    {
        while (currentLineIndex < dialogueLines.Length)
        {
            yield return StartCoroutine(TypeLine(dialogueLines[currentLineIndex]));
            currentLineIndex++;
            yield return new WaitUntil(() => Input.anyKeyDown); // Wait for key press
        }

        OnTypingComplete?.Invoke(); // Trigger event when done
    }

    // Type out a single line
    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";
        isTyping = true;
        cancelTyping = false;

        foreach (char letter in line.ToCharArray())
        {
            if (cancelTyping) // Skip typing if canceled
            {
                dialogueText.text = line;
                break;
            }

            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // Check for input to skip typing
    void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            cancelTyping = true;
        }
    }
}

