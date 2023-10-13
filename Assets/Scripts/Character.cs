using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterAnimator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 colliderSize = new(1f, 1f);

    [Header("Character Info")]
    [SerializeField] private string characterName;

    [Header("Character Stats")]
    [SerializeField] private int characterHealth = 100;
    [SerializeField] private float characterSpeed = 8;

    [Header("Character Actions")]
    public bool isMoving = false;
    public bool isDying;  
    private Vector2 lastMovementDirection;

    [Header("Solid Objects Layer")]
    [SerializeField] private LayerMask solidObjectLayer;

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

        Vector3 nextPos =  transform.position + characterSpeed * Time.deltaTime * new Vector3(movementValue.x, movementValue.y);

        if (IsWalkable(nextPos))
            transform.position = nextPos;

        // Store the last movement direction the character was facing
        if (movementValue != Vector2.zero)
        {
            lastMovementDirection = movementValue;
            animator.lastMoveX = movementValue.x;
            animator.lastMoveY = movementValue.y;
        }
    }

    public void HandleUpdate()
    {
        animator.IsMoving = isMoving;
    }

    public IEnumerator Death()
    {
        isDying = true;
        Debug.Log($"{name} is dead");
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        yield return null;
    }

    public void TakeDamage(int damageAmount)
    {
        characterHealth -= damageAmount;
        if (characterHealth <= 0) StartCoroutine(Death());
    }

    private bool IsWalkable(Vector2 nextPos)
    {
        if (Physics2D.OverlapBox(nextPos, colliderSize, solidObjectLayer))
        {
            return false;
        }

        return true;
    }
}
