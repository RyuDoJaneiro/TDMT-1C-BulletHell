using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator StartMatch()
    {
        textMeshProUGUI.text = "¿Listo?";
        yield return new WaitForSeconds(1);
        textMeshProUGUI.text = "¡Ya!";
        yield return null;
    }
}
