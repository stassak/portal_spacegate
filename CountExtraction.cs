using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountExtraction : MonoBehaviour
{

    [SerializeField] private int extractionTime = 180;
    [SerializeField] private MenuPlayer menuPlayer;
    [SerializeField] private GameObject levelCompleteScreen;

    public TextMeshProUGUI timerText;

    private float currentTime;

    void Start()
    {
        currentTime = extractionTime;
    }

    void Update()
    {
        if (GameState.IsGameOver)
        {
            timerText.gameObject.SetActive(false);
            return;
        }

        currentTime -= Time.deltaTime;
        timerText.text = "Extraction time: " + Mathf.Ceil(currentTime).ToString();

        Debug.Log("timer" + currentTime);
        if (currentTime <= 0)
        {
            currentTime = 0;
            if (levelCompleteScreen != null)
            {
                levelCompleteScreen.SetActive(true);
            }
            enabled = false;
            //skipping to the next level automatical
            //if (menuPlayer != null)
            //{
            //    menuPlayer.NextLevel();
            //}
        Time.timeScale = 0;
        }
    }
        
}