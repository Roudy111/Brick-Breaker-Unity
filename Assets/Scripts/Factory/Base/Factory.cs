using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Factory base class implementing the Factory Method pattern.
/// This design enables flexible product creation and supports future game expansion.
/// 
/// Key benefits:
/// - Allows addition of new product types without modifying existing code
/// - Decouples product creation from product usage
/// - Supports interface-based product variations
/// 
/// Future extensibility:
/// - New product types can implement different interfaces
/// - Products can have independent behaviors and properties
/// - Supports runtime product type decisions
/// 
/// Design Pattern: Factory Method Pattern
/// </summary>
public abstract class Factory : MonoBehaviour
{
    /// <summary>
    /// Creates a new product instance at the specified position and rotation.
    /// Each concrete factory can implement its own creation logic.
    /// </summary>
    /// <param name="position">World position for the new product</param>
    /// <param name="rotation">Initial rotation for the new product</param>
    /// <returns>A new product instance implementing IProduct</returns>
    public abstract IProduct GetProduct(Vector3 position, Quaternion rotation);
}