using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    private ScoreManager scoreManager;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text HighscoreText;


    private void OnEnable()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.ScoreChanged += UpdateScoreText;
            scoreManager.HighscoreUpdated += UpdateHighscoreText;
            scoreManager.ResetScore();

        }
        else
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }
    }

    private void OnDisable()
    {
        if (scoreManager != null)
        {
            scoreManager.ScoreChanged -= UpdateScoreText;
            scoreManager.HighscoreUpdated -= UpdateHighscoreText;
        }
    }

    void UpdateScoreText(int score)
    {
        ScoreText.text = $"Score : {score}";
    }

    void UpdateHighscoreText()
    {
        if (HighscoreText != null && DataManager.Instance != null)
        {
            string highscores = DataManager.Instance.GetFormattedHighscores();
            HighscoreText.text = $"Highscores:\n{highscores}";
        }
    }

    void Start()
    {
        UpdateHighscoreText();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
