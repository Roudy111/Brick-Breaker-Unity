using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Here comes different game states that help to game manager have less responsibilites
/// </summary>

public abstract class GameState
{
    //a protected field that will store the reference to the GameManager
    protected GameManager gameManager;

    //the constructor for the GameState class
    public GameState(GameManager manager)
    {
        this.gameManager = manager;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}

public class StartGame : GameState
{
    public StartGame(GameManager manager) : base(manager)
    {
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}


public class PlayingGame : GameState
{
    public PlayingGame(GameManager manager) : base(manager)
    {
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}

public class GameOver : GameState
{
    public GameOver(GameManager manager) : base(manager)
    {
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}