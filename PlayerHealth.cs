using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private ShakeCamera deathCamShake;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject deathCamera;

    [Header("Scene Management")]
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private float destroyDelay = 0.1f;

    [Header("Player Visuals")]
    [SerializeField] private GameObject externalModel;
    [SerializeField] private GameObject internalModel;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float lowHealthThreshold = 25f; // warning when health <= 20

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float overlayFlashAlpha = 0.45f;
    [SerializeField] private float overlayFadeSpeed = 3f;

    [Header("Low Health Warning")]
    [SerializeField] private Image lowHealthWarningImage;
    [SerializeField] private float blinkSpeed = 4f;
    [SerializeField] private float maxBlinkAlpha = 0.5f;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private float gameOverDelay = 1.5f;

    [Header("Player Scripts To Disable")]
    [SerializeField] private MonoBehaviour[] scriptsToDisable;

    [Header("Death Effect")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionPoint;
   

    [Header("Death Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    private bool isDead = false;
    private bool showDamageFlash = false;

    void Start()
    {
        if (deathCamera != null)
            deathCamera.SetActive(false); // important

        if (playerCamera != null)
            playerCamera.SetActive(true);

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    void Update()
    {
        HandleDamageOverlay();
        HandleLowHealthWarning();
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        showDamageFlash = true;
        UpdateHealthUI();

        if (PLayerManager.Instance != null)
        {
            PLayerManager.Instance.OnPlayerHit();
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (healthText != null)
        {
            healthText.text = ": " + Mathf.CeilToInt(currentHealth);
        }
    }

    private void HandleDamageOverlay()
    {
        if (damageOverlay == null) return;

        Color c = damageOverlay.color;

        if (showDamageFlash)
        {
            c.a = overlayFlashAlpha;
            showDamageFlash = false;
        }
        else
        {
            c.a = Mathf.MoveTowards(c.a, 0f, overlayFadeSpeed * Time.unscaledDeltaTime);
        }

        damageOverlay.color = c;
    }

    private void HandleLowHealthWarning()
    {
        if (lowHealthWarningImage == null || isDead) return;

        if (currentHealth > 0f && currentHealth <= lowHealthThreshold)
        {
            Color c = lowHealthWarningImage.color;
            c.a = Mathf.PingPong(Time.unscaledTime * blinkSpeed, maxBlinkAlpha);
            lowHealthWarningImage.color = c;
        }
        else
        {
            Color c = lowHealthWarningImage.color;
            c.a = 0f;
            lowHealthWarningImage.color = c;
        }
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        GameState.IsGameOver = true;

        // Stop physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Disable gameplay scripts
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // 1. Switch camera FIRST
        if (playerCamera != null) playerCamera.SetActive(false);
        if (deathCamera != null) deathCamera.SetActive(true);

        yield return null; // wait 1 frame (IMPORTANT)

        // 2. Explosion position (IN FRONT OF CAMERA)
        Vector3 spawnPos = deathCamera.transform.position + deathCamera.transform.forward * 5f;

        // 3. Spawn explosion
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, spawnPos, Quaternion.identity);

        // 4. Play sound (same position)
        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, spawnPos);

        // 5. Camera shake (synced)
        if (deathCamShake != null)
            StartCoroutine(deathCamShake.Shake(0.4f, 0.4f));

        // 6. SMALL delay so explosion is visible ON PLAYER
        yield return new WaitForSecondsRealtime(0.2f);

        // 7. Hide player AFTER explosion
        if (externalModel != null) externalModel.SetActive(false);
        if (internalModel != null) internalModel.SetActive(false);

        // 8. Wait cinematic time
        yield return new WaitForSecondsRealtime(gameOverDelay);

        // 9. Show UI
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        // 10. Wait a bit then destroy player
        yield return new WaitForSecondsRealtime(destroyDelay);

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        //Destroy(gameObject);

        // 11. Load menu
      //  SceneManager.LoadScene(menuSceneName); //skipping to the main menu automaticaly

    }
}
