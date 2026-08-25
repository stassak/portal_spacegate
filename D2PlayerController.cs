using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class D2PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 20f;
    public float acceleration = 10f;
    public float deceleration = 8f;

    private Vector3 targetVelocity;
    private Vector3 smoothVelocity;

    [Header("Rotation")]
    public float pitchSpeed = 100f;   // rotate around X
    public float yawSpeed = 100f;     // rotate around Y
    public float rollSpeed = 150f;    // rotate around Z (spin)

    private Vector3 targetRotSpeed;    // desired rotation speed
    private Vector3 smoothRotSpeed;    // smoothed rotation speed value
    public float rotationAcceleration = 4f;
    public float rotationDeceleration = 2f;

    private float moveX;
    private float moveY;
    private float pitchInput;
    private float yawInput;
    private float rollInput;

    [Header("Weapons")]
    [SerializeField] private AudioClip[] shootingSounds;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float fireCooldown = 0.25f;
    private int currentWeaponIndex = 0;
    private float nextFireTime = 0f;

    [Header("Weapon Fire Restrictions")]
    [SerializeField] private bool[] semiOnlyWeapon;

    [Header("Weapon UI")]
    [SerializeField] private TextMeshProUGUI weaponText;
    [SerializeField] private Image weaponIcon;          // UI Image
    [SerializeField] private Sprite[] weaponIcons;      // Icons for each weapon

    [Header("Shooting Audio")]
    [SerializeField] private AudioSource shootingAudioSource;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Engine")]
    [SerializeField] private AudioSource powerEngineAudioSource;
    [SerializeField] private AudioClip powerEngineSound;
    [SerializeField] private float engineMinVolume = 0.2f;
    [SerializeField] private float engineMaxVolume = 1.0f;
    [SerializeField] private float engineVolumeSpeed = 2f;
    [SerializeField] private float engineSoundFadeInTime = 0.5f;
    [SerializeField] private float engineSoundFadeOutTime = 1.0f;

    [Header("Engine Effects")]
    [SerializeField] private AudioSource engineAudioSource;  // Separate audio source for engine sound
    [SerializeField] private AudioClip engineBoostSound;    // Engine boost audio clip
    [SerializeField] private GameObject[] engineLights;      // Array of engine light GameObjects

    private bool isEngineActive = false;
    private float engineSoundVolume = 0f;

    // NEW: Shooting Mode Variables
    public enum ShootingMode {Burst , Semi} //Auto //option

    [Header("Shooting Modes")]
    [SerializeField] private ShootingMode currentShootingMode = ShootingMode.Burst;

    // Burst mode variables
    [SerializeField] private int burstCount = 5;           // Number of shots per burst
    [SerializeField] private float burstDelay = 0.1f;      // Delay between shots in a burst
    private bool isBursting = false;
    private int burstShotsFired = 0;
    private Coroutine burstCoroutine = null;

    [Header("Burst UI")]
    [SerializeField] private TextMeshProUGUI shootingModeText;
    [SerializeField] private Image shootingModeIcon;
    [SerializeField] private Sprite semiIcon;
    [SerializeField] private Sprite burstIconSprite;


    // Semi-auto variables
    private bool canShootSemi = true;

    // UI Elements for shooting mode
   // [SerializeField] private TextMeshProUGUI shootingModeText;

    public int CurrentWeaponIndex => currentWeaponIndex;
    public System.Action onWeaponChanged;

    void Start()
    {
        GameState.IsGameOver = false;

        Time.timeScale = 1f;
        nextFireTime = 0f;
        isBursting = false;
        burstCoroutine = null;
        canShootSemi = true;

        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;

        // Initialize engine audio source if not set
        if (engineAudioSource == null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
            engineAudioSource.spatialBlend = 1f; // 3D sound
            engineAudioSource.loop = true;
        }

        engineAudioSource.volume = 0f;
        engineAudioSource.loop = true;

        if (engineBoostSound != null)
        {
            engineAudioSource.clip = engineBoostSound;
            engineAudioSource.Play(); // always running
        }

        // Initialize engine lights
        if (engineLights != null && engineLights.Length > 0)
        {
            SetEngineLights(false);
        }

        // FIX: Initialize weapons first
        SelectWeapon(0); // This will set the first weapon active

        // FIX: Initialize shooting mode AFTER weapon is selected
        SetShootingMode(currentShootingMode);

        Debug.Log("Weapons count: " + weapons.Length);
        Debug.Log("First weapon active: " + (weapons.Length > 0 && weapons[0] != null ? weapons[0].activeSelf : false));

        // Initialize shooting audio source if not set
        if (shootingAudioSource == null)
        {
            shootingAudioSource = gameObject.AddComponent<AudioSource>();
        }

        Debug.Log("FirePoint: " + firePoint);
        Debug.Log("Projectiles length: " + projectiles.Length);
        Debug.Log("Weapons length: " + weapons.Length);

        // Update shooting mode UI
        UpdateShootingModeUI();
    }

    void Update()
    {
        // --- Movement Input (X + Y movement) ---
        moveX = Input.GetAxis("Horizontal");   // A/D
        moveY = Input.GetAxis("Vertical");     // W/S

        // --- Rotation Controls ---
        pitchInput = 0f;
        yawInput = 0f;
        rollInput = 0f;

        if (Input.GetKey(KeyCode.W)) pitchInput = -1f;
        if (Input.GetKey(KeyCode.S)) pitchInput = 1f;

        if (Input.GetKey(KeyCode.A)) yawInput = -1f;
        if (Input.GetKey(KeyCode.D)) yawInput = 1f;

        if (Input.GetKey(KeyCode.Q)) rollInput = 1f;
        if (Input.GetKey(KeyCode.E)) rollInput = -1f;

        // Check if any flight control is being used
        bool anyFlightControl =
            Mathf.Abs(moveX) > 0.1f ||
            Mathf.Abs(moveY) > 0.1f ||
            Mathf.Abs(pitchInput) > 0.1f ||
            Mathf.Abs(yawInput) > 0.1f ||
            Mathf.Abs(rollInput) > 0.1f;

        // Handle engine effects based on flight control input
        HandleEngineEffects(anyFlightControl);

        HandleWeaponSwitch();
        HandleShootingModeSwitch();

        // FIX: Move shooting logic BEFORE the game over check? 
        // Actually, keep it after but make sure it has its own check
        HandleProjectileShooting();

        // stop game proccess (this should be checked inside HandleProjectileShooting too)
        if (GameState.IsGameOver) return;
        //-----------------weapon index debug-------------------------------------------------------

      //  Debug.Log("Weapon index: " + currentWeaponIndex);
        if (currentWeaponIndex < weapons.Length)
          //  Debug.Log("Weapon object: " + weapons[currentWeaponIndex]);
            
        if (GameState.IsGameOver)
            return;
    }

    void FixedUpdate()
    {
        if (GameState.IsGameOver)
            return;

        HandleMovement();
        HandleRotation();
    }

    void UpdateWeaponUI()
    {
        // ICON (main part )
        if (weaponIcon != null && currentWeaponIndex < weaponIcons.Length)
        {
            weaponIcon.sprite = weaponIcons[currentWeaponIndex];
        }
    }

    private void HandleEngineEffects(bool anyFlightControl)
    {
        // If there's flight control input, activate engine effects
        if (anyFlightControl)
        {
            if (!isEngineActive)
            {
                isEngineActive = true;
                SetEngineLights(true);

                if (engineAudioSource != null && engineBoostSound != null)
                {
                    if (!engineAudioSource.isPlaying)
                    {
                        engineAudioSource.Play();
                    }
                }
            }

            // Fade in engine sound
            if (engineAudioSource != null)
            {
                engineSoundVolume = Mathf.MoveTowards(
                    engineSoundVolume,
                    1f,
                    Time.deltaTime / engineSoundFadeInTime
                );
                engineAudioSource.volume = engineSoundVolume;
            }
        }
        else
        {
            if (isEngineActive)
            {
                // Fade out engine sound
                if (engineAudioSource != null)
                {
                    engineSoundVolume = Mathf.MoveTowards(
                        engineSoundVolume,
                        0f,
                        Time.deltaTime / engineSoundFadeOutTime
                    );
                    engineAudioSource.volume = engineSoundVolume;

                    // Stop audio when volume reaches 0
                    if (engineSoundVolume <= 0.01f)
                    {
                        engineAudioSource.Stop();
                        isEngineActive = false;
                        SetEngineLights(false);
                    }
                }
                else
                {
                    isEngineActive = false;
                    SetEngineLights(false);
                }
            }
        }
    }

    private void HandleEngineSound(bool engineInput)
    {
        if (engineAudioSource == null) return;

        float targetVolume = engineInput ? engineMaxVolume : engineMinVolume;

        engineAudioSource.volume = Mathf.Lerp(
            engineAudioSource.volume,
            targetVolume,
            Time.deltaTime * engineVolumeSpeed
        );
    }

    private void SetEngineLights(bool active)
    {
        if (engineLights == null) return;

        foreach (GameObject lightObj in engineLights)
        {
            if (lightObj != null)
            {
                lightObj.SetActive(active);
            }
        }
    }

    private void HandleMovement()
    {
        // Raw input → desired movement direction
        Vector3 input = new Vector3(moveX, moveY, 0);

        // Convert input into target velocity
        targetVelocity = input * moveSpeed;

        // Smoothly interpolate velocity (accel & decel)
        smoothVelocity = Vector3.Lerp(
            smoothVelocity,
            targetVelocity,
            (targetVelocity.magnitude > 0 ? acceleration : deceleration) * Time.fixedDeltaTime
        );

        // Apply smoothed velocity
        rb.velocity = new Vector3(smoothVelocity.x, smoothVelocity.y, 0);

        // Lock Z axis permanently
        Vector3 pos = rb.position;
        pos.z = 0f;
        rb.position = pos;
    }

    private void HandleRotation()
    {
        // raw rotation input → desired rotation speed per axis
        targetRotSpeed = new Vector3(
            pitchInput * pitchSpeed,
            yawInput * yawSpeed,
            rollInput * rollSpeed
        );

        // smooth rotation inertia
        smoothRotSpeed = Vector3.Lerp(
            smoothRotSpeed,
            targetRotSpeed,
            (targetRotSpeed.sqrMagnitude > 0 ? rotationAcceleration : rotationDeceleration)
            * Time.fixedDeltaTime
        );

        // apply smooth rotation
        Quaternion deltaRotation = Quaternion.Euler(smoothRotSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    // --- WEAPON SWITCHING ---
    private void HandleWeaponSwitch()
    {
        for (int i = 0; i < weapons.Length; i++)
            if (Input.GetKeyDown((i + 1).ToString()))
                SelectWeapon(i);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            SelectWeapon((currentWeaponIndex + 1) % weapons.Length);
        else if (scroll < 0f)
            SelectWeapon((currentWeaponIndex - 1 + weapons.Length) % weapons.Length);
    }

    // NEW: Handle shooting mode switching
    /*   private void HandleShootingModeSwitch()
       {
           if (Input.GetKeyDown(KeyCode.LeftShift))
           {
               // Cycle through shooting modes
               int nextMode = ((int)currentShootingMode + 1) % System.Enum.GetValues(typeof(ShootingMode)).Length;
               currentShootingMode = (ShootingMode)nextMode;
               UpdateShootingModeUI();

               // Cancel any ongoing burst
               if (burstCoroutine != null)
               {
                   StopCoroutine(burstCoroutine);
                   burstCoroutine = null;
                   isBursting = false;
               }

               // Reset semi-auto flag
               canShootSemi = true;

              // Debug.Log($"Shooting mode changed to: {currentShootingMode}");
           }
      }
     */

    private void HandleShootingModeSwitch()
    {
        // Weapon 3 and 4 are SEMI ONLY
        if (currentWeaponIndex == 2 || currentWeaponIndex == 3)
        {
            currentShootingMode = ShootingMode.Semi;
            UpdateShootingModeUI();

            return; // block Shift switching
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Cycle through shooting modes
            int nextMode = ((int)currentShootingMode + 1) %
                System.Enum.GetValues(typeof(ShootingMode)).Length;

            currentShootingMode = (ShootingMode)nextMode;

            UpdateShootingModeUI();


            // Cancel burst if running
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
                burstCoroutine = null;
                isBursting = false;
            }


            // Reset semi-auto flag
            canShootSemi = true;
        }
    }

    // NEW: Update shooting mode UI
    void UpdateShootingModeUI()
    {
        if (shootingModeText != null)
        {
            shootingModeText.text = currentShootingMode.ToString();
        }

        if (shootingModeIcon != null)
        {
            switch (currentShootingMode)
            {
                case ShootingMode.Semi:
                    shootingModeIcon.sprite = semiIcon;
                    break;

                case ShootingMode.Burst:
                    shootingModeIcon.sprite = burstIconSprite;
                    break;
            }
        }
    }

    private void SelectWeapon(int index)
    {
        if (index >= weapons.Length) return;

        currentWeaponIndex = index;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].SetActive(i == index);
        }

        UpdateWeaponUI(); // THIS WAS MISSING

        onWeaponChanged?.Invoke();
    }

    
    // MODIFIED: Handle projectile shooting with different modes
    private void HandleProjectileShooting()
    {
        // Don't shoot if game is over
        if (GameState.IsGameOver) return;

        switch (currentShootingMode)
        {
            case ShootingMode.Semi:
                HandleSemiShooting();
                break;
            case ShootingMode.Burst:
                HandleBurstShooting();
                break;
                /* case ShootingMode.Auto:
                     HandleAutoShooting();
                     break;*/
        }
    }

    // FIX: Semi-auto shooting (single shot per click)
    private void HandleSemiShooting()
    {
        bool shootingInput = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (shootingInput && canShootSemi && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
            canShootSemi = false;

            // Reset semi-auto flag after cooldown
            StartCoroutine(ResetSemiShoot());
        }
    }

    // FIX: Reset semi-auto shooting flag
    private IEnumerator ResetSemiShoot()
    {
        yield return new WaitForSeconds(fireCooldown);
        canShootSemi = true;
    }

    // FIX: Burst shooting (multiple shots per click)
    private void HandleBurstShooting()
    {
        bool shootingInput = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);

        if (shootingInput && !isBursting && Time.time >= nextFireTime)
        {
            burstShotsFired = 0;
            if (burstCoroutine != null)
            {
                StopCoroutine(burstCoroutine);
            }
            burstCoroutine = StartCoroutine(BurstShootSequence());
        }
    }

    // NEW: Burst shoot sequence coroutine
    private IEnumerator BurstShootSequence()
    {
        isBursting = true;

        for (int i = 0; i < burstCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstDelay);
        }

        nextFireTime = Time.time + fireCooldown;
        isBursting = false;
        burstCoroutine = null;
    }

    // NEW: Auto shooting (continuous while holding)
    /* private void HandleAutoShooting()
     {
         bool shootingInput = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

         if (shootingInput && Time.time >= nextFireTime)
         {
             Shoot();
             nextFireTime = Time.time + fireCooldown;
         }
     }*/

    // Modified shoot method with better error handling
    private void Shoot()
    {
      /*  Debug.Log("================================================");
        Debug.Log("FIRE! Mode: " + currentShootingMode);
        Debug.Log("Current Weapon Index: " + currentWeaponIndex);
        Debug.Log("Projectiles length: " + projectiles.Length);*/

        if (currentWeaponIndex >= projectiles.Length)
        {
            Debug.LogError("Weapon index out of range!");
            return;
        }

        GameObject projPrefab = projectiles[currentWeaponIndex];
        if (projPrefab == null)
        {
            //Debug.LogError("Projectile prefab is null for index " + currentWeaponIndex);
            return;
        }

        if (firePoint == null)
        {
         //   Debug.LogError("FirePoint is null!");
            return;
        }

        GameObject proj = Instantiate(projPrefab, firePoint.position, firePoint.rotation);
       // Debug.Log("Projectile instantiated: " + proj.name);

        Shooting shootScript = proj.GetComponent<Shooting>();
        if (shootScript != null)
        {
            shootScript.Launch(firePoint.forward);
          //  Debug.Log("Projectile launched!");
        }
        else
        {
           // Debug.LogWarning("No Shooting script on projectile!");
        }

        // Play shooting sound
        if (audioSource != null && shootingSounds.Length > currentWeaponIndex && shootingSounds[currentWeaponIndex] != null)
        {
            audioSource.PlayOneShot(shootingSounds[currentWeaponIndex]);
           // Debug.Log("Sound played");
        }
    }
    // NEW: Public method to change shooting mode from UI
    public void SetShootingMode(ShootingMode mode)
    {
        currentShootingMode = mode;
        UpdateShootingModeUI();

        // Cancel any ongoing burst
        if (burstCoroutine != null)
        {
            StopCoroutine(burstCoroutine);
            burstCoroutine = null;
            isBursting = false;
        }

        canShootSemi = true;
    }

    // NEW: Get current shooting mode
    public ShootingMode GetShootingMode()
    {
        return currentShootingMode;
    }
}