using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostSound : MonoBehaviour
{
    [Header("Boost Sound Settings")]
    public AudioClip boostSound;   // assign your looping thruster sound
    public float boostVolume = 0.8f;

    private AudioSource boostAudio;

    void Start()
    {
        // Create a dedicated AudioSource
        boostAudio = gameObject.AddComponent<AudioSource>();
        boostAudio.clip = boostSound;
        boostAudio.loop = true;
        boostAudio.playOnAwake = false;
        boostAudio.volume = boostVolume;
        boostAudio.spatialBlend = 0f; // 0 = 2D sound
    }

    void Update()
    {
        // Hold V to play boost sound
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (boostSound != null && !boostAudio.isPlaying)
                boostAudio.Play();
        }

        // Release V to stop
        if (Input.GetKeyUp(KeyCode.V))
        {
            if (boostAudio.isPlaying)
                boostAudio.Stop();
        }
    }

}
