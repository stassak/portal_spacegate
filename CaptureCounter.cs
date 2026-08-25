using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CaptureCounter : MonoBehaviour
{
    private bool hasLost = false;

    public int objectCount = 0;
    public int requiredToLose = 20;

    public TextMeshProUGUI countText;

    [SerializeField] private MenuPlayer menuPlayer;
    [Header("Progress UI")]
    [SerializeField] private Slider progressBar;

    [Header("UI")]
    [SerializeField] private GameObject loseScene;

    void Start()
    {
        if (loseScene != null)
        {
            loseScene.SetActive(false);
        }


        if (progressBar != null)
        {
            progressBar.maxValue = requiredToLose;
            progressBar.value = objectCount;
        }

        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasLost) return;

        // Enemy reached protected zone
        if (other.CompareTag("Enemy"))
        {
            objectCount++;

            UpdateUI();
            CheckLose();
        }

        // Destroy EVERYTHING that enters boundary
        Destroy(other.gameObject);
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            float percent = (float)objectCount / requiredToLose * 100f;
            percent = Mathf.Clamp(percent, 0f, 100f);
            countText.text = $"captured zone: {percent}%";// {objectCount}/{requiredToLose}";
            //also % and amount
            //countText.text = $"Captured: {percent:0}% ({objectCount}/{requiredToLose})";
        }

        if (progressBar != null)
        {
            progressBar.value = objectCount;
        }
    }

    private void CheckLose()
    {
        if (objectCount >= requiredToLose && !hasLost )
        {
            hasLost = true;

           // Debug.Log("YOU LOST - CAPTURE COMPLETE");

            if (menuPlayer != null)
            {
                menuPlayer.ShowLoseMenu();
            }
        }
    }

    /*private void LoseGame()
    {
        
        SceneManager.LoadScene("MainMenu");
    }*/
}
