using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// LevelManager handles the game's level progression, brick initialization.
    /// It follows the Single Responsibility Principle by focusing solely on level-related tasks based on our current level of game development.
    /// 
    /// Need to decouple UI after GameState implementation
    /// </summary>


    public static int currentLevel { get; private set; } = 1; // the field to track current Level -- always initialzed at 1 
    private bool isChangingLevel = false; // New flag to prevent multiple coroutines
    
    [SerializeField] private Text LevelText;

    // Number of brick rows to create
    [SerializeField] private int LineCount = 6;

    //reference to brick
    [SerializeField] private ConcreteBrickFactory brickFactory;

   
    
    void OnEnable()
    {

        // Subscribes to the LevelFinished event when the script is enabled.
        Counter.LevelFinished += OnLevelFinished;

    }
    void OnDestroy()
    {
        // Unsubscribes from the LevelFinished event when the script is destroyed.
        Counter.LevelFinished -= OnLevelFinished;

    }
    void Start()
    {
       UpdateLevelText();

        if (brickFactory == null)
        {
            Debug.LogError("BrickFactory is not set in BrickManager!");
        }
        InitiateBlocks();
        
    }

    /// <summary>
    /// Creates a grid of bricks for the current level using the brick factory.
    /// </summary>
    public void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        Counter.m_TotalBrick = 0;
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                IProduct product = brickFactory.GetProduct(position, Quaternion.identity);
                if (product is Brick brick)
                {
                    Counter.m_TotalBrick++;
                }
                else
                {
                    Debug.LogError("Product created is not a Brick!");
                }
            }
        }
        Debug.Log($"Total Bricks: {Counter.m_TotalBrick}");
        GameManager.instance.UpdateGameState(GameStates.gameloop);

    }

    /// <summary>
    /// Coroutine to handle the transition to the next level.
    /// </summary>
    IEnumerator InitiateNextLevel()
    {
        GameManager.instance.UpdateGameState(GameStates.levelIsChanging);
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
    /// <summary>
    /// Event handler for when a level is completed.
    /// Initiates the transition to the next level if not already changing levels.
    /// </summary>

    private void OnLevelFinished()
    {

        if (!isChangingLevel)
        {
            StartCoroutine(InitiateNextLevel());
        }

    }
    /// <summary>
    /// Removes all existing bricks from the scene.
    /// </summary>
    public void DeleteAllBricks()
    {
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (var brick in bricks)
        {
            Destroy(brick.gameObject);
        }
        Counter.m_TotalBrick = 0;
    }

   
}
