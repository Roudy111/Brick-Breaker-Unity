using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manages the main menu interface and navigation.
/// Handles player name input, game start/exit, and highscore display.
/// 
/// Key responsibilities:
/// - Manages player name input
/// - Handles scene transitions
/// - Displays highscores in menu
/// - Provides game exit functionality
/// 
/// Dependencies:
/// - DataManager for player data persistence
/// - SceneManager for scene transitions
/// - TextMeshPro for input handling
/// 
/// Note: Implements platform-specific quit functionality
/// </summary>

public class MenuHandler : MonoBehaviour
{
    // reference to InputField for player name 
    public TMP_InputField TM_PlayeNameInput;
    public Text highscoresText;

    /// <summary>
    /// Initializes menu components and sets up input listeners
    /// </summary>
    private void Start()
    {
        // Setup player name input handling
        if (TM_PlayeNameInput != null)
        {
            TM_PlayeNameInput.onEndEdit.AddListener(OnInputFieldEndEdit);
        }
        UpdateHighscoresUI();
    }

    /// <summary>
    /// Initiates game start sequence
    /// Ensures player has a name before starting     /// </summary>

    public void StartGame()
    {
        // Set default player name if none provided
        if (string.IsNullOrEmpty(DataManager.Instance.currentPlayerId))
        {
            DataManager.Instance.currentPlayerId = "Player";
        }
        SceneManager.LoadScene(1);
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Handles platform-specific game exit
    /// Different behavior for editor and built game
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Updates player name in DataManager
    /// Called when player finishes editing name input
    /// </summary>
    private void SetPlayerName()
    {
        if (DataManager.Instance != null && TM_PlayeNameInput != null)
        {
            DataManager.Instance.currentPlayerId = TM_PlayeNameInput.text;
        }
    }

    private void OnInputFieldEndEdit(string value)
    {
        SetPlayerName();
    }

    /// <summary>
    /// Updates highscore display in menu
    /// Fetches current highscores from DataManager
    /// </summary>
    private void UpdateHighscoresUI()
    {
        if (highscoresText != null && DataManager.Instance != null)
        {
            highscoresText.text = DataManager.Instance.GetFormattedHighscores();
        }
    }
}