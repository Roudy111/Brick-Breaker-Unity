using System;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    
    public static int currentLevel { get; private set; } = 1;
    [SerializeField] private int LineCount = 6;

    [Header("UI Components")]
    [SerializeField] private Text LevelText;

    [Header("Factory")]
    [SerializeField] private ConcreteBrickFactory brickFactory;

    private void Start()
    {
        Debug.Log("LevelManager: Initializing");
        if (brickFactory == null)
        {
            Debug.LogError("LevelManager: BrickFactory not set!");
            return;
        }

        UpdateLevelText();
        InitiateBlocks();
    }

    public void IncrementLevel()
    {
        currentLevel++;
        Debug.Log($"LevelManager: Level incremented to {currentLevel}");
        UpdateLevelText();
    }

    public void ShowLevelText()
    {
        if (LevelText != null)
        {
            LevelText.gameObject.SetActive(true);
            Debug.Log($"LevelManager: Showing level text for level {currentLevel}");
        }
    }

    public void HideLevelText()
    {
        if (LevelText != null)
        {
            LevelText.gameObject.SetActive(false);
            Debug.Log("LevelManager: Hiding level text");
        }
    }

    private void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
            Debug.Log($"LevelManager: Level text updated to: {LevelText.text}");
        }
    }

    public void InitiateBlocks()
    {
        Debug.Log($"LevelManager: Initiating blocks for level {currentLevel}");

        if (brickFactory == null)
        {
            Debug.LogError("LevelManager: Cannot initiate blocks - BrickFactory is null");
            return;
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        Counter.m_TotalBrick = 0;

        // Calculate starting position for centered blocks
        Vector3 startPosition = new Vector3(-1.5f, 2.5f, 0);

        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = startPosition + new Vector3(step * x, i * 0.3f, 0);
                IProduct product = brickFactory.GetProduct(position, Quaternion.identity);

                if (product is Brick)
                {
                    Counter.m_TotalBrick++;
                }
                else
                {
                    Debug.LogError("LevelManager: Product created is not a Brick!");
                }
            }
        }

        // Scale difficulty with level (optional)
        AdjustDifficultyForLevel();

        Debug.Log($"LevelManager: Created {Counter.m_TotalBrick} bricks for level {currentLevel}");
    }

    private void AdjustDifficultyForLevel()
    {
        // Optional: Add difficulty scaling logic here
        // For example:
        // - Increase brick health
        // - Add more exploding bricks
        // - Change brick patterns
        // - Adjust ball speed
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

#if UNITY_EDITOR
    // Editor-only validation
    private void OnValidate()
    {
        if (LineCount <= 0)
        {
            Debug.LogWarning("LevelManager: LineCount must be greater than 0");
            LineCount = 1;
        }
    }

    private void Reset()
    {
        // Set default values when component is first added
        LineCount = 6;
    }
#endif
}