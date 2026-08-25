using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private D2PlayerController playerController;

    [Header("UI")]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Sprite[] weaponIcons;

    void Start()
    {
        UpdateUI();

        if (playerController != null)
            playerController.onWeaponChanged += UpdateUI;
    }

    void UpdateUI()
    {
        if (playerController == null || weaponIcon == null) return;

        int index = playerController.CurrentWeaponIndex;

        if (index < weaponIcons.Length && weaponIcons[index] != null)
        {
            weaponIcon.sprite = weaponIcons[index];
        }
    }
}
