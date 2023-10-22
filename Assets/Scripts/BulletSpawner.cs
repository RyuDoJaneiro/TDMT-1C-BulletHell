using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    [SerializeField] private float projectileSpeed;
    private Vector2 projectileDirection;

    public Vector2 ProjectileDirection { get { return projectileDirection; } set { projectileDirection = value; } }

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    public void ShootProjectile()
    {
        GameObject projectile = objectPooler.SpawnFromPool("Fire", transform.position, Quaternion.identity);
        projectile.TryGetComponent<Projectile>(out var spawnedProjectile);

        spawnedProjectile.ProjectileSpeed = projectileSpeed;
        spawnedProjectile.projectileDirection = projectileDirection;
    }
}
