using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// Manages level-related functionality including brick layout, level progression, and level state.
/// Implements Factory pattern for brick creation and follows Single Responsibility Principle
/// for level management tasks.
/// 
/// Key responsibilities:
/// - Manages level progression and counting
/// - Handles brick initialization and layout
/// - Controls level transition UI and timing
/// - Manages brick cleanup between levels
/// 
/// Design patterns used:
/// - Factory Pattern for brick creation / to give a base for furthur development possiblites
/// - Observer Pattern for level completion events
/// 
/// Dependencies:
/// - Requires ConcreteBrickFactory for brick instantiation
/// - Uses Counter system for brick tracking
/// - Interfaces with GameManager for state changes
/// </summary>
public class LevelManager : MonoBehaviour
{


    // Current level number, encapsulated with read-only access
    public static int currentLevel { get; private set; } = 1; // the field to track current Level -- always initialzed at 1 

    // UI element for displaying current level
    [SerializeField] private Text LevelText;

    // Number of brick rows to create
    [SerializeField] private int LineCount = 6;

    //reference to brick
    [SerializeField] private ConcreteBrickFactory brickFactory;

    private Ball ball;

   
    
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
        // reference to ball object
        ball = FindObjectOfType<Ball>();

    }

    /// <summary>
    /// Event handler for when a level is completed.
    /// Initiates the transition to the next level if not already changing levels.
    /// </summary>
    private void OnLevelFinished()
    {
        
            StartCoroutine(InitiateNextLevel());
        
    }
    /// <summary>
    /// Coroutine to handle the transition to the next level.
    /// </summary>
    IEnumerator InitiateNextLevel()
    {
        GameManager.instance.UpdateGameState(GameStates.levelIsChanging);
        ball.ResetBall(); // Reset ball position
        currentLevel++; // Increment level
        
        UpdateLevelText(); // Update level text

        LevelText.gameObject.SetActive(true); // Show level text

        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        LevelText.gameObject.SetActive(false); // Hide level text

        InitiateBlocks(); // Initialize new blocks

        //Debug.Log("About to change state to GameLoop");
        //Debug.Log($"Current Game State: {GameManager.instance.state} before calling UpdateGameState to GameLoop");
        
        GameManager.instance.UpdateGameState(GameStates.idle);
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

    }



    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
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
