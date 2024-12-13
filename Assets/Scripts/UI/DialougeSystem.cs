using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    private class DialogueMessage
    {
        public string text;
        public Color color = Color.green;
        public bool isVisible = true;
    }

    [System.Serializable]
    private class BootMessage
    {
        public string text;
        public bool isWarning;
        public Color textColor = Color.green;
        public Color TextColor => isWarning ? Color.red : textColor;
    }

    [SerializeField] private TextMeshProUGUI bootMessagesText; // For boot sequence messages
    [SerializeField] private TextMeshProUGUI operatorText;     // Separate text for operator messages
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private Color defaultTextColor = Color.green;

    [Header("Boot Sequence")]
    [SerializeField] private BootMessage[] bootSequence = new BootMessage[]
    {
        new BootMessage { text = "> SYSTEM BOOT...", isWarning = false },
        new BootMessage { text = "> NEURAL LINK ESTABLISHED", isWarning = false },
        new BootMessage { text = "> SCANNING FOR NEURAL IMPRINTS...", isWarning = false },
        new BootMessage { text = "> WARNING: FRAGMENTED MEMORY DETECTED", isWarning = true }
    };

    [Header("First Contact")]
    [SerializeField] private string firstContactMessage = "> OPERATOR: Are you still there? We've been waiting...";

    private List<DialogueMessage> bootMessageHistory = new List<DialogueMessage>();
    private bool isTyping = false;

    private void Start()
    {
        if (bootMessagesText != null && operatorText != null)
        {
            bootMessagesText.color = defaultTextColor;
            bootMessagesText.text = "";
            operatorText.color = defaultTextColor;
            operatorText.text = "";
            StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        // Play boot sequence in the boot messages area
        foreach (var message in bootSequence)
        {
            yield return StartCoroutine(TypeBootMessage(new DialogueMessage 
            { 
                text = message.text,
                color = message.TextColor
            }));
            yield return new WaitForSeconds(0.5f);
        }

        // Show operator message in its own text area
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeOperatorMessage(firstContactMessage));
    }

    private void UpdateBootDisplay()
    {
        string fullText = "";
        foreach (var msg in bootMessageHistory)
        {
            if (msg.isVisible)
            {
                fullText += $"<color=#{ColorUtility.ToHtmlStringRGB(msg.color)}>{msg.text}</color>\n";
            }
        }
        bootMessagesText.text = fullText.TrimEnd('\n') + "<color=#00ff00>_</color>";
    }

    private IEnumerator TypeBootMessage(DialogueMessage message)
    {
        isTyping = true;
        string originalText = message.text;
        message.text = "";
        bootMessageHistory.Add(message);

        foreach (char c in originalText)
        {
            message.text += c;
            UpdateBootDisplay();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private IEnumerator TypeOperatorMessage(string message)
    {
        isTyping = true;
        string currentText = "";
        operatorText.text = "";

        foreach (char c in message)
        {
            currentText += c;
            operatorText.text = currentText + "<color=#00ff00>_</color>";
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}