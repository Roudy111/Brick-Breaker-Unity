using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    [System.Serializable]
    private class DialogueMessage
    {
        public string text;
        public Color color = Color.green;
        public bool isVisible = false;
        public int visibleCharacters = 0;
    }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI bootMessagesText;
    [SerializeField] private TextMeshProUGUI operatorText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private Color defaultTextColor = Color.green;
    [SerializeField] private Color warningColor = Color.red;

    [Header("Dialogue Content")]
    [SerializeField] private DialogueMessage[] bootMessages = new DialogueMessage[]
    {
        new DialogueMessage { text = "> SYSTEM BOOT..." },
        new DialogueMessage { text = "> NEURAL LINK ESTABLISHED" },
        new DialogueMessage { text = "> SCANNING FOR NEURAL IMPRINTS..." },
        new DialogueMessage { text = "> WARNING: FRAGMENTED MEMORY DETECTED" }
    };

    [Header("Dialogue Choices")]
    [SerializeField] private string operatorMessage = "> OPERATOR: Are you still there? We've been waiting...";
    [SerializeField] private string yesResponse = "> OPERATOR: Thank god. We thought we lost you in the last neural dive.";
    [SerializeField] private string noResponse = "> OPERATOR: Of course not. They wiped you clean. But somewhere inside, you remember.";

    private bool isTyping = false;

    private void Start()
    {
        SetupUI();
        StartCoroutine(PlaySequence());
    }

    private void SetupUI()
    {
        bootMessagesText.color = defaultTextColor;
        bootMessagesText.text = "";
        operatorText.color = defaultTextColor;
        operatorText.text = "";

        // Configure button listeners
        yesButton.onClick.AddListener(() => HandleChoice(yesResponse));
        noButton.onClick.AddListener(() => HandleChoice(noResponse));
        
        // Hide buttons initially
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
    }

    private IEnumerator PlaySequence()
    {
        // Play boot sequence
        for (int i = 0; i < bootMessages.Length; i++)
        {
            if (bootMessages[i].text.Contains("WARNING"))
            {
                bootMessages[i].color = warningColor;
            }
            yield return StartCoroutine(TypeMessage(bootMessages[i], true));
            yield return new WaitForSeconds(0.5f);
        }

        // Show operator message and enable choices
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(TypeOperatorMessage(operatorMessage));
        
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private IEnumerator TypeMessage(DialogueMessage message, bool isBootMessage)
    {
        isTyping = true;
        message.isVisible = true;
        message.visibleCharacters = 0;
        
        while (message.visibleCharacters < message.text.Length)
        {
            message.visibleCharacters++;
            UpdateDisplay(isBootMessage);
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void UpdateDisplay(bool isBootMessage)
    {
        if (isBootMessage)
        {
            string fullText = "";
            for (int i = 0; i < bootMessages.Length; i++)
            {
                if (bootMessages[i].isVisible)
                {
                    string visibleText = bootMessages[i].text.Substring(0, bootMessages[i].visibleCharacters);
                    fullText += $"<color=#{ColorUtility.ToHtmlStringRGB(bootMessages[i].color)}>{visibleText}</color>\n";
                }
            }
            bootMessagesText.text = fullText.TrimEnd('\n') + "<color=#00ff00>_</color>";
        }
    }

    private IEnumerator TypeOperatorMessage(string message)
    {
        operatorText.text = "";
        string currentText = "";

        foreach (char c in message)
        {
            currentText += c;
            operatorText.text = currentText + "<color=#00ff00>_</color>";
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void HandleChoice(string response)
    {
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        StartCoroutine(ShowResponse(response));
    }

    private IEnumerator ShowResponse(string response)
    {
        operatorText.text = "";
        string currentText = "";

        foreach (char c in response)
        {
            currentText += c;
            operatorText.text = currentText + "<color=#00ff00>_</color>";
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(2f);
        // Handle sequence completion here
    }
}