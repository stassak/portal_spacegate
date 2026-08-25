using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerPowerSystem : MonoBehaviour
{
  

    [Header("Energy Settings")]
    public float minEnergy = 400f;
    public float maxEnergy = 800f;
    private float currentEnergy = 0f;



    [Header("UI")]
    public TextMeshProUGUI energyText;
    public Slider energySlider;

    [Header("Level")]
    public GameObject levelCompleteScreen;

    [Header("Audio")]
    public AudioSource interruptedProcess;
    [SerializeField] private AudioSource energyPickupSound; // Sound when picking up energy


    [Header("Warning UI")]
    [SerializeField] private GameObject lowEnergyIcon; //  icon
    [SerializeField] private float warningThreshold = 0.2f; // 20%

    private bool levelEnded = false;

    /*void Start()
    {
        Time.timeScale = 1f;
        // Random energy (like your timer)
        currentEnergy = Random.Range(minEnergy, maxEnergy);

        if (energySlider != null)
        {
            energySlider.maxValue = maxEnergy;//energySlider.maxValue = maxEnergy;//energySlider.maxValue = currentEnergy;
            energySlider.value = currentEnergy;
        }

        if (interruptedProcess != null)
        {
            interruptedProcess.Play();
        }
    }*/
    void Start()
    {
        Time.timeScale = 1f;

        currentEnergy = maxEnergy;//Random.Range(minEnergy, maxEnergy);//maxEnergy;//Random.Range(minEnergy, maxEnergy);

        if (energySlider != null)
        {
            energySlider.maxValue = currentEnergy; // FIXED
            energySlider.value = currentEnergy;
        }

        if (interruptedProcess != null)
        {
            interruptedProcess.Play();
        }
    }

    void Update()
    {
       // Debug.Log("Energy: " + currentEnergy + " | TimeScale: " + Time.timeScale);

        if (levelEnded) return;

        DrainEnergy();
        UpdateUI();
    }

    void DrainEnergy()
    {
        //float drainSpeed = 0.01f;// manual assign timer
        
        currentEnergy -= Time.deltaTime;// drainSpeed
        // currentEnergy -= 0f;
       // Debug.Log("Current energy = " + currentEnergy);
        if (currentEnergy <= 0f)
        {
            currentEnergy = 0f;
        //    levelEnded = true;//
            LevelComplete();
        }
    }

    void UpdateUI()
    {
        if (energyText != null)
        {
            energyText.text = "Energy: " + Mathf.Ceil(currentEnergy).ToString();
        }

        if (energySlider != null)
        {
            energySlider.value = currentEnergy;
        }

        float percent = 1f;
        if (energySlider != null && energySlider.maxValue > 0f)
        {
            percent = currentEnergy / energySlider.maxValue;
        }

        if (lowEnergyIcon != null)
        {
            lowEnergyIcon.SetActive(percent <= warningThreshold);
        }

        if (energyText != null)
        {
            energyText.color = (percent <= warningThreshold) ? Color.red : Color.white;
        }
    }

    void LevelComplete()
    {
        // Prevent double lose screens
        if (levelEnded) return;

        levelEnded = true;
        GameState.IsGameOver = true;

        Debug.Log("Energy depleted /////// Level Complete!");

        if (levelCompleteScreen != null)
            levelCompleteScreen.SetActive(true);

        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudio)
        {
            if (audio.gameObject.CompareTag("Enemy"))
            {
                audio.Stop();
            }
        }

        Time.timeScale = 0f;
    }

    public void AddEnergy(float amount)
    {
        if (levelEnded) return;

        currentEnergy += amount;

        // IMPORTANT
        currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

        UpdateUI();
    }

    /*  public void AddEnergy(float amount)
      {
          if (levelEnded) return;

          currentEnergy += amount;
          currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);

          // Optional: sound
          if (energyPickupSound != null)
              energyPickupSound.Play();

          Debug.Log("Energy added: " + amount);
      }*/

    public void FullRecharge()
    {
        if (levelEnded) return;

        currentEnergy = maxEnergy;
        UpdateUI();
    }
}
