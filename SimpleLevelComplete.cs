using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleLevelComplete : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private float checkDelay = 0.5f;

    [Header("UI")]
    [SerializeField] private GameObject levelCompleteScreen;

    private bool levelFinished = false;

    void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("Level script started!");

        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(false);

        StartCoroutine(CheckEnemiesRoutine());
    }

    IEnumerator CheckEnemiesRoutine()
    {
        while (!levelFinished)
        {
            yield return new WaitForSeconds(checkDelay);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

            Debug.Log("Enemies left: " + enemies.Length);

            if (enemies.Length == 0)
            {
                LevelComplete();
            }
        }
    }

    void LevelComplete()
    {
        if (levelFinished) return;
        levelFinished = true;

        Debug.Log("LEVEL COMPLETE!");

        GameState.IsGameOver = true;

        // show UI
        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(true);

        // stop gameplay
        Time.timeScale = 0f;
    }
}
