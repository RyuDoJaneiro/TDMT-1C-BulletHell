using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    public void OnObjectSpawn();
}

public class Projectile : MonoBehaviour, IPooledObject
{
    private float projectileSpeed;
    public Vector2 projectileDirection;

    public float ProjectileSpeed { get { return projectileSpeed; } set {  projectileSpeed = value; } }
    public Vector2 ProjectileDirection { get {  return projectileDirection; } set { projectileDirection = value; } }

    public void OnObjectSpawn()
    {
        
    }

    private void Start()
    {
        Vector2 direction = new Vector2(projectileDirection.x - transform.position.x, projectileDirection.y - transform.position.y).normalized;

        GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
