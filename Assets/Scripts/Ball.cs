// Ball.cs
using UnityEngine;
using System;
using System.Collections;

public class Ball : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 initialLocalPosition;
    private Transform paddleTransform;
    private bool isResetting = false;

    [Header("Ball Physics")]
    [SerializeField] private float maxVelocity = 3.0f;
    [SerializeField] private float accelerationFactor = 0.01f;
    [SerializeField] private float verticalBias = 0.5f;
    [SerializeField] private float launchForce = 2.0f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        paddleTransform = transform.parent;
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        // Only check for launch in BallIdle state
        if (GameManager.instance.state == GameStates.BallIdle && Input.GetKeyDown(KeyCode.Space))
        {
            LaunchBall();
        }
    }

    public void LaunchBall()
    {
        if (isResetting || !m_Rigidbody)
        {
            Debug.Log("Ball: Cannot launch - ball is resetting or no Rigidbody found");
            return;
        }

        Debug.Log("Ball: Launching ball");

        // Ensure we're not kinematic before applying force
        m_Rigidbody.isKinematic = false;

        // Detach from paddle
        transform.SetParent(null);

        // Calculate launch direction
        float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;

        // Apply force
        m_Rigidbody.AddForce(forceDir * launchForce, ForceMode.VelocityChange);

        // Update game state
        GameManager.instance.UpdateGameState(GameStates.GameLoop);

        Debug.Log($"Ball: Launched with force {forceDir * launchForce}");
    }

    public void ResetBall()
    {
        if (isResetting) return;
        isResetting = true;

        Debug.Log("Ball: Starting reset");

        if (m_Rigidbody)
        {
            m_Rigidbody.isKinematic = true;
            m_Rigidbody.velocity = Vector3.zero;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }

        transform.SetParent(paddleTransform);
        transform.localPosition = initialLocalPosition;

        isResetting = false;
        Debug.Log("Ball: Reset complete");
    }

    private void OnCollisionExit(Collision other)
    {
        if (!m_Rigidbody || m_Rigidbody.isKinematic) return;
        AdjustVelocityAfterCollision();
    }

    private void AdjustVelocityAfterCollision()
    {
        var velocity = m_Rigidbody.velocity;
        velocity += velocity.normalized * accelerationFactor;

        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * verticalBias : Vector3.down * verticalBias;
        }

        if (velocity.magnitude > maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        m_Rigidbody.velocity = velocity;
    }
}