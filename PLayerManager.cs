using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PLayerManager : MonoBehaviour
{
    public static PLayerManager Instance;

    [Header("UI Hit Explosion")]
    public Image hitExplosionImage;
    public Sprite[] hitExplosionSprites;

    public float flashDuration = 0.15f;
    public float startScale = 0.6f;
    public float endScale = 1.3f;

    [Header("Energy player")]
    public float energyPlayer;

    int lastIndex = -1;
    private Coroutine flashCoroutine;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
             return;
    }

    void Start()
    {
        //energy level
        // Debug.Log(energyPlayer);
        Time.timeScale = 1f;

        if (hitExplosionImage != null)
            hitExplosionImage.gameObject.SetActive(false);
    }

    public void OnPlayerHit(int damage = 1)
    {
        if (GameState.IsGameOver)
            return;
        PlayHitFlash();
    }

    void PlayHitFlash()
    {
        if (hitExplosionImage == null)
            return;

        if (hitExplosionSprites == null || hitExplosionSprites.Length == 0)
            return;

        int index;

        do
        {
            index = Random.Range(0, hitExplosionSprites.Length);
        }
        while (index == lastIndex && hitExplosionSprites.Length > 1);

        lastIndex = index;

        hitExplosionImage.sprite = hitExplosionSprites[index];

        hitExplosionImage.transform.SetAsLastSibling();

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(HitFlashRoutine());
    }

    IEnumerator HitFlashRoutine()
    {
        hitExplosionImage.gameObject.SetActive(true);

        float t = 0f;

        Color c = hitExplosionImage.color;
        c.a = 0.8f;

        hitExplosionImage.color = c;

        hitExplosionImage.transform.localScale =
            Vector3.one * startScale;

        while (t < flashDuration)
        {
            t += Time.unscaledDeltaTime;

            float p = t / flashDuration;

            c.a = Mathf.Lerp(0.8f, 0f, p);

            hitExplosionImage.color = c;

            float scale = Mathf.Lerp(startScale, endScale, p);

            hitExplosionImage.transform.localScale =
                Vector3.one * scale;

            yield return null;
        }

        hitExplosionImage.gameObject.SetActive(false);
    }
}