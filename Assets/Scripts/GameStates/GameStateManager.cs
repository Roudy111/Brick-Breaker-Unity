using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStateManager : singleton<GameStateManager>
{
    private GameStates currentState;
    private bool isTransitioning;
    private Coroutine activeTransition;

    [Header("Components")]
    private Ball ball;
    private LevelManager levelManager;
    private GameOverState gameOverState;

    [Header("Settings")]
    [SerializeField] private float levelTransitionDelay = 5f;

    public static event Action<GameStates> OnStateEnter;
    public static event Action<GameStates> OnStateExit;
    public static event Action OnBallDestroyed;

    public GameStates CurrentState => currentState;

    public override void Awake()
    {
        base.Awake();
        InitializeComponents();
        SubscribeToEvents();
    }

    private void InitializeComponents()
    {
        ball = FindObjectOfType<Ball>();
        levelManager = FindObjectOfType<LevelManager>();
        gameOverState = FindObjectOfType<GameOverState>();

        if (!ball || !levelManager || !gameOverState)
        {
            Debug.LogError("GameStateManager: Required components missing!");
        }
    }

    private void SubscribeToEvents()
    {
        Counter.LevelFinished += OnLevelFinished;
        OnBallDestroyed += OnBallLost;
    }

    private void OnDestroy()
    {
        Counter.LevelFinished -= OnLevelFinished;
        OnBallDestroyed -= OnBallLost;
    }

    private void Start()
    {
        ChangeState(GameStates.BallIdle);
    }

    private void OnLevelFinished()
    {
        Debug.Log("GameStateManager: Level finished event received");
        ChangeState(GameStates.LevelChanging);
    }

    private void OnBallLost()
    {
        Debug.Log("GameStateManager: Ball lost event received");
        ChangeState(GameStates.GameOver);
    }

    public void ChangeState(GameStates newState)
    {
        if (activeTransition != null)
        {
            StopCoroutine(activeTransition);
            activeTransition = null;
        }

        activeTransition = StartCoroutine(TransitionState(newState));
    }

    private IEnumerator TransitionState(GameStates newState)
    {
        Debug.Log($"GameStateManager: Starting transition from {currentState} to {newState}");
        isTransitioning = true;

        // Exit old state
        OnStateExit?.Invoke(currentState);
        yield return HandleStateExit(currentState);

        // Enter new state
        currentState = newState;
        OnStateEnter?.Invoke(currentState);
        yield return HandleStateEnter(newState);

        isTransitioning = false;
        activeTransition = null;
        Debug.Log($"GameStateManager: Completed transition to {currentState}");
    }

    private IEnumerator HandleStateExit(GameStates state)
    {
        switch (state)
        {
            case GameStates.LevelChanging:
                if (levelManager) levelManager.HideLevelText();
                break;
        }
        yield return null;
    }

    private IEnumerator HandleStateEnter(GameStates state)
    {
        switch (state)
        {
            case GameStates.BallIdle:
                if (ball) ball.ResetBall();
                break;

            case GameStates.GameLoop:
                break;

            case GameStates.LevelChanging:
                yield return HandleLevelChange();
                break;

            case GameStates.GameOver:
                if (gameOverState) gameOverState.InitiateGameOver();
                break;
        }
    }

    private IEnumerator HandleLevelChange()
    {
        if (ball) ball.ResetBall();

        if (levelManager)
        {
            levelManager.IncrementLevel();
            levelManager.ShowLevelText();

            yield return new WaitForSeconds(levelTransitionDelay);

            levelManager.HideLevelText();
            levelManager.InitiateBlocks();

            // Now change to BallIdle state
            ChangeState(GameStates.BallIdle);
        }
    }

    // Public methods for other components to notify events
    public void NotifyBallLaunched()
    {
        if (currentState == GameStates.BallIdle)
        {
            ChangeState(GameStates.GameLoop);
        }
    }

    public void NotifyBallDestroyed()
    {
        OnBallDestroyed?.Invoke();
    }
}

public enum GameStates
{
    BallIdle,   // Ball is attached to paddle
    GameLoop,    // Ball is in play
    LevelChanging,
    GameOver
}