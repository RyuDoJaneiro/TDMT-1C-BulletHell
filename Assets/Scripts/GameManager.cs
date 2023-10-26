using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private GameObject menuObject;
    [SerializeField] private GameObject defeatObject;
    [SerializeField] private PlayerController playerReference;
    [SerializeField] private int hordeNumber = 1;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject creditsObject;
    [SerializeField] private GameObject inGameButton;
    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void StartMatch()
    {
        menuObject.SetActive(false);
        Camera.main.gameObject.SetActive(false);
        enemySpawner.SpawnEnemyAmount = 3;
        StartCoroutine(SetupMatch());
    }

    private IEnumerator SetupMatch()
    {
        healthBar.SetActive(true);
        playerReference.gameObject.SetActive(true);
        playerReference.CharacterHealth = 100;
        playerReference.isDying = false;
        readyText.gameObject.SetActive(true);
        readyText.text = "¿Listo?";
        yield return new WaitForSeconds(1.5f);
        readyText.text = "¡YA!";
        yield return new WaitForSeconds(0.5f);
        inGameButton.SetActive(true);
        readyText.gameObject.SetActive(false);

        enemySpawner.enabled = true;
        enemySpawner.i = 0;
    }

    public void VerifyVictory()
    {
        if (playerReference == null)
        {
            Debug.LogError($"{name}: Player reference is null!");
            return;
        }

        if (playerReference.EliminationCount >= enemySpawner.SpawnEnemyAmount)
        {
            Debug.Log($"{name}: El jugador ganó una ronda");
            StartCoroutine(NextHorde());
        }
    }

    public void PlayerDefeat()
    {
        enemySpawner.enabled = false;
        healthBar.SetActive(false);
        mainCamera.SetActive(true);
        defeatObject.SetActive(true);
    }

    private IEnumerator NextHorde()
    {
        hordeNumber++;
        playerReference.CharacterHealth = 100;
        enemySpawner.enabled = false;
        readyText.gameObject.SetActive(true);
        readyText.text = "¡Ganaste!";
        yield return new WaitForSeconds(1.5f);
        readyText.text = "Siguiente ronda";
        yield return new WaitForSeconds(1f);
        readyText.gameObject.SetActive(false);
        enemySpawner.enabled = true;
        enemySpawner.i = 0;
        enemySpawner.SpawnEnemyAmount += 3;
    }

    public void MatchToMenu()
    {
        enemySpawner.enabled = false;
        healthBar.SetActive(false);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies != null)
        {
            foreach(GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
        mainCamera.SetActive(true);
        playerReference.gameObject.SetActive(false);
        menuObject.SetActive(true);
        inGameButton.SetActive(false);
    }

    public void DefeatToMenu()
    {
        menuObject.SetActive(true);
        defeatObject.SetActive(false);
    }

    public void CreditsToMenu()
    {
        creditsObject.SetActive(false);
        menuObject.SetActive(true);
    }

    public void MenuToCredits()
    {
        creditsObject.SetActive(true);
        menuObject.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
