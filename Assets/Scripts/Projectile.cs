using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Vector2 projectileDirection;
    private int attackValue;
    private int objetive;

    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }
    public Vector2 ProjectileDirection { get { return projectileDirection; } set { projectileDirection = value; } }
    public int AttackValue { get { return attackValue; } set { attackValue = value; } }
    public int Objetive { get { return objetive; } set { objetive = value; } }

    public void Start()
    {
        Vector2 normalizedDirection = projectileDirection.normalized;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(normalizedDirection.y, normalizedDirection.x) * Mathf.Rad2Deg) - 45));

        GetComponent<Rigidbody2D>().velocity = normalizedDirection * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == objetive)
        {
            other.gameObject.GetComponent<Character>().TakeDamage(attackValue);
        }
    }
}
