using System;
using UnityEngine;

public abstract class Brick : MonoBehaviour, IProduct
{
    public static event Action<int> BrickDestroyed;
    public int PointValue;
    [SerializeField] protected Color gizmoColor = Color.yellow;
    [SerializeField] protected bool showGizmos = true;
    protected private AudioSource audioSource;
    public bool IsDestroyed { get; private set; } = false;
    public string ProductName { get; set; }
    public virtual void Initialize()
    {
        SetProductName();
        SetupAudioSource();
    }
    protected virtual void Start()
    {
        Initialize();
   


    }
     protected virtual void SetProductName()
    {
        // Set a default product name, can be overridden in derived classes
        ProductName = "Generic Brick";
    }
    protected virtual void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        // Ensure the AudioSource is set up correctly
        audioSource.playOnAwake = false;
    }
    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!IsDestroyed)
        {
            DestroyBrick();
        }
    }
    public virtual void DestroyBrick()
    {
        if (!IsDestroyed)
        {
            BrickDestroyed?.Invoke(PointValue);
            IsDestroyed = true;
            Destroy(gameObject, 0.1f);
        }
    }
    protected virtual void OnDrawGizmosSelected()
    {
        DrawGizmos();
    }
    protected virtual void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            DrawGizmos();
        }
    }
    protected virtual void DrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}