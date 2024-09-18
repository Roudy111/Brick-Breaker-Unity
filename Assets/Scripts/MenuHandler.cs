using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public TMP_InputField TM_PlayeNameInput;
    public Text highscoresText;

    private void Start()
    {
        if (TM_PlayeNameInput != null)
        {
            TM_PlayeNameInput.onEndEdit.AddListener(OnInputFieldEndEdit);
        }
        UpdateHighscoresUI();
    }

    public void StartGame()
    {
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

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

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

    private void UpdateHighscoresUI()
    {
        if (highscoresText != null && DataManager.Instance != null)
        {
            highscoresText.text = DataManager.Instance.GetFormattedHighscores();
        }
    }
}