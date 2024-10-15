using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state;
    public static event Action<GameStates> OnGameStateChanged;


    private ScoreManager scoreManager;
    private LevelManager levelManager;

    void Start()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        UpdateGameState(GameStates.ballIdle);
    }


    public void Awake()
    {
        instance = this;
        levelManager = FindObjectOfType<LevelManager>();
    }
    public void UpdateGameState(GameStates newstate)
    {
        state = newstate;
        Debug.Log($"Game State Updated: {newstate}");

        switch (newstate)
        {
            case GameStates.ballIdle:
                HandleBallIdle();
                break;
            case GameStates.gameloop:
                HandleGameLoop();
                break;
            case GameStates.levelIsChanging:
                HandleLevelIschanging();
                break;
            case GameStates.gameOver:
                HandleGameOver();
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newstate);
    }

    private void HandleBallIdle()
    {

    }
    private void HandleGameLoop()
    {

    }
    private void HandleLevelIschanging()
    {

    }
    private void HandleGameOver()
    {

    }




}
public enum GameStates
{
    ballIdle,
    gameloop,
    levelIsChanging,
    gameOver,


}
