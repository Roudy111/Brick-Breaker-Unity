using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverState : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void InitiateGameOver()
    {
        Debug.Log("GameOverState: Initiating game over sequence");
        levelManager?.DeleteAllBricks();
        gameOverText.SetActive(true);
        backToMenuButton.SetActive(true);
    }

    void Update()
    {
        if (GameStateManager.Instance.CurrentState == GameStates.GameOver
            && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        Debug.Log("GameOverState: Restarting game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Debug.Log("GameOverState: Returning to menu");
        SceneManager.LoadScene(0);
    }
}