using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceJump : MonoBehaviour
{
   

    public KeyCode jumpKey = KeyCode.T;

    public float jumpX = 0f;

    private Vector3 startPosition;
    private bool isJumped = false;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    [Header("Teleport Flash")]
    [SerializeField] private Image teleportFlash;
    [SerializeField] private float flashDuration = 0.25f;

    void Start()
    {
        startPosition = transform.position;

        if (teleportFlash != null)
            teleportFlash.gameObject.SetActive(false);
        /* if (teleportFlash != null)
         {
             Color c = teleportFlash.color;
             c.a = 0f;
             teleportFlash.color = c;
         }*/
    }

    void Update()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            // Play jump sound
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }

            StartCoroutine(TeleportFlashRoutine());


        }
    }

    void ToggleJump()
    {
        if (!isJumped)
        {
            transform.position = new Vector3(
                startPosition.x + jumpX,
                startPosition.y,
                startPosition.z
            );
        }
        else
        {
            transform.position = startPosition;
        }

        isJumped = !isJumped;
    }
    IEnumerator TeleportFlashRoutine()
    {
        // 🔥 ALWAYS teleport first
        ToggleJump();

        // If no UI → just exit (but teleport still works)
        if (teleportFlash == null) yield break;

        teleportFlash.gameObject.SetActive(true);

        Color c = teleportFlash.color;
        c.a = 1f;
        teleportFlash.color = c;

        float t = 0f;

        while (t < flashDuration)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, t / flashDuration);
            teleportFlash.color = c;

            yield return null;
        }

        c.a = 0f;
        teleportFlash.color = c;

        teleportFlash.gameObject.SetActive(false);
    }
}
