using UnityEngine;
using System.Collections;

public interface IHaveCollider
{
    Collider2D Collider2D { get; }
}
public interface IBulletOwner : IHaveCollider
{
    void OnBulletHit(GameObject target);
}