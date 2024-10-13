using UnityEngine;
using System;

public abstract class Brick : MonoBehaviour
{
    public event Action<int> onDestroyed;

    public int PointValue;
    [SerializeField] protected Color gizmoColor = Color.yellow;
    [SerializeField] protected bool showGizmos = true;

    protected bool isDestroyed = false;
    protected Renderer brickRenderer;
    protected MaterialPropertyBlock materialPropertyBlock;

    protected virtual void Start()
    {
        InitializeRenderer();
        SetBrickColor();
    }

    protected void InitializeRenderer()
    {
        brickRenderer = GetComponentInChildren<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    protected void SetBrickColor()
    {
        Color brickColor = GetColorByPointValue();
        materialPropertyBlock.SetColor("_BaseColor", brickColor);
        brickRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    protected virtual Color GetColorByPointValue()
    {
        switch (PointValue)
        {
            case 1:
                return Color.magenta;
            case 2:
                return Color.yellow;
            case 5:
                return Color.blue;
            default:
                return Color.red;
        }
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