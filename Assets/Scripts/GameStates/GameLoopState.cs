using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopState : MonoBehaviour
{
    [SerializeField]
    private Rigidbody Ball;
    // Start is called before the first frame update




    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.state == GameStates.gameloop)
        {
            StartGame();
        }


    }
    


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
            GameManager.instance.UpdateGameState(GameStates.gameloop);
        }

    }
}