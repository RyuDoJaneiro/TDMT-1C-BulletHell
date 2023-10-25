using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private Vector2 rectangleArea = new Vector2 (10f, 10f);
    [SerializeField] private float borderDistance = 1f;
    [SerializeField] private int spawnedEnemyAmount = 0;
    [SerializeField] private float timeBeetweenSpawn = 3;
    private float spawnTimer;
    private int i = 0;
    private PlayerController playerReference;


    private void Start()
    {
        playerReference = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (i < spawnedEnemyAmount)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer > timeBeetweenSpawn)
            {
                GenerateEnemy();
                spawnTimer = 0f;
                i++;
                VerifyVictory();
            }
        }
        
    }

    private void VerifyVictory()
    {
        if (playerReference == null)
            return;

        if (playerReference.EliminationCount >= spawnedEnemyAmount)
        {
            Debug.Log("EL JUGADOR GANO");
        }

    }

    private void GenerateEnemy()
    {
        GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        bool InHorizontalBorder = Random.Range(0f, 1f) < 0.5f;

        float posX = 0f;
        float posY = 0f;

        if (InHorizontalBorder)
        {
            posX = Random.Range(-rectangleArea.x / 2f, rectangleArea.x / 2f);
            posY = (Random.Range(0f, 1f) < 0.5f) ? -rectangleArea.y / 2f : rectangleArea.y / 2f;
            posY += (posY < 0) ? -borderDistance : borderDistance;
        }
        else
        {
            posX = (Random.Range(0f, 1f) < 0.5f) ? -rectangleArea.x / 2f : rectangleArea.x / 2f;
            posX += (posX < 0) ? -borderDistance : borderDistance;
            posY = Random.Range(-rectangleArea.y / 2f, rectangleArea.y / 2f);
        }

        Vector3 randomPosition = new Vector2(posX, posY);

        Instantiate(randomEnemy, randomPosition, Quaternion.identity);
    }
}
