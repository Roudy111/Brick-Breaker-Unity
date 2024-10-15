using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static int m_TotalBrick { get; set; } = 0;
    public static event Action LevelFinished;



    private void OnEnable()
    {
        Brick.BrickDestroyed += BrickCounter;



    }

    private void OnDisable()
    {
        Brick.BrickDestroyed -= BrickCounter;

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


}
