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


    void Start()
    {
        // Set initial game state to idle, waiting for player input
        UpdateGameState(GameStates.idle);
    }


    public void Awake()
    {
        instance = this;
    }

    //State machine for game states that has been defined in enum.
    // Handle of gameState has been distrubuted to different classes because it could be overkilled for such a simple game
    public void UpdateGameState(GameStates newstate)
    {
        state = newstate;
        Debug.Log($"Game State Updated: {newstate}");

        // Notify all subscribers about the state change
        OnGameStateChanged?.Invoke(newstate);
    }

}
public enum GameStates
{
    idle,
    gamePlay,
    levelIsChanging,
    gameOver,


}
