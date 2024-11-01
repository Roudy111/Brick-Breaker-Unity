using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverState : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject backToMenuButton;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void InitiateGameOver()
    {
        levelManager.DeleteAllBricks();
        gameOverText.SetActive(true);
        backToMenuButton.SetActive(true);
    }

    void Update()
    {
        // Only handle restart input if game is over
        if (GameManager.instance.state == GameStates.GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}