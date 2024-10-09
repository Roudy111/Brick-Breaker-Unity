using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


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

    public void UpdateGameStates(GameStates newstate)
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
    }




    [SerializeField]
    public Brick BrickPrefab;
    [SerializeField]
    public int LineCount = 6;
    [SerializeField]
    private Rigidbody Ball;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text HighscoreText;
    [SerializeField]
    private GameObject GameOverText;
    [SerializeField]
    private Text CurrentplayerName;
    [SerializeField] private Text LevelText;

    public int currentLevel { get; private set; } = 1; // the variable to track current Level -- always initialzed at 1 
    private bool isChangingLevel = false; // New flag to prevent multiple coroutines

    private bool m_Started = false;
    private int m_TotalBrick = 0;
    private bool m_GameOver = false;
    [SerializeField]
    private GameObject backToMenu;

    private ScoreManager scoreManager;

    void Start()
    {
       

       

        InitiateBlocks();
        UpdateHighscoreText();
        CurrentPlayerNameSet();
        UpdateLevelText();

    }
    private void OnEnable()
    {
        scoreManager = ScoreManager.Instance;
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged += UpdateScoreText;
            scoreManager.OnHighscoreUpdated += UpdateHighscoreText;
            scoreManager.ResetScore();

        }
        else
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }
        

    }
    void OnDestroy()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreText;
            scoreManager.OnHighscoreUpdated -= UpdateHighscoreText;
        }
        BrickPrefab.onDestroyed -= AddPoint;
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
        else
        {
            // Check for new level in Update instead
            CheckForNewLevel();
        }
    }

    void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_TotalBrick = 0;

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed += AddPoint;
                m_TotalBrick++;
            }
        }
        
    }
    void AddPoint(int point)
    {
        scoreManager.AddPoints(point);
        m_TotalBrick--;



    }
    void CheckForNewLevel()
    {
        // Check if there are no more bricks in the scene and we're not already changing level
        if (m_TotalBrick <= 0 && FindObjectsOfType<Brick>().Length == 0 && !isChangingLevel)
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
        InitiateBlocks(); // Initialize new blocks
        isChangingLevel = false; // Reset the flag
    }
    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
        }
    }
    void DeleteAllBricks()
    {
        // Find all brick objects in the scene
        Brick[] bricks = FindObjectsOfType<Brick>();
        
        // Destroy each brick
        foreach (var brick in bricks)
        {
            Destroy(brick.gameObject);
        }

        // Reset the brick count
        m_TotalBrick = 0;
    }
    
   
 




    void UpdateScoreText(int score)
    {
        ScoreText.text = $"Score : {score}";
    }

    void UpdateHighscoreText()
    {
        if (HighscoreText != null && DataManager.Instance != null)
        {
            string highscores = DataManager.Instance.GetFormattedHighscores();
            HighscoreText.text = $"Highscores:\n{highscores}";
        }
    }

    void StartGame()
    {
        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0);
        forceDir.Normalize();

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    public void GameOver()
    {
        m_GameOver = true;
        DeleteAllBricks();
        GameOverText.SetActive(true);
        UpdateHighscoreText();
        backToMenu.SetActive(true);
        
        
        
    }


    void CurrentPlayerNameSet()
    {
        if (CurrentplayerName != null && DataManager.Instance != null)
        {
            CurrentplayerName.text = $"Player Name: {DataManager.Instance.currentPlayerId}";
        }
    }

    public void Back2Menu()
    {
        SceneManager.LoadScene(0);
    }
}