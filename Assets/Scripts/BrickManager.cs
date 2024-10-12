using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{

    [SerializeField]
    public Brick BrickPrefab;
    [SerializeField]
    public int LineCount = 6;
    private int m_TotalBrick = 0;
    public event Action LevelFinished;


    // Start is called before the first frame update
    void Start()
    {
        InitiateBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onDestroyed()
    {
        BrickPrefab.onDestroyed -= AddPoint;
    }

    public void InitiateBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_TotalBrick = 0;

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed += AddPoint;
                m_TotalBrick++;
            }
            
        }
        Debug.Log($"Total Bricks : {m_TotalBrick} ");
        
    }
    void AddPoint(int point)
    {
        ScoreManager.Instance.AddPoints(point);
        m_TotalBrick--;
        if(m_TotalBrick == 0)
        {
            LevelFinished?.Invoke();

        }
    }
    public void DeleteAllBricks()
    {
        // Find all brick objects in the scene
        Brick[] bricks = FindObjectsOfType<Brick>();
        
        // Destroy each brick
        foreach (var brick in bricks)
        {
            Destroy(brick.gameObject);
        }

        // Reset the brick count
        m_TotalBrick = 0;
    }
}
