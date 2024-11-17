using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// - Creation logic can be modified without affecting brick behavior
/// - Maintains separation of concerns for brick creation
/// - Uses probability-based product selection to keep modularity of the levelup process --- If it was supposed to be maintained and developed furthur a in editor interface is needed for design of each level
/// - Supports multiple regular brick variants (Deferent in Color and PointValue)
/// - New brick types can be added by extending Brick class
/// </summary>
public class ConcreteBrickFactory : Factory
{
    // List of regular brick prefabs for variety
    [SerializeField] private List<RegularBrick> regularBrickPrebafs;

    // Special exploding brick prefab
    [SerializeField] private ExplodingBrick explodingBrickPrefab;

    // Probability of creating an exploding brick (0-1 range)
    [SerializeField][Range(0f, 1f)] private float explodingBrickProbability = 0.2f;

    /// <summary>
    /// Creates a new brick instance, randomly choosing between regular and exploding types.
    /// Demonstrates how factory can make runtime decisions about product types.
    /// </summary>
    public override IProduct GetProduct(Vector3 position, Quaternion rotation)
    {
        Brick brick;
        // Randomly decide if this should be an exploding brick
        if (Random.value < explodingBrickProbability && explodingBrickPrefab != null)
        {
            brick = Instantiate(explodingBrickPrefab, position, rotation);
        }
        else
        {
            // Randomly select from regular brick variants
            int randomPrefab = Random.Range(0, regularBrickPrebafs.Count);
            brick = Instantiate(regularBrickPrebafs[randomPrefab], position, rotation);
        }

        return brick;
    }
}