using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float climbSpeed = 30f;     // pitch up/down
    [SerializeField] private float turnSpeed = 60f;      // yaw
    [SerializeField] private float bankAmount = 45f;     // roll visual
    [SerializeField] private float baseSpeed = 50f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float boostMultiplier = 50f;
    [SerializeField] private float boostForce = 2000f;

    [Header("Boost Lights")]
    public Light redPointLight;
    public Light boostSpotLight;

    //[SerializeField] private AudioClip engineBoostSound;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] shootingSounds;
    [SerializeField] private AudioClip engineBoostSound;


    [Header("Weapons")]
    [SerializeField] private GameObject[] weapons;       // visual models or gun objects
    [SerializeField] private Transform firePoint;        // muzzle point
    [SerializeField] private GameObject[] projectiles;   // matching projectile for each weapon
    [SerializeField] private float fireCooldown = 0.25f;
    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;

  /*  [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] shootingSounds; // optional, one per weapon*/
    private float targetSpeed;
    private float currentSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        targetSpeed = baseSpeed;
        currentSpeed = baseSpeed;

        if (redPointLight != null) redPointLight.enabled = false;
        if (boostSpotLight != null) boostSpotLight.enabled = false;

        SelectWeapon(0);
    }

    void Update()
    {
       // HandleBoost();
        HandleWeaponSwitch();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // --- WEAPON SWITCHING ---
    private void HandleWeaponSwitch()
    {
        // number keys 1,2,3...
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                SelectWeapon(i);
            }
        }

        // scroll wheel
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

    private void SelectWeapon(int index)
    {
        currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == index);
        }

        Debug.Log($"Switched to weapon: {weapons[index].name}");
    }

    // --- SHOOTING ---
    private void HandleShooting()
    {
        bool isShooting = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Shoot()
    {
        if (firePoint == null || projectiles.Length == 0) return;

        GameObject projPrefab = projectiles[currentWeaponIndex];
        if (projPrefab == null) return;

        GameObject proj = Instantiate(projPrefab, firePoint.position, firePoint.rotation);

        // If projectile has a "Shooting" script
        Shooting shootScript = proj.GetComponent<Shooting>();
        if (shootScript != null)
            shootScript.Launch(firePoint.forward);

        // Optional: audio per weapon
        if (audioSource != null)
        {
            AudioClip sound = (shootingSounds != null && currentWeaponIndex < shootingSounds.Length)
                ? shootingSounds[currentWeaponIndex]
                : null;

            if (sound != null)
                audioSource.PlayOneShot(sound);
        }
    }

    // --- BOOST ---
   /* private void HandleBoost()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            targetSpeed = baseSpeed * boostMultiplier;

            if (redPointLight != null) redPointLight.enabled = true;
            if (boostSpotLight != null) boostSpotLight.enabled = true;

            rb.AddForce(transform.forward * boostForce, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            targetSpeed = baseSpeed;

            if (redPointLight != null) redPointLight.enabled = false;
            if (boostSpotLight != null) boostSpotLight.enabled = false;
        }
    }*/

    // --- MOVEMENT ---
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float rollInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        float pitch = -vertical * climbSpeed * Time.fixedDeltaTime;
        float yaw = horizontal * turnSpeed * Time.fixedDeltaTime;
        float roll = rollInput * 60f * Time.fixedDeltaTime;

        transform.Rotate(pitch, yaw, roll, Space.Self);

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.velocity = transform.forward * currentSpeed;
    }

}
