using System;
using System.Collections.Generic;
using UnityEngine;


public class BrickManager : MonoBehaviour
{

    [SerializeField] private List<RegularBrick> brickPrebafs;
    [SerializeField] private ExplodingBrick explodingBrickPrefab;
    [SerializeField] private int LineCount = 6;
    [SerializeField][Range(0f, 1f)] private float explodingBrickProbability = 0.2f;

    private int m_TotalBrick = 0;
    public event Action LevelFinished;

    void Start()
    {
        InitiateBlocks();
    }

    void OnDestroy()
    {
        // Clean up event subscriptions
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (var brick in bricks)
        {
            brick.onDestroyed -= BrickCounter;
        }
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
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                Brick brick;

                if (UnityEngine.Random.value < explodingBrickProbability && explodingBrickPrefab != null)
                {
                    brick = Instantiate(explodingBrickPrefab, position, Quaternion.identity);
                }
                else
                {
                    int randomPrefab = UnityEngine.Random.Range(0, brickPrebafs.Count);
                    brick = Instantiate(brickPrebafs[randomPrefab], position, Quaternion.identity);
                }
                brick.onDestroyed += BrickCounter;
                m_TotalBrick++;



            }
        }

        Debug.Log($"Total Bricks: {m_TotalBrick}");
    }

    void BrickCounter(int point)
    {

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
            brick.onDestroyed -= BrickCounter;
            Destroy(brick.gameObject);
        }

        m_TotalBrick = 0;
    }
}