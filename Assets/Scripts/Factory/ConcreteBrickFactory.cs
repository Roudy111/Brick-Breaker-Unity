using System.Collections.Generic;
using UnityEngine;

public class ConcreteBrickFactory : Factory
{
    [SerializeField] private List<RegularBrick> brickPrebafs;
    [SerializeField] private ExplodingBrick explodingBrickPrefab;
    [SerializeField][Range(0f, 1f)] private float explodingBrickProbability = 0.2f;
    public override IProduct GetProduct(Vector3 position, Quaternion rotation)
    {
        Brick brick;
        if (Random.value < explodingBrickProbability && explodingBrickPrefab != null)
        {
            brick = Instantiate(explodingBrickPrefab, position, rotation);
        }
        else
        {
            int randomPrefab = Random.Range(0, brickPrebafs.Count);
            brick = Instantiate(brickPrebafs[randomPrefab], position, rotation);
        }

        return brick;
    }
}