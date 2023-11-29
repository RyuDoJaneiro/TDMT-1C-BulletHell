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
    [SerializeField] private GameObject chestGameObject;
    [SerializeField] private bool isChestOpen = false;

    public bool IsChestOpen { get { return isChestOpen; } set { isChestOpen = value; } }
    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void StartGame()
    {
        menuObject.SetActive(false);
        Camera.main.gameObject.SetActive(false);
        StartCoroutine(SetupGame());
    }

    private IEnumerator SetupGame()
    {
        healthBar.SetActive(true);
        playerReference.gameObject.SetActive(true);
        playerReference.CharacterCurrentHealth = 100;
        playerReference.isDying = false;
        readyText.gameObject.SetActive(true);
        readyText.text = "¿Listo?";
        yield return new WaitForSeconds(1.5f);
        readyText.text = "¡YA!";
        yield return new WaitForSeconds(0.5f);
        inGameButton.SetActive(true);
        readyText.gameObject.SetActive(false);

        StartCoroutine(StartHorde());
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
            chestGameObject.SetActive(true);
            readyText.gameObject.SetActive(true);
            readyText.text = "¡Oleada superada!";
        }
    }

    public void PlayerDefeat()
    {
        enemySpawner.enabled = false;
        healthBar.SetActive(false);
        mainCamera.SetActive(true);
        defeatObject.SetActive(true);
    }

    private IEnumerator StartHorde()
    {
        hordeNumber++;
        playerReference.CharacterCurrentHealth = playerReference.CharacterMaxHealth;
        readyText.gameObject.SetActive(true);
        readyText.text = $"Oleada N° {hordeNumber}";
        yield return new WaitForSeconds(1.5f);
        readyText.text = "¡Ya!";
        readyText.gameObject.SetActive(false);
        enemySpawner.SpawnEnemyAmount += 3;
        StartCoroutine(enemySpawner.SpawnEnemies());
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
