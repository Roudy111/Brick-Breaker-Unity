using UnityEngine;
using System.Collections;
public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 initialLocalPosition;
    private Transform paddleTransform;

    [SerializeField] private float maxVelocity = 3.0f;
    [SerializeField] private float accelerationFactor = 0.01f;
    [SerializeField] private float verticalBias = 0.5f;

    private Coroutine resetCoroutine;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        paddleTransform = transform.parent;
        initialLocalPosition = transform.localPosition;
    }
    
    void OnEnable()
    {
        // Subscribe to game state changes
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    void OnDestroy()
    {
        // Unsubscribe from game state changes
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void OnCollisionExit(Collision other)
    {
        // Only adjust velocity if the Rigidbody is not kinematic
        if (m_Rigidbody != null && !m_Rigidbody.isKinematic)
        {
            AdjustVelocityAfterCollision();
        }
    }

    private void AdjustVelocityAfterCollision()
    {
        // Safety check to ensure we can modify velocity
        if (m_Rigidbody.isKinematic)
            return;

        var velocity = m_Rigidbody.velocity;

        // After a collision we accelerate a bit
        velocity += velocity.normalized * accelerationFactor;

        // Check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * verticalBias : Vector3.down * verticalBias;
        }

        // Max velocity
        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        // Only set velocity if still not kinematic (double-check for thread safety)
        if (!m_Rigidbody.isKinematic)
        {
            m_Rigidbody.velocity = velocity;
        }
    }

    public void ResetBall()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetBallCoroutine());
    }

    private IEnumerator ResetBallCoroutine()
    {
        if (m_Rigidbody != null)
        {
            // First, make sure the Rigidbody is not kinematic while we reset velocities
            m_Rigidbody.isKinematic = false;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
            
            // Now we can safely make it kinematic
            m_Rigidbody.isKinematic = true;
        }

        // Reset position relative to paddle
        transform.SetParent(paddleTransform);
        transform.localPosition = initialLocalPosition;

        // Wait for the specified delay
        yield return new WaitForSeconds(1);

        // Re-enable physics simulation
        if (m_Rigidbody != null)
        {
            m_Rigidbody.WakeUp();
            m_Rigidbody.isKinematic = false;
        }
    }

    private void HandleGameStateChanged(GameStates newState)
    {
        if (newState == GameStates.levelIsChanging)
        {
            ResetBall();
        }
    }
}