using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    public int currentLevel { get; private set; } = 1; // the variable to track current Level -- always initialzed at 1 
    private bool isChangingLevel = false; // New flag to prevent multiple coroutines
    
    [SerializeField] private Text LevelText;

    [SerializeField] private ConcreteBrickFactory brickFactory;
    [SerializeField] private int LineCount = 6;
    private int m_TotalBrick = 0;
    public event Action LevelFinished;

    void OnEnable()
    {
        
        LevelFinished +=OnLevelFinished;





    }

    void Start()
    {
       UpdateLevelText();
       // subscribe to bricks destruction event for couting the Bricks & adding score
        Brick.BrickDestroyed += BrickCounter;

        if (brickFactory == null)
        {
            Debug.LogError("BrickFactory is not set in BrickManager!");
        }
        InitiateBlocks();
        
    }
    public void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_TotalBrick = 0;
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                IProduct product = brickFactory.GetProduct(position, Quaternion.identity);
                if (product is Brick brick)
                {
                    m_TotalBrick++;
                }
                else
                {
                    Debug.LogError("Product created is not a Brick!");
                }
            }
        }
        Debug.Log($"Total Bricks: {m_TotalBrick}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        LevelFinished -=OnLevelFinished;
        Brick.BrickDestroyed -= BrickCounter;




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
        void BrickCounter(int point)
    {
        Debug.Log($"pointValue : {point}");
        m_TotalBrick--;
        Debug.Log($"Total Brick: {m_TotalBrick}");
        if (m_TotalBrick == 0)
        {
            LevelFinished?.Invoke();
        }
    }

    public void DeleteAllBricks()
    {
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (var brick in bricks)
        {
            Destroy(brick.gameObject);
        }
        m_TotalBrick = 0;
    }

    private void OnLevelFinished()
    {
        
        if(!isChangingLevel)
        {
            StartCoroutine(InitiateNextLevel());
            
        }

    }
}
