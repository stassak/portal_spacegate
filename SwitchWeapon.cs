using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWeapon : MonoBehaviour
{
    [Header("Player Weapons")]
    public GameObject[] weapons;               // assign all weapons (child objects or prefabs)
    private int currentWeaponIndex = 0;

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;        // bullet or laser projectile
    public Transform firePoint;                // where bullets spawn from
    public float fireRate = 0.25f;             // delay between shots
    public float projectileSpeed = 100f;       // bullet speed
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioSource shootAudio;             // optional
    public AudioClip shootSound;

    void Start()
    {
        SelectWeapon(currentWeaponIndex);
    }

    void Update()
    {
        HandleWeaponSwitch();
        HandleShooting();
    }

    void HandleWeaponSwitch()
    {
        //  Switch with number keys
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SwitchWeaponPLayer(i);
            }
        }

        // Optional scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            SelectWeapon(currentWeaponIndex);
        }
        else if (scroll < 0f)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0) currentWeaponIndex = weapons.Length - 1;
            SelectWeapon(currentWeaponIndex);
        }
    }

    void HandleShooting()
    {
        bool shooting = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (shooting && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void SwitchWeaponPLayer(int index)
    {
        if (index >= 0 && index < weapons.Length)
        {
            currentWeaponIndex = index;
            SelectWeapon(currentWeaponIndex);
        }
    }

    void SelectWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }
        Debug.Log("Switched to weapon: " + weapons[index].name);
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = firePoint.forward * projectileSpeed;

        if (shootAudio && shootSound)
            shootAudio.PlayOneShot(shootSound);
    }
}
