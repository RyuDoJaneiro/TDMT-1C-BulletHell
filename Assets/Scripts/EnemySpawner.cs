using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private Vector2 rectangleArea = new Vector2(10f, 10f);
    [SerializeField] private float borderDistance = 1f;
    [SerializeField] private int spawnEnemyAmount = 3;
    [SerializeField] private float timeBeetweenSpawn = 3;

    public int SpawnEnemyAmount { get { return spawnEnemyAmount; } set { spawnEnemyAmount = value; } }

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

    public IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnEnemyAmount; i++)
        {
            yield return new WaitForSeconds(timeBeetweenSpawn);
            GenerateEnemy();
        }
        yield return null;
    }
}
