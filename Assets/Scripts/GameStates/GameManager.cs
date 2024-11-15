using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// Core manager class that handles the game's state machine.
/// 
/// Key responsibilities:
/// - Manages game state transitions (idle, gameplay, level changing, game over)
/// - Coordinates between ScoreManager, LevelManager, and other subsystems
/// - Broadcasts state changes to other components through events
/// 
/// Dependencies:
/// - Requires ScoreManager, LevelManager, GameOverState, and Ball components
/// - Uses event system for state change notifications
/// </summary>

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state;
    public static event Action<GameStates> OnGameStateChanged;

// References to dependencies
    private ScoreManager scoreManager;
    private LevelManager levelManager;
    private GameOverState gameOverState;
    private Ball ball;

    void Start()
    {
        // Initialize score management system
        scoreManager = ScoreManager.Instance;
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }
        // Set initial game state to idle, waiting for player input
        UpdateGameState(GameStates.idle);
    }


    public void Awake()
    {
        instance = this;
        levelManager = FindObjectOfType<LevelManager>();
        gameOverState = FindObjectOfType<GameOverState>();
        ball = FindObjectOfType<Ball>();

    }

    //State machine for game states that has been defined in enum.
    public void UpdateGameState(GameStates newstate)
    {
        state = newstate;
        Debug.Log($"Game State Updated: {newstate}");

        switch (newstate)
        {
            case GameStates.gamePlay:
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
        // Notify all subscribers about the state change
        OnGameStateChanged?.Invoke(newstate);
    }



    private void HandleLevelIschanging()
    {
       ball.ResetBall();

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
    idle,
    gamePlay,
    levelIsChanging,
    gameOver,


}
