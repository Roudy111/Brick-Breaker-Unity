using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/// <summary>
/// Handles the game over state logic and UI presentation.
/// Responsible for managing end-game scenarios and providing options to restart or return to menu.
/// 
/// Key responsibilities:
/// - Displays game over UI elements
/// - Handles restart input (Space key)
/// - Manages level cleanup through LevelManager
/// - Provides navigation options (restart/menu)
/// 
/// Dependencies:
/// - Requires GameManager for state management
/// - Needs LevelManager for brick cleanup
/// - Uses Unity UI system for displaying game over elements
/// </summary>
public class GameOverState : MonoBehaviour
{

    // UI elements for game over state
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private LevelManager levelManager;
    //for managing the input for Restartgame in update method instead of using new Input System which is overkill for this simple game
    private bool IsGameOver = false;    // Tracks if game is in game over state to handle restart input
  

    private void OnEnable()
    {
        // Subscribe to game state changes to detect game over condition
        GameManager.OnGameStateChanged += HandleGameStateChanged;
        //reference to LevelManager
        levelManager = FindObjectOfType<LevelManager>();
        
    }

    private void OnDisable()
    {
        // Unsubscribe to game state changes to detect game over condition

        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
    private void Update()
    {
         // Check for space input only in game over state

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

    // Shows game over UI and cleans up level
    public void InitiateGameOver()
    {
        IsGameOver = true;
        levelManager.DeleteAllBricks();
        gameOverText.SetActive(true);
        backToMenuButton.SetActive(true);
        // You might want to call a method to update and display the high score here
        // scoreManager.UpdateHighScore();
    }
    

    //to restart the game with reloading the active scene
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