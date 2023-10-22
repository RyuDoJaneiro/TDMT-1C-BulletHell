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
        Shoot
    }

    [Header("Enemy Behaviour")]
    [SerializeField] private EnemyBehaviour behaviourType;
    [SerializeField] private float minDistance = 0.1f;
    [SerializeField] private int attackValue = 1;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    private float timer;
    private Transform playerReference;
    private Vector3 playerPosition;
    private BulletSpawner bulletSpawnerReference;

    private void Awake()
    {
        playerReference = GameObject.Find("Player").GetComponent<Transform>();
        bulletSpawnerReference = GetComponent<BulletSpawner>();
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
                        playerReference.GetComponent<PlayerController>().TakeDamage(attackValue);
                        timer = 0f;
                    }
                    break;
                case EnemyBehaviour.Shoot:
                    if (timer > timeBetweenAttacks)
                    {
                        if (!bulletSpawnerReference)
                        {
                            Debug.LogError($"{name}: Buller spawner is null!");
                            break;
                        }

                        bulletSpawnerReference.ProjectileDirection = new Vector2(playerPosition.x, playerPosition.y);
                        bulletSpawnerReference.ShootProjectile();
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
            playerReference.GetComponent<PlayerController>().TakeDamage(attackValue);
    }

}
