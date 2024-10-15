using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpdate : MonoBehaviour
{
    /// <summary>
    /// Subscribe to the event of the destruction of each Brick to update score in realtime for each seassion
    /// </summary>

    void OnEnable()
    {
        Brick.BrickDestroyed += UpdateScore;
        ScoreManager.Instance.ScoreChanged += OnScoreChanged;
    }

    void OnDisable()
    {
        Brick.BrickDestroyed -= UpdateScore;
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreChanged -= OnScoreChanged;
        }
    }

    void UpdateScore(int pointValue)
    {
        ScoreManager.Instance.AddPoints(pointValue);
        Debug.Log($"Brick destroyed. Points added: {pointValue}");
    }

    void OnScoreChanged(int newScore)
    {
        Debug.Log($"Score updated. Current score: {newScore}");
    }
}

