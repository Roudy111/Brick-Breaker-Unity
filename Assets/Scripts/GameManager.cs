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
    private GameOverState gameOverState;

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
        gameOverState = FindObjectOfType<GameOverState>();
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
        if (!gameOverState)
        {

            gameOverState.InitiateGameOver();
        }
    }




}
public enum GameStates
{
    ballIdle,
    gameloop,
    levelIsChanging,
    gameOver,


}
