using UnityEngine;

public class ExplodingBrick : Brick
{
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private float upwardsModifier = 0.4f;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private AudioClip explosionSFX;

    private bool hasExploded = false;

    public override void Initialize()
    {
        base.Initialize();
        SetBrickLayerMask();

 
    }

    private void PlayExplosionSound()
    {
        if (explosionSFX != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position);
        }
        else
        {
            Debug.LogError("Failed to play explosion sound. AudioClip or AudioSource is missing.");
        }
    }

    private void SetBrickLayerMask()
    {
        // Set the layer mask to only include the brick layer (layer 7)
        brickLayer = 1 << 7;
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (!IsDestroyed && !hasExploded)
        {
            DestroyBrick();
 
            
        }
    }

    public override void DestroyBrick()
    {
        if (!IsDestroyed && !hasExploded)
        {
            base.DestroyBrick();
            Explode();

        }
    }

    private void Explode()
    {
        hasExploded = true;
        ApplyExplosionForce(true);
        PlayExplosionSound();

    }

    private void ApplyExplosionForce(bool isInitialExplosion)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, brickLayer);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }

            Brick hitBrick = hit.GetComponent<Brick>();
            if (hitBrick != null && !hitBrick.IsDestroyed)
            {
                if (hitBrick is ExplodingBrick explodingBrick)
                {
                    if (isInitialExplosion)
                    {
                        explodingBrick.TriggerExplosion();
                    }
                }
                else
                {
                    hitBrick.DestroyBrick();
                }
            }
        }
    }

    public void TriggerExplosion()
    {
        if (!IsDestroyed && !hasExploded)
        {
            DestroyBrick();
        }
    }

    protected override void DrawGizmos()
    {
        base.DrawGizmos();
        if (!showGizmos) return;
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}