using UnityEngine;
using System;

public abstract class Brick : MonoBehaviour
{
    public event Action<int> onDestroyed;

    public int PointValue;
    [SerializeField] protected Color gizmoColor = Color.yellow;
    [SerializeField] protected bool showGizmos = true;

    protected bool isDestroyed = false;


    protected virtual void Start()
    {


    }








    protected virtual void OnCollisionEnter(Collision other)
    {
        if (!isDestroyed)
        {
            DestroyBrick();
        }
    }

    public virtual void DestroyBrick()
    {
        if (!isDestroyed)
        {
            onDestroyed?.Invoke(PointValue);
            isDestroyed = true;
            Destroy(gameObject, 0.1f);
        }
    }

    // Add this new method
    public bool IsDestroyed()
    {
        return isDestroyed;
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