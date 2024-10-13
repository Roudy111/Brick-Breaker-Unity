using UnityEngine;

public class RegularBrick : Brick
{
    [SerializeField] private AudioClip clickSFX;

    private void PlayClickSound()
    {
        if (clickSFX != null && audioSource != null)
        {
            AudioSource.PlayClipAtPoint(clickSFX, transform.position);
        }
        else
        {
            Debug.LogError("Failed to play explosion sound. AudioClip or AudioSource is missing.");
        }
    }
    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        PlayClickSound();
    }

}