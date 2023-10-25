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
    [SerializeField] private PlayerController playerReference;
    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void StartMatch()
    {
        menuObject.SetActive(false);
        Camera.main.gameObject.SetActive(false);
        StartCoroutine(SetupMatch());
    }

    private IEnumerator SetupMatch()
    {
        playerReference.gameObject.SetActive(true);
        readyText.gameObject.SetActive(true);
        readyText.text = "¿Listo?";
        yield return new WaitForSeconds(1.5f);
        readyText.text = "¡YA!";
        yield return new WaitForSeconds(0.5f);
        readyText.gameObject.SetActive(false);

        enemySpawner.enabled = true;
    }
}
