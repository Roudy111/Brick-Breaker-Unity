using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Ball>())
        {
            Debug.Log("DeathZone: Ball collision detected");
            Destroy(other.gameObject);
            GameStateManager.Instance.NotifyBallDestroyed();
        }
    }
}