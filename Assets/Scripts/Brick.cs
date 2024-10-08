using UnityEngine;
using UnityEngine.Events;
using System;

public class Brick : MonoBehaviour
{
    public event Action<int> onDestroyed;
    
    public int PointValue;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private float upwardsModifier = 0.4f;
    [SerializeField] private Color gizmoColor = Color.yellow;
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private LayerMask brickLayer; // New field for brick layer mask

    private bool isExploded = false;
    private Renderer brickRenderer;
    private MaterialPropertyBlock materialPropertyBlock;

    private void Start()
    {
        InitializeRenderer();
        SetBrickColor();
        SetBrickLayerMask();
    }

    private void InitializeRenderer()
    {
        brickRenderer = GetComponentInChildren<Renderer>();
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void SetBrickColor()
    {
        Color brickColor = GetColorByPointValue();
        materialPropertyBlock.SetColor("_BaseColor", brickColor);
        brickRenderer.SetPropertyBlock(materialPropertyBlock);
    }

    private Color GetColorByPointValue()
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

    private void SetBrickLayerMask()
    {
        // Set the layer mask to only include the brick layer (layer 7)
        brickLayer = 1 << 7;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isExploded)
        {
            onDestroyed.Invoke(PointValue);
            if (PointValue == 1)
            {
                ApplyExplosionForce(true);
            }
            isExploded = true;
            Destroy(gameObject, 0.1f);
        }
        onDestroyed?.Invoke(PointValue);
        
        //slight delay to be sure the ball have time to bounce
        Destroy(gameObject, 0.1f);
    }

    private void ApplyExplosionForce(bool isInitialExplosion)
    {
        // Use OverlapSphere with the brick layer mask
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, brickLayer);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
            Brick hitBrick = hit.GetComponent<Brick>();
            if (hitBrick != null && !hitBrick.isExploded)
            {
                if (isInitialExplosion || hitBrick.PointValue != 1)
                {
                    hitBrick.DestroyBrick();
                }
            }
        }
    }

    public void DestroyBrick()
    {
        if (!isExploded)
        {
            onDestroyed.Invoke(PointValue);
            isExploded = true;
            Destroy(gameObject, 0.1f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        DrawExplosionGizmos();
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            DrawExplosionGizmos();
        }
    }

    private void DrawExplosionGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}