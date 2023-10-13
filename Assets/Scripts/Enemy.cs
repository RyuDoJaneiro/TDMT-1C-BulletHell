using System.Collections;
using System.Collections.Generic;
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
    private Transform playerReference;

    private void Awake()
    {
        playerReference = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        NearPlayer();
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
            MoveCharacter(new Vector3(playerReference.transform.position.x, playerReference.transform.position.y) - transform.position);
        } 
        else
        {
            if (isDying)
                return;

            // Se determina que hará cada enemigo en caso de encontrarse dentro del rango mínimo de distancia
            switch (behaviourType)
            {
                case EnemyBehaviour.Explode:
                    StartCoroutine(Explode());
                    break;
                default:
                    Debug.LogError("This enemy has not behaviour");
                    break;
            }
        }
    }

    // Explosive enemy death logic
    private IEnumerator Explode()
    {
        Debug.Log($"{name}: About to explode!");
        StartCoroutine(Death());
        yield return new WaitForSeconds(1f);
        playerReference.GetComponent<PlayerController>().TakeDamage(5);
    }
}
