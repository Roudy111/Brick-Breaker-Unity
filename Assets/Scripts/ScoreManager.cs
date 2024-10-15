using UnityEngine;
using System;

public class ScoreManager : singleton<ScoreManager>
{

    public event Action<int> ScoreChanged;
    public event Action HighscoreUpdated;

    private int currentScore;
    public int CurrentScore
    {
        get => currentScore;
        private set
        {
            if (currentScore != value)
            {
                currentScore = value;
                ScoreChanged?.Invoke(currentScore);
                CheckHighscoreUpdate();
            }
        }
    }


    public override void Awake()
    {
        base.Awake();
        ResetScore();
    }

    public void AddPoints(int points)
    {
        CurrentScore += points;
    }

    private void CheckHighscoreUpdate()
    {
        if (DataManager.Instance != null)
        {
            bool wasUpdated = DataManager.Instance.AddOrUpdateHighscore(DataManager.Instance.currentPlayerId, CurrentScore);
            if (wasUpdated)
            {
                HighscoreUpdated?.Invoke();
            }
        }
    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}