using UnityEngine;
using System;

public class ScoreManager : singleton<ScoreManager>
{
    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;
        private set
        {
            if (currentScore != value)
            {
                currentScore = value;
                OnScoreChanged?.Invoke(currentScore);
                CheckAndUpdateHighscore();
            }
        }
    }

    public event Action<int> OnScoreChanged;
    public event Action OnHighscoreUpdated;

    public override void Awake()
    {
        base.Awake();
        ResetScore();
    }

    public void AddPoints(int points)
    {
        CurrentScore += points;
    }

    private void CheckAndUpdateHighscore()
    {
        if (DataManager.Instance != null)
        {
            bool wasUpdated = DataManager.Instance.AddOrUpdateHighscore(DataManager.Instance.currentPlayerId, CurrentScore);
            if (wasUpdated)
            {
                OnHighscoreUpdated?.Invoke();
            }
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}