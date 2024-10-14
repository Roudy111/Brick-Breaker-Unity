using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
public class BrickManager : MonoBehaviour
{
    [SerializeField] private ConcreteBrickFactory brickFactory;
    [SerializeField] private int LineCount = 6;
    private int m_TotalBrick = 0;
    public static event Action LevelFinished;
    void Start()
    {
        // subscribe to bricks destruction event for couting the Bricks & adding score
        Brick.BrickDestroyed += BrickCounter;
        Brick.BrickDestroyed += UpdateScore;
        if (brickFactory == null)
        {
            Debug.LogError("BrickFactory is not set in BrickManager!");
        }
        InitiateBlocks();
    }
    void OnDestroy()
    {
        Brick.BrickDestroyed -= BrickCounter;
        Brick.BrickDestroyed -= UpdateScore;
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
    void UpdateScore(int PointValue)
    {
        ScoreManager.Instance.AddPoints(PointValue);
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

}