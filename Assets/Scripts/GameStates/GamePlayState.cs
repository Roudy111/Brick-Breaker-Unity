using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages the active gameplay state, specifically handling the ball launch mechanics.
/// Implements the initial game start logic when transitioning from idle to gameplay state.
/// 
/// Key responsibilities:
/// - Handles ball launch input through InputHandler
/// - Controls initial ball trajectory and physics
/// - Manages state transition from idle to gameplay
/// 
/// Note: The current direction calculation could be improved for better gameplay feel
/// 
/// Dependencies:
/// - Requires GameManager for state management
/// - Uses InputHandler singleton for input processing
/// - Needs reference to ball's Rigidbody component
/// </summary>
public class GamePlayState : MonoBehaviour
{
    [SerializeField]
    private Rigidbody Ball;     // Reference to the ball's physics component
    // Start is called before the first frame update




   
    void Update()
    {
         // Check for ball launch input only in idle state

        if (InputHandler.Instance.IsSpaceAllowed() && GameManager.instance.state == GameStates.idle)
        {
            StartGame();
        }


    }
    


    // Initializes gameplay by launching the ball
    void StartGame()
    {
        if (Ball != null)
        {
            float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
            // direction should be improved 
            Vector3 forceDir = new(randomDirection, 1, 0);
            forceDir.Normalize();

            Ball.transform.SetParent(null);
            Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);

            //change to GamePlay state
            GameManager.instance.UpdateGameState(GameStates.gamePlay);
        }

    }
}