using UnityEngine;

/// <summary>
/// Handles all input states and controls for the game
/// Implements singleton pattern for global access
/// </summary>
public class InputHandler : singleton<InputHandler>
{


    private bool isSpaceEnabled = true;
    private bool isPaddleMovementEnabled = true;


    private void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.idle:
                // Ball attached to paddle, waiting for launch
                isSpaceEnabled = true;
                isPaddleMovementEnabled = true;
                break;

            case GameStates.gamePlay:
                // Ball in motion, playing state
                isSpaceEnabled = false;
                isPaddleMovementEnabled = true;
                break;

            case GameStates.levelIsChanging:
                isSpaceEnabled = false;
                isPaddleMovementEnabled = false;
                break;

            case GameStates.gameOver:
                isSpaceEnabled = false;
                isPaddleMovementEnabled = false;
                break;
        }
    }

    public bool IsSpaceAllowed()
    {
        return isSpaceEnabled && Input.GetKeyDown(KeyCode.Space);
    }

    public float GetPaddleMovement()
    {
        return isPaddleMovementEnabled ? Input.GetAxis("Horizontal") : 0f;
    }
}