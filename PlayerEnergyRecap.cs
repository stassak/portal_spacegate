using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergyRecap : MonoBehaviour
{
    [SerializeField] private float energyAmount = 800f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerSystem player = other.GetComponent<PlayerPowerSystem>();

        if (player != null)
        {
            player.AddEnergy(energyAmount);
            Destroy(gameObject);
        }
    }
}
