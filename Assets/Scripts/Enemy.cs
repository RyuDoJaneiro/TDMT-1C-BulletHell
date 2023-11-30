using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    public enum EnemyBehaviour
    {
        Explode,
        Chase,
        Shoot,
        Boss
    }

    [Header("Enemy Behaviour")]
    [SerializeField] private EnemyBehaviour behaviourType;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    private float timer;
    private Transform playerReference;
    private Vector3 playerPosition;

    private void Awake()
    {
        playerReference = GameObject.Find("Player").GetComponent<Transform>();
        animator = GetComponent<CharacterAnimator>();
    }

    private void Update()
    {
        playerPosition = playerReference.position;
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        NearPlayer();
        HandleUpdate();
    }

    private void NearPlayer()
    {
        if (playerReference == null)
        {
            Debug.LogError("Player doesn't exists");
            return;
        }

        if (Vector2.Distance(transform.position, playerReference.position) > minDistance)
        {
            MoveCharacter(new Vector2(playerReference.position.x - transform.position.x, playerReference.position.y - transform.position.y).normalized);
            isMoving = true;
        }
        else
        {
            isMoving = false;

            if (isDying)
                return;

            // Se determina que hará cada enemigo en caso de encontrarse dentro del rango mínimo de distancia
            switch (behaviourType)
            {
                case EnemyBehaviour.Explode:
                    StartCoroutine(Explode());
                    break;
                case EnemyBehaviour.Chase:
                    if (timer > timeBetweenAttacks)
                    {
                        playerReference.GetComponent<PlayerController>().TakeDamage(characterAttackValue);
                        timer = 0f;
                    }
                    break;
                case EnemyBehaviour.Shoot:
                    if (timer > timeBetweenAttacks)
                    {
                        StartCoroutine(Shoot(new Vector2(playerPosition.x - transform.position.x, playerPosition.y - transform.position.y)));
                        timer = 0f;
                    }
                    break;
                case EnemyBehaviour.Boss:
                    if (timer > timeBetweenAttacks)
                    {
                        Vector2[] directions = 
                        {
                            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                            new Vector2(1, 1).normalized, new Vector2(1, -1).normalized,
                            new Vector2(-1, 1).normalized, new Vector2(-1, -1).normalized
                        };

                        foreach (Vector2 direction in directions)
                        {
                            StartCoroutine(Shoot(direction));
                        }

                        timer = 0f;
                    }
                    break;
                default:
                    Debug.LogError("This enemy has not behaviour");
                    break;
            }
        }
    }

    // Explosive enemy logic
    private IEnumerator Explode()
    {
        Debug.Log($"{name}: is about to explode!");
        StartCoroutine(Death());

        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, playerReference.position) < minDistance)
            playerReference.GetComponent<PlayerController>().TakeDamage(characterAttackValue);
    }

}
