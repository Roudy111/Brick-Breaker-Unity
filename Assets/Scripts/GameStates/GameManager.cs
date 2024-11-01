using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state { get; private set; }
    public static event Action<GameStates> OnGameStateChanged;

    [Header("Game Components")]
    private ScoreManager scoreManager;
    private LevelManager levelManager;
    private GameOverState gameOverState;
    private Ball ball;

    [Header("Game Settings")]
    [SerializeField] private float levelTransitionDelay = 5f;
    private bool isTransitioning = false;

    void Awake()
    {
        instance = this;
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        levelManager = FindObjectOfType<LevelManager>();
        gameOverState = FindObjectOfType<GameOverState>();
        ball = FindObjectOfType<Ball>();
        scoreManager = ScoreManager.Instance;

        if (!levelManager || !ball || !scoreManager)
        {
            Debug.LogError("GameManager: Required components missing!");
        }
    }

    void Start()
    {
        Debug.Log("GameManager: Starting game");
        UpdateGameState(GameStates.BallIdle);
    }

    public void UpdateGameState(GameStates newState)
    {
        Debug.Log($"GameManager: Attempting state change from {state} to {newState}");

        // Allow LevelChanging -> BallIdle transition even when transitioning
        if (isTransitioning && newState != GameStates.GameOver &&
            !(state == GameStates.LevelChanging && newState == GameStates.BallIdle))
        {
            Debug.Log($"GameManager: Blocked state change to {newState} - currently transitioning");
            return;
        }

        state = newState;
        Debug.Log($"GameManager: State changed to {newState}");

        switch (newState)
        {
            case GameStates.BallIdle:
                Debug.Log("GameManager: Handling BallIdle state");
                if (ball) ball.ResetBall();
                isTransitioning = false;
                break;

            case GameStates.GameLoop:
                Debug.Log("GameManager: Handling GameLoop state");
                isTransitioning = false;
                break;

            case GameStates.LevelChanging:
                Debug.Log("GameManager: Handling LevelChanging state");
                isTransitioning = true;
                if (ball) ball.ResetBall();
                break;

            case GameStates.GameOver:
                Debug.Log("GameManager: Handling GameOver state");
                if (gameOverState) gameOverState.InitiateGameOver();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}
public enum GameStates
{
    BallIdle,   // Ball is attached to paddle
    GameLoop,    // Ball is in play
    LevelChanging,
    GameOver
}