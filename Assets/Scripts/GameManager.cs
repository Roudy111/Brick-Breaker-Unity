using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public enum GameStates
{
    ballIdle,
    gameloop,
    levelIsChanging,
    gameOver,


}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameStates state;
    public static event Action<GameStates> OnGameStateChanged;


    public void UpdateGameState(GameStates newstate)
    {
        state = newstate;

        switch (newstate)
        {
            case GameStates.ballIdle:
                break;
            case GameStates.gameloop:
                break;
            case GameStates.levelIsChanging:
                break;
            case GameStates.gameOver:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newstate);
    }

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
    void OnEnable()
    {
        
        levelManager = FindObjectOfType<LevelManager>();

    }

   

    void Update()
    {



    }



   


    



}
