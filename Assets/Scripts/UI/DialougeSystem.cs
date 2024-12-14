using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    private class DialogueChoice
    {
        public string input;
        public string response;
        public Color responseColor = Color.green;
    }

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

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI bootMessagesText;
    [SerializeField] private TextMeshProUGUI operatorText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private Button choiceButtonPrefab;

    [Header("Settings")]
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
    [SerializeField] private DialogueChoice[] choices = new DialogueChoice[]
    {
        new DialogueChoice 
        { 
            input = "RESPOND: Yes",
            response = "> OPERATOR: Thank god. We thought we lost you in the last neural dive."
        },
        new DialogueChoice 
        { 
            input = "RESPOND: No",
            response = "> OPERATOR: Of course not. They wiped you clean. But somewhere inside, you remember."
        }
    };

    private List<DialogueMessage> bootMessageHistory = new List<DialogueMessage>();
    private bool isTyping = false;

    private void Start()
    {
        SetupUI();
        StartCoroutine(PlaySequence());
    }

    private void SetupUI()
    {
        if (ValidateComponents())
        {
            bootMessagesText.color = defaultTextColor;
            bootMessagesText.text = "";
            operatorText.color = defaultTextColor;
            operatorText.text = "";
        }
    }

    private bool ValidateComponents()
    {
        if (bootMessagesText == null || operatorText == null || 
            choiceContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogError("Missing required components!");
            return false;
        }
        return true;
    }

    private IEnumerator PlaySequence()
    {
        // Boot sequence
        foreach (var message in bootSequence)
        {
            yield return StartCoroutine(TypeBootMessage(new DialogueMessage 
            { 
                text = message.text,
                color = message.TextColor
            }));
            yield return new WaitForSeconds(0.5f);
        }

        // Show operator message and choices
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeOperatorMessage(firstContactMessage));
        ShowChoices();
    }

    private void ShowChoices()
    {
        foreach (var choice in choices)
        {
            CreateChoiceButton(choice);
        }
    }

    private void CreateChoiceButton(DialogueChoice choice)
    {
        Button button = Instantiate(choiceButtonPrefab, choiceContainer);
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        
        // Set text and style
        buttonText.text = $"> {choice.input}";
        buttonText.color = defaultTextColor;
        
        // Add click handler
        button.onClick.AddListener(() => HandleChoice(choice));
    }

    private void HandleChoice(DialogueChoice choice)
    {
        StartCoroutine(ProcessChoice(choice));
    }

    private IEnumerator ProcessChoice(DialogueChoice choice)
    {
        // Clear choices
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // Show response
        yield return StartCoroutine(TypeOperatorMessage(choice.response));
        
        yield return new WaitForSeconds(2f);
        HandleSequenceComplete();
    }

    private void HandleSequenceComplete()
    {
        // Add any completion logic here
        Debug.Log("Dialogue sequence complete");
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