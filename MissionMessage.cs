using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionMessage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject missionPanel;
    [SerializeField] private TextMeshProUGUI missionText;

    [Header("Timer")]
    [SerializeField] private float displayTime = 10f;


    void Start()
    {
        if (missionPanel != null)
            missionPanel.SetActive(true);


        if (missionText != null)
        {
            missionText.text =
                "MISSION OBJECTIVE\n\n" +
                "Destroy all enemy " +
                "infrastructure";
        }


        Invoke(nameof(HideMission), displayTime);
    }


    void HideMission()
    {
        if (missionPanel != null)
            missionPanel.SetActive(false);
    }
}

