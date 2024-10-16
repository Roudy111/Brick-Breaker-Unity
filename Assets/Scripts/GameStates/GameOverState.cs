using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverState : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private LevelManager levelManager;
    //for managing the input for Restartgame in update method instead of using new Input System which is overkill for this simple game
    private bool IsGameOver = false;
  

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        //reference to LevelManager
        levelManager = FindObjectOfType<LevelManager>();
        
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }
    private void HandleGameStateChanged(GameStates newState)
    {
        if (newState == GameStates.gameOver)
        {
            InitiateGameOver();
        }
    }


    public void InitiateGameOver()
    {
        IsGameOver = true;
        levelManager.DeleteAllBricks();
        gameOverText.SetActive(true);
        backToMenuButton.SetActive(true);
        // You might want to call a method to update and display the high score here
        // scoreManager.UpdateHighScore();
    }

    public void RestartGame()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
       
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0); // Assuming 0 is your menu scene index
    }

}