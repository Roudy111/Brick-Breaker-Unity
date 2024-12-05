using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class IntroTextSequence
{
    public string text;
    [SerializeField] private Color textColor = Color.green; // Default to green
    public float fontSize = 40f;

    public Color TextColor => textColor;
}

public class TerminalSequenceController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI systemDetailsText;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button initializeButton;
    [SerializeField] private Image gridOverlay;
    [SerializeField] private Image scanLineOverlay;

    [Header("Text Settings")]
    [SerializeField] private Color defaultTextColor = Color.green;
    
    [Header("Sequence Settings")]
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private float glitchDuration = 0.5f;
    [SerializeField] private float sceneDelay = 1f;
    
    [Header("Intro Sequences")]
    [SerializeField] private IntroTextSequence[] introSequences = new IntroTextSequence[]
    {
        new IntroTextSequence { text = "NEO-TOKYO, 2099", fontSize = 36f },
        new IntroTextSequence { text = "The digital walls have eyes.", fontSize = 24f },
        new IntroTextSequence { text = "Every piece of data is monitored, controlled, encrypted by the Megacorps.", fontSize = 24f },
        new IntroTextSequence { text = "But they can't stop what they can't see.", fontSize = 28f },
        new IntroTextSequence { text = "INITIATING SEQUENCE: NEON BREACH", fontSize = 42f }
    };

    private int currentScene = 0;
    private bool isTyping = false;
    private Coroutine typeCoroutine;

    private void OnEnable()
    {
        // Initialize text components
        if (mainText != null)
        {
            mainText.color = defaultTextColor;
            mainText.text = "";
        }

        if (systemDetailsText != null)
        {
            systemDetailsText.color = defaultTextColor;
            systemDetailsText.text = "";
            systemDetailsText.fontSize = 24;
        }
    }

    private void Start()
    {
        Debug.Log("Starting sequence...");
        SetupUI();
        StartSequence();
        CheckTextVisibility();
        EnsureTextVisibility();
    }

    private void SetupUI()
    {
        if (mainText == null || systemDetailsText == null)
        {
            Debug.LogError("Text components not assigned!");
            return;
        }

        // Configure main text
        //mainText.alignment = TextAlignmentOptions.Center;
        //mainText.enableWordWrapping = true;
        
        // Configure system details text
        systemDetailsText.alignment = TextAlignmentOptions.Left;
        systemDetailsText.fontSize = 24;
        systemDetailsText.alpha = 0.5f;

        skipButton.gameObject.SetActive(false);
        initializeButton.gameObject.SetActive(false);
        
        StartCoroutine(ShowSkipButtonDelayed());
        skipButton.onClick.AddListener(HandleSkip);
        initializeButton.onClick.AddListener(HandleInitialize);
    }
    private IEnumerator ShowSkipButtonDelayed()
{
    yield return new WaitForSeconds(2f);
    skipButton.gameObject.SetActive(true);
}

    private void StartSequence()
    {
        currentScene = 0;
        Debug.Log($"Starting sequence with {introSequences.Length} scenes");
        PlayNextScene();
    }
    void CheckTextVisibility()
{
    // Verify TextMeshPro component reference
    if (mainText == null)
    {
        Debug.LogError("mainText reference is missing!");
        return;
    }

    // Check core visibility settings
    Debug.Log($"Alpha: {mainText.alpha}");
    Debug.Log($"Enabled: {mainText.enabled}");
    Debug.Log($"GameObject active: {mainText.gameObject.activeInHierarchy}");
    Debug.Log($"Color: {mainText.color}");
}

void EnsureTextVisibility()
{
    // Make text fully visible
    mainText.alpha = 1f;
    mainText.color = new Color(mainText.color.r, mainText.color.g, mainText.color.b, 1f);
    
    // Ensure GameObject hierarchy is active
    mainText.gameObject.SetActive(true);
    
    // Verify parent Canvas
    Canvas parentCanvas = mainText.GetComponentInParent<Canvas>();
    if (parentCanvas != null)
    {
        parentCanvas.gameObject.SetActive(true);
    }
    
    // Set test text
    mainText.text = "Test Message";
}

    

    private void PlayNextScene()
    {
        if (currentScene >= introSequences.Length)
        {
            ShowInitializeButton();
            return;
        }

        if (typeCoroutine != null)
        {
            StopCoroutine(typeCoroutine);
        }

        var sequence = introSequences[currentScene];
        Debug.Log($"Playing scene {currentScene}: {sequence.text}");
        
        mainText.fontSize = sequence.fontSize;
        mainText.color = sequence.TextColor;
        typeCoroutine = StartCoroutine(TypeText(sequence.text));
        UpdateSystemDetails();
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        mainText.text = "";
        string currentText = "";

        foreach (char c in text)
        {
            currentText += c;
            mainText.text = currentText + "<color=#00ff00>_</color>"; // Add blinking cursor
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        yield return new WaitForSeconds(sceneDelay);
        StartCoroutine(TriggerGlitchEffect());
    }

    private IEnumerator TriggerGlitchEffect()
    {
        RectTransform rect = mainText.GetComponent<RectTransform>();
        Vector2 originalPos = rect.anchoredPosition;

        float elapsed = 0f;
        while (elapsed < glitchDuration)
        {
            rect.anchoredPosition = originalPos + Random.insideUnitCircle * 10f;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = originalPos;
        currentScene++;
        PlayNextScene();
    }

    private void UpdateSystemDetails()
    {
        string details = $"> SYSTEM://init_sequence_{currentScene + 1}\n" +
                        $"> MEMORY://allocated_0x{Random.Range(0, 0xFFFFFF):X6}";
        
        StartCoroutine(TypeSystemDetails(details));
    }

    private IEnumerator TypeSystemDetails(string text)
    {
        string currentText = "";
        foreach (char c in text)
        {
            currentText += c;
            systemDetailsText.text = currentText;
            yield return new WaitForSeconds(typingSpeed * 0.5f); // Type system details faster
        }
    }

    private void HandleSkip()
    {
        if (typeCoroutine != null)
            StopCoroutine(typeCoroutine);
            
        StopAllCoroutines(); // Stop all running animations
        currentScene = introSequences.Length;
        ShowInitializeButton();
    }

    private void ShowInitializeButton()
    {
        mainText.text = "";
        skipButton.gameObject.SetActive(false);
        initializeButton.gameObject.SetActive(true);
    }

    private void HandleInitialize()
    {
        Debug.Log("Initialize System");
    }
}