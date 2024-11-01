using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static int currentLevel { get; private set; } = 1;

    [Header("UI Components")]
    [SerializeField] private Text LevelText;

    [Header("Level Settings")]
    [SerializeField] private int LineCount = 6;
    [SerializeField] private float levelTransitionDelay = 5f;
    [SerializeField] private ConcreteBrickFactory brickFactory;

    private bool isChangingLevel = false;

    void OnEnable()
    {
        Debug.Log("LevelManager: Subscribing to LevelFinished event");
        Counter.LevelFinished += OnLevelFinished;
    }

    void OnDestroy()
    {
        Debug.Log("LevelManager: Unsubscribing from LevelFinished event");
        Counter.LevelFinished -= OnLevelFinished;
    }

    void Start()
    {
        UpdateLevelText();
        if (brickFactory == null)
        {
            Debug.LogError("LevelManager: BrickFactory not set!");
            return;
        }
        InitiateBlocks();
    }

    public void InitiateBlocks()
    {
        Debug.Log($"LevelManager: Initiating blocks for level {currentLevel}");
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        Counter.m_TotalBrick = 0;

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                IProduct product = brickFactory.GetProduct(position, Quaternion.identity);
                if (product is Brick)
                {
                    Counter.m_TotalBrick++;
                }
            }
        }
        Debug.Log($"LevelManager: Total Bricks created: {Counter.m_TotalBrick}");
    }

    IEnumerator InitiateNextLevel()
    {
        if (isChangingLevel)
        {
            Debug.Log("LevelManager: Level change already in progress");
            yield break;
        }

        isChangingLevel = true;
        Debug.Log($"LevelManager: Starting transition to level {currentLevel + 1}");

        GameManager.instance.UpdateGameState(GameStates.LevelChanging);
        currentLevel++;

        UpdateLevelText();
        LevelText.gameObject.SetActive(true);

        Debug.Log($"LevelManager: Waiting {levelTransitionDelay} seconds");
        yield return new WaitForSeconds(levelTransitionDelay);

        LevelText.gameObject.SetActive(false);
        InitiateBlocks();

        Debug.Log("LevelManager: Level setup complete, returning to BallIdle state");
        GameManager.instance.UpdateGameState(GameStates.BallIdle);

        isChangingLevel = false;
    }

    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
            Debug.Log($"LevelManager: Updated level text to {LevelText.text}");
        }
    }

    private void OnLevelFinished()
    {
        Debug.Log("LevelManager: Level finished event received");
        if (!isChangingLevel)
        {
            StartCoroutine(InitiateNextLevel());
        }
    }

    public void DeleteAllBricks()
    {
        Debug.Log("LevelManager: Deleting all bricks");
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (var brick in bricks)
        {
            Destroy(brick.gameObject);
        }
        Counter.m_TotalBrick = 0;
        Debug.Log("LevelManager: All bricks deleted");
    }
}