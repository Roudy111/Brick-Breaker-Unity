using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int currentLevel { get; private set; } = 1; // the variable to track current Level -- always initialzed at 1 
    private bool isChangingLevel = false; // New flag to prevent multiple coroutines
    
    [SerializeField] private Text LevelText;
    private BrickManager brickManager;
    private Factory brickFactory;

    void OnEnable()
    {
        BrickManager.LevelFinished += OnLevelFinished;




    }

    void Start()
    {
       UpdateLevelText();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        BrickManager.LevelFinished -= OnLevelFinished;



    }
        IEnumerator InitiateNextLevel()
    {
        isChangingLevel = true;
        currentLevel++; // Increment level
        
        UpdateLevelText(); // Update level text
        LevelText.gameObject.SetActive(true); // Show level text

        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        LevelText.gameObject.SetActive(false); // Hide level text
        brickManager.InitiateBlocks(); // Initialize new blocks
        isChangingLevel = false; // Reset the flag
    }
    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"Level {currentLevel}";
        }
    }

    private void OnLevelFinished()
    {
        
        if(!isChangingLevel)
        {
            StartCoroutine(InitiateNextLevel());
            
        }

    }
}
