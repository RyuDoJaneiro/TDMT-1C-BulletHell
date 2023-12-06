using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
    [SerializeField] private GameObject mapVariationsContainer;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private GameObject portal;
    [SerializeField] private GameObject slimeBoss;
    [SerializeField] private Slider slimeHealthBar;
    [SerializeField] private GameObject slimeHealthObject;
    [SerializeField] private GameObject victoryPanel;
    private List<GameObject> mapVariations = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < mapVariationsContainer.transform.childCount; i++)
        {
            mapVariations.Add(mapVariationsContainer.transform.GetChild(i).transform.gameObject);
        }
        enemySpawner = GetComponent<EnemySpawner>();
    }

    private void Update()
    {
        if (slimeBoss.activeInHierarchy == true)
            slimeHealthBar.value = slimeBoss.GetComponent<Enemy>().CharacterCurrentHealth;
    }

    public void StartGame()
    {
        ResetGame();

        enemySpawner.enabled = false;
        menuObject.SetActive(false);
        Camera.main.gameObject.SetActive(false);
        tutorial.SetActive(true);
        healthBar.SetActive(true);
        playerReference.gameObject.SetActive(true);
        inGameButton.SetActive(true);
        readyText.gameObject.SetActive(false);
        portal.SetActive(true);
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
            readyText.gameObject.SetActive(true);
            readyText.text = "¡Oleada superada!";
            portal.SetActive(true);
        }
        else if (slimeBoss.GetComponent<Enemy>().isDying == true)
        {
            victoryPanel.SetActive(true);
            mainCamera.SetActive(true);
            ResetGame();
        }
    }

    public void PlayerDefeat()
    {
        ResetGame();

        enemySpawner.enabled = false;
        mainCamera.SetActive(true);
        defeatObject.SetActive(true);
    }

    private IEnumerator StartHorde()
    {
        hordeNumber++;
        enemySpawner.enabled = true;
        playerReference.CharacterCurrentHealth = playerReference.CharacterMaxHealth;
        playerReference.EliminationCount = 0;

        if (hordeNumber >= 3)
        {
            slimeBoss.SetActive(true);
            slimeBoss.GetComponent<Enemy>().CharacterCurrentHealth = slimeBoss.GetComponent<Enemy>().CharacterMaxHealth;
            slimeHealthObject.SetActive(true);
            readyText.gameObject.SetActive(true);
            readyText.text = "Jefe Final";
            yield return new WaitForSeconds(1.5f);
            readyText.gameObject.SetActive(false);
        }
        else
        {
            readyText.gameObject.SetActive(true);
            readyText.text = $"Nivel N° {hordeNumber}";
            yield return new WaitForSeconds(1.5f);
            readyText.gameObject.SetActive(false);
            enemySpawner.SpawnEnemyAmount += 3;
            StartCoroutine(enemySpawner.SpawnEnemies());
        }
        
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
        slimeHealthObject.SetActive(false);

        ResetGame();
    }

    public void VictoryToMenu()
    {
        healthBar.SetActive(false);
        playerReference.gameObject.SetActive(false);
        mainCamera.SetActive(true);
        menuObject.SetActive(true);
        victoryPanel.SetActive(false);
        slimeHealthObject.SetActive(false);

        ResetGame();
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

    public IEnumerator NextLevel()
    {
        tutorial.SetActive(false);
        playerReference.CharacterSpeed = 0;

        loadingPanel.SetActive(true);

        playerReference.GetComponent<Transform>().position = Vector3.zero;
        foreach (GameObject mapVariation in mapVariations)
        {
            mapVariation.SetActive(false);
        }
        mapVariations[UnityEngine.Random.Range(0, mapVariations.Count)].SetActive(true);

        yield return new WaitForSeconds(2f);
        playerReference.CharacterSpeed = 8;

        loadingPanel.SetActive(false);

        StartCoroutine(StartHorde());

        yield return null;
    }

    public void ResetGame()
    {
        hordeNumber = 0;
        enemySpawner.SpawnEnemyAmount = 0;
        playerReference.gameObject.SetActive(false);
        slimeHealthObject.SetActive(false);
        healthBar.SetActive(false);
        playerReference.transform.position = Vector2.zero;
        playerReference.CharacterCurrentHealth = 100;
        playerReference.isDying = false;
        slimeBoss.GetComponent<Enemy>().isDying = false;
        playerReference.EliminationCount = 0;
        portal.SetActive(false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.name == "SlimeBoss")
                enemy.SetActive(false);
            else
                Destroy(enemy);
        }
    }
}
