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
    levelChange,
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
            case GameStates.levelChange:
                break;
            case GameStates.gameOver:
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(newstate);
    }





    [SerializeField]
    private Rigidbody Ball;

    [SerializeField]
    private GameObject GameOverText;



    private bool m_Started = false;
    
    private bool m_GameOver = false;
    [SerializeField]
    private GameObject backToMenu;

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
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
        }
        if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }


    }

    void StartGame()
    {
        m_Started = true;
        float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    public void GameOver()
    {
        m_GameOver = true;
        levelManager.DeleteAllBricks();
        GameOverText.SetActive(true);
        //UpdateHighscoreText();
        backToMenu.SetActive(true);
        
        
        
    }


    

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    }

}
