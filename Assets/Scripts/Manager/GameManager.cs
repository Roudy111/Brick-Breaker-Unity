using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;

public enum GameStates
{
    ballIdle,
    ballMove,
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
            case GameStates.ballMove:
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

    [SerializeField] private Text LevelText;

    public int currentLevel { get; private set; } = 1; // the variable to track current Level -- always initialzed at 1 
    private bool isChangingLevel = false; // New flag to prevent multiple coroutines

    private bool m_Started = false;
    
    private bool m_GameOver = false;
    [SerializeField]
    private GameObject backToMenu;

    private ScoreManager scoreManager;
    private BrickManager brickManager;

    

    void Start()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

       UpdateGameState(GameStates.ballIdle);
       UpdateLevelText();



    }
    void OnEnable()
    {
        brickManager = FindObjectOfType<BrickManager>();
        brickManager.LevelFinished += OnLevelFinished;


    }

   
    void OnDestroy()
    {
        brickManager.LevelFinished -= OnLevelFinished;
       
        
        
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

    


    private void OnLevelFinished()
    {
        if(FindObjectsOfType<Brick>().Length == 0 && !isChangingLevel)
        {
            StartCoroutine(InitiateNextLevel());

        }

    }

    IEnumerator InitiateNextLevel()
    {
        isChangingLevel = true;
        currentLevel++; // Increment level
        
        UpdateLevelText(); // Update level text
        LevelText.gameObject.SetActive(true); // Show level text

        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        LevelText.gameObject.SetActive(false); // Hide level text
        brickManager.InitiateBlocks(); // Initialize new blocks
        isChangingLevel = false; // Reset the flag
    }
    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
        }
    }
    
    
   
 




    

    void StartGame()
    {
        m_Started = true;
        float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    public void GameOver()
    {
        m_GameOver = true;
        brickManager.DeleteAllBricks();
        GameOverText.SetActive(true);
        //UpdateHighscoreText();
        backToMenu.SetActive(true);
        
        
        
    }


    

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    }

}
