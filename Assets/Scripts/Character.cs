using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterAnimator))]
public class Character : MonoBehaviour
{
    protected CharacterAnimator animator;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 colliderSize = new(1f, 1f);

    [Header("Character Info")]
    [SerializeField] private string characterName;
    [SerializeField] private int isEnemyOf;

    [Header("Character Stats")]
    [SerializeField] protected int characterHealth = 100;
    [SerializeField] private float characterSpeed = 8;
    [SerializeField] protected int characterAttackValue = 1;

    [Header("Character Actions")]
    public bool isMoving = false;
    public bool isDying;  

    [Header("Solid Objects Layer")]
    [SerializeField] private LayerMask solidObjectLayer;

    [Header("Bullet prefab")]
    [SerializeField] private GameObject bulletPrefab;
 
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<CharacterAnimator>();
    }

    public void MoveCharacter(Vector2 movementValue)
    {
        if (isDying)
            return;

        animator.MoveX = Mathf.Clamp(movementValue.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(movementValue.y, -1f, 1f);

        Vector2 nextPos =  transform.position + characterSpeed * Time.deltaTime * new Vector3(movementValue.x, movementValue.y);

        if (IsWalkable(nextPos))
            transform.position = nextPos;

        // Store the last movement direction the character was facing
        if (movementValue != Vector2.zero)
        {
            animator.LastMoveX = movementValue.x;
            animator.LastMoveY = movementValue.y;
        }
    }

    public void HandleUpdate()
    {
        animator.IsMoving = isMoving;
        animator.IsDying = isDying;
    }

    public IEnumerator Death()
    {
        if (gameObject.layer == 7) GameObject.Find("Player").GetComponent<PlayerController>().EliminationCount++;
        isDying = true;
        Debug.Log($"{name} is dead");
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator Shoot(Vector2 direction)
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        spawnedBullet.TryGetComponent<Projectile>(out var component);
        component.ProjectileDirection = direction;
        component.AttackValue = characterAttackValue;
        component.Objetive = isEnemyOf;

        yield return new WaitForSeconds(8f);
        Destroy(spawnedBullet);
    }

    public void TakeDamage(int damageAmount)
    {
        characterHealth -= damageAmount;
        if (characterHealth <= 0) StartCoroutine(Death());
    }

    private bool IsWalkable(Vector2 nextPos)
    {


        Collider2D[] colliders = Physics2D.OverlapBoxAll(nextPos, colliderSize, solidObjectLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != this.gameObject && gameObject.layer != 7)
            {
                return false;
            }
        }

        return true;
    }
}
