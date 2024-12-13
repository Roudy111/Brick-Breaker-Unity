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

    [SerializeField] private TextMeshProUGUI mainText;
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
    [SerializeField] private string[] responses = new string[]
    {
        "> OPERATOR: Thank god. We thought we lost you in the last neural dive.",
        "> OPERATOR: Of course not. They wiped you clean. But somewhere inside, you remember."
    };

    private List<DialogueMessage> messageHistory = new List<DialogueMessage>();
    private bool isTyping = false;

    private void Start()
    {
        if (mainText != null)
        {
            mainText.color = defaultTextColor;
            mainText.text = "";
            StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        // Play boot sequence
        foreach (var message in bootSequence)
        {
            yield return StartCoroutine(TypeMessage(new DialogueMessage 
            { 
                text = message.text,
                color = message.TextColor
            }));
            yield return new WaitForSeconds(0.5f);
        }

        // Show first contact
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeMessage(new DialogueMessage 
        { 
            text = firstContactMessage,
            color = defaultTextColor
        }));

        // Show final message (for demo, using first response)
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeMessage(new DialogueMessage 
        { 
            text = responses[0],
            color = defaultTextColor
        }));
    }

    private void AddMessageToHistory(DialogueMessage message)
    {
        messageHistory.Add(message);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        string fullText = "";
        foreach (var msg in messageHistory)
        {
            if (msg.isVisible)
            {
                fullText += $"<color=#{ColorUtility.ToHtmlStringRGB(msg.color)}>{msg.text}</color>\n";
            }
        }
        mainText.text = fullText.TrimEnd('\n') + "<color=#00ff00>_</color>";
    }

    private IEnumerator TypeMessage(DialogueMessage message)
    {
        isTyping = true;
        string originalText = message.text;
        message.text = "";
        AddMessageToHistory(message);

        foreach (char c in originalText)
        {
            message.text += c;
            UpdateDisplay();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}