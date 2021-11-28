using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    //controllers
    private Rigidbody2D rb;
    private AudioSource playerAudioSource = null;
    [SerializeField] private AudioSource playerWeaponAudioSource = null;
    [SerializeField] private AudioSource playerLegsAudioSource = null;
    private ParticleSystem playerParticleSystem = null;
    private Animator playerAnimator;
    [SerializeField] private Animator playerWeaponAnimator;
    [SerializeField] private Animator playerLegsAnimator;

    //stats   
    private int shotgunProjectiles = 0;
    public float currentPlayerHealth = 100;
    public float playerHealth = 100;
    private float playerSpeed = 5000;
    private float playerDashSpeed = 20000;
    private float playerAttackCooldown = 0;
    public static Vector3 playerPosition = Vector3.zero;
    private int playerWeaponProjectileType = 0;
    private float playerWeaponDamage = 2;
    private float playerWeaponProjectileSpeed = 1000;
    public static int playerWeaponType = 0;
    private const float immortalityTime = 5;
    private int playerCurrentAmmoCount = 0;
    private int playerClipAmmoCount = 0;
    public int playerTotalAmmoCount = 0;
    private float playerReloadingCooldown = 0;
    private float playerDashCooldown = 0.5f;
    public int playerAmmoDropCount = 0;
    private const float dashTime = 0.15f;
    private const float timeToDash = 0.25f;
    private float holdTime = 0;
    private float playerFootstepsCooldownTime = 0.4f;
    private float playerWeaponScatterAngle = 0;

    private Coroutine playerFootstepsCooldownCoroutine = null;

    //statements
    private bool isPlayerRotating = false;
    private bool isPlayerMoving = false;
    public bool isPlayerImmortal = false;
    private bool allCoroutinesStopped = false;
    private bool canPlayerAttack = true;
    private bool canPlayerDash = true;
    private bool isPlayerDashing = false;
    private bool canPlayFootstepSound = true;
    private int currentPlayerWeaponType = 0;
    private bool canPlayAdditionalAnimation = false;
    public static bool katanaAttack = false;
    private bool canRestartWalkingAnimation = false;

    //touch
    public static bool ignoreTouch = false;
    public static bool touchBlocked = false;
    [SerializeField] private GameObject rightStick = null;
    [SerializeField] private GameObject leftStick = null;
    [SerializeField] private GameObject rightStickGO = null;
    [SerializeField] private GameObject leftStickGO = null;
    private Vector3 rightStickPosition = Vector3.zero;
    private Vector3 leftStickPosition = Vector3.zero;
    private Vector3 rightStickStartPosition = Vector3.zero;
    private Vector3 leftStickStartPosition = Vector3.zero;
    private int rightStickTouchID = -1;
    private int leftStickTouchID = -1;

    //UI
    [SerializeField] private TextMeshProUGUI healthAmountText;
    [SerializeField] private TextMeshProUGUI ammoAmountText;
    [SerializeField] private AttackZoneController attackZoneController;

    //sounds
    [SerializeField] private AudioClip playerWeaponSound = null;
    [SerializeField] private AudioClip[] pistolSounds = null;
    [SerializeField] private AudioClip[] katanaSounds = null;
    [SerializeField] private AudioClip[] assaultRifleSounds = null;
    [SerializeField] private AudioClip[] shotgunSounds = null;
    [SerializeField] private AudioClip[] sniperRifleSounds = null;
    [SerializeField] private AudioClip[] grenadeLauncherSounds = null;
    [SerializeField] private AudioClip playerHitSound = null;
    [SerializeField] private AudioClip currentPlayerFootstepSound = null;
    [SerializeField] private AudioClip[] playerFootstepGroundSounds = null;

    //animations
    private string currentBodyAnimation = null;
    private string currentLegsWalkingAnimation = null;
    private string currentWeaponAttackAnimation = null;
    private string currentWeaponIdleAnimation = null;

    private string currentAdditionalLegsWalkingAnimation = null;
    private string currentAdditionalWeaponAttackAnimation = null;
    private string currentAdditionalWeaponIdleAnimation = null;

    private const string playerLegsIdleAnimation = "PlayerLegsIdle";
    private bool isPlayingWalkingAnimation = false;

    private Coroutine reloadingCoroutine = null;
    private Coroutine attackingCoroutine = null;
    private Coroutine shootingDelayCoroutine = null;
    private Coroutine immortalityCoroutine = null;
    private Coroutine dashCooldownCoroutine = null;
    private Coroutine dashCoroutine = null;
    private bool shootingDelayActivated = false;
    private Vector3 playerMovementVector = Vector3.zero;
    private Vector3 playerDashVector = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAudioSource = GetComponent<AudioSource>();
        playerParticleSystem = GetComponent<ParticleSystem>();
        playerAnimator = GetComponent<Animator>();
        //playerDashTrailController = playerDashTrail.GetComponent<DashTrailController>();
        SetupDefaults();
    }



    private void FixedUpdate()
    {
        if (!GameManager.gameOver && GameManager.gameStarted)
        {
            if (!isPlayerDashing)
            {
                if (isPlayerMoving)
                {
                    playerMovementVector = (leftStickPosition - leftStickStartPosition).normalized;

                    if (rb.velocity.magnitude < 5)
                    {
                        rb.AddForce(playerMovementVector * playerSpeed * Time.fixedDeltaTime); //android
                    }
                    if (!isPlayingWalkingAnimation || canRestartWalkingAnimation)
                    {
                        if ((currentAdditionalLegsWalkingAnimation != null && !canPlayAdditionalAnimation) || currentAdditionalLegsWalkingAnimation == null)
                        {
                            playerLegsAnimator.Play(currentLegsWalkingAnimation);
                        }
                        else
                        {
                            playerLegsAnimator.Play(currentAdditionalLegsWalkingAnimation);
                        }
                        canRestartWalkingAnimation = false;
                        isPlayingWalkingAnimation = true;
                    }
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    if (isPlayingWalkingAnimation || canRestartWalkingAnimation)
                    {
                        playerLegsAnimator.Play(playerLegsIdleAnimation);
                        canRestartWalkingAnimation = false;
                        isPlayingWalkingAnimation = false;
                    }
                }
            }
            else
            {
                if (rb.velocity.magnitude < PlayerWeaponStats.maxKatanaDashSpeed)
                {
                    rb.AddForce(playerDashVector * playerDashSpeed * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if (currentPlayerHealth > playerHealth)
        {
            playerParticleSystem.Play();
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }
            playerAudioSource.PlayOneShot(playerHitSound);
            currentPlayerHealth = playerHealth;
        }
        else if (currentPlayerHealth < playerHealth)
        {
            if (playerHealth <= 100)
            {
                currentPlayerHealth = playerHealth;
            }
            else
            {
                currentPlayerHealth = 100;
            }
        }
        if (currentPlayerHealth <= 0 && !GameManager.gameOver)
        {
            healthAmountText.SetText("0");
            GameManager.gameOver = true;
        }
        if (!GameManager.gameOver)
        {
            CheckTouches();
            if (GameManager.gameStarted)
            {
                healthAmountText.SetText(currentPlayerHealth.ToString());
                if (katanaAttack)
                {
                    DamageKatanaTargets();
                }
                if (playerTotalAmmoCount == -1 && playerCurrentAmmoCount != -1)
                {
                    ammoAmountText.SetText(playerCurrentAmmoCount.ToString());
                }
                else if (playerTotalAmmoCount == -1 && playerCurrentAmmoCount == -1)
                {
                    ammoAmountText.SetText("INF");
                }
                else
                {
                    ammoAmountText.SetText(playerCurrentAmmoCount.ToString() + "I" + playerTotalAmmoCount.ToString());
                }
                if (playerTotalAmmoCount == 0)
                {
                    playerWeaponType = 0;
                }

                PlayFootstepSound();
                if (currentPlayerWeaponType != playerWeaponType)
                {
                    SetWeaponSettings();
                }
                if (isPlayerRotating)
                {
                    if (rightStickPosition.x - rightStickStartPosition.x >= 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (rightStickPosition - rightStickStartPosition)));
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (rightStickPosition - rightStickStartPosition)));
                    }

                    if (!shootingDelayActivated && canPlayerAttack)
                    {
                        if (shootingDelayCoroutine != null)
                        {
                            StopCoroutine(shootingDelayCoroutine);
                        }
                        shootingDelayCoroutine = StartCoroutine(ShootingDelay());
                        shootingDelayActivated = true;
                    }
                    Attack();
                }
                else
                {
                    shootingDelayActivated = false;
                }
                playerPosition = transform.position;
            }
        }
        else
        {          
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
            if (rightStickGO.activeSelf)
            {
                rightStickGO.SetActive(false);
            }
            if (leftStickGO.activeSelf)
            {
                leftStickGO.SetActive(false);
            }
        }
    }

    private void SetupDefaults()
    {
        ignoreTouch = false;
        touchBlocked = false;
        isPlayerMoving = false;
        shootingDelayActivated = false;
        playerHealth = 100;
        currentPlayerHealth = playerHealth;
        playerSpeed = 5000;
        playerPosition = Vector3.zero;
        isPlayerRotating = false;
        allCoroutinesStopped = false;
        playerAttackCooldown = 0;
        canPlayerAttack = true;
        canPlayerDash = true;
        playerWeaponType = 0;
        currentPlayerWeaponType = playerWeaponType;
        playerWeaponProjectileType = 0;
        playerWeaponDamage = 2;
        playerWeaponProjectileSpeed = 0;
        
        rightStickPosition = Vector3.zero;
        leftStickPosition = Vector3.zero;
        rightStickStartPosition = Vector3.zero;
        leftStickStartPosition = Vector3.zero;
        rightStickTouchID = -1;
        leftStickTouchID = -1;
    }

    private void CheckTouches()
    {
        if (Input.touchCount > 0)
        {
            Touch[] touch = Input.touches;

            for (int i = 0; i < Input.touchCount; i++)
            {
                if (touch[i].phase == TouchPhase.Began)
                {
                    if (!ignoreTouch && !touchBlocked)
                    {
                        if (rightStickTouchID == -1 && touch[i].position.x >= Screen.width / 2)
                        {
                            rightStickTouchID = touch[i].fingerId;
                            rightStickStartPosition = touch[i].position;
                            rightStickGO.transform.position = rightStickStartPosition;
                            rightStick.transform.position = rightStickStartPosition;
                        }
                        if (leftStickTouchID == -1 && touch[i].position.x < Screen.width / 2)
                        {
                            leftStickTouchID = touch[i].fingerId;
                            leftStickStartPosition = touch[i].position;
                            leftStickGO.transform.position = leftStickStartPosition;
                            leftStick.transform.position = leftStickStartPosition;

                        }
                    }
                }

                if (touch[i].phase == TouchPhase.Ended)
                {
                    if (touch[i].fingerId == rightStickTouchID)
                    {
                        rightStickTouchID = -1;
                        if (!isPlayerRotating && holdTime <= timeToDash)
                        {
                            StartDash();
                        }
                        else
                        {
                            isPlayerRotating = false;
                        }
                        holdTime = 0;
                        if (rightStickGO.activeSelf)
                        {
                            rightStickGO.SetActive(false);
                        }
                    }
                    if (touch[i].fingerId == leftStickTouchID)
                    {
                        leftStickTouchID = -1;
                        isPlayerMoving = false;
                        leftStickStartPosition = Vector3.zero;
                        leftStickPosition = Vector3.zero;
                        if (leftStickGO.activeSelf)
                        {
                            leftStickGO.SetActive(false);
                        }
                    }
                }

                if (touch[i].phase == TouchPhase.Stationary)
                {
                    if (!touchBlocked)
                    {
                        if (rightStickTouchID > -1 && touch[i].fingerId == rightStickTouchID)
                        {
                            holdTime += Time.deltaTime;
                        }
                    }
                }

                if (touch[i].phase == TouchPhase.Moved)
                {
                    if (!touchBlocked)
                    {
                        if (rightStickTouchID > -1 && touch[i].fingerId == rightStickTouchID)
                        {
                            rightStickGO.SetActive(true);
                            rightStickPosition = touch[i].position;
                            rightStick.transform.position = rightStickPosition;
                            isPlayerRotating = true;
                        }
                        if (leftStickTouchID > -1 && touch[i].fingerId == leftStickTouchID)
                        {
                            leftStickGO.SetActive(true);
                            leftStickPosition = touch[i].position;
                            leftStick.transform.position = leftStickPosition;
                            isPlayerMoving = true;
                        }
                    }
                    else
                    {
                        if (touch[i].fingerId == rightStickTouchID)
                        {
                            rightStickTouchID = -1;

                            isPlayerRotating = false;

                            holdTime = 0;
                            if (rightStickGO.activeSelf)
                            {
                                rightStickGO.SetActive(false);
                            }
                        }
                        if (touch[i].fingerId == leftStickTouchID)
                        {
                            leftStickTouchID = -1;
                            isPlayerMoving = false;
                            leftStickStartPosition = Vector3.zero;
                            leftStickPosition = Vector3.zero;
                            if (leftStickGO.activeSelf)
                            {
                                leftStickGO.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlayFootstepSound()
    {
        if (isPlayerMoving && canPlayFootstepSound)
        {
            int soundID = Random.Range(0, playerFootstepGroundSounds.Length);
            currentPlayerFootstepSound = playerFootstepGroundSounds[soundID]; //fix
            if (playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }
            playerLegsAudioSource.PlayOneShot(currentPlayerFootstepSound);
            playerFootstepsCooldownCoroutine = StartCoroutine(PlayerFootstepsCooldown());
        }

        if (!isPlayerMoving && !canPlayFootstepSound)
        {
            if (playerFootstepsCooldownCoroutine != null)
            {
                StopCoroutine(playerFootstepsCooldownCoroutine);
            }
            canPlayFootstepSound = true;
        }
    }

    public void SetWeaponSettings()
    {
        if (attackingCoroutine != null)
        {
            StopCoroutine(attackingCoroutine);
            canPlayerAttack = true;
        }
        if (reloadingCoroutine != null)
        {
            StopCoroutine(reloadingCoroutine);
            canPlayerAttack = true;
        }
        playerWeaponDamage = PlayerWeaponStats.weaponsDamage[playerWeaponType];
        playerAttackCooldown = PlayerWeaponStats.weaponAttackCooldowns[playerWeaponType];
        playerWeaponProjectileType = PlayerWeaponStats.weaponProjectileTypes[playerWeaponType];
        playerWeaponProjectileSpeed = PlayerWeaponStats.weaponProjectilesSpeed[playerWeaponType];
        playerWeaponScatterAngle = PlayerWeaponStats.weaponScatterAngles[playerWeaponType];

        playerClipAmmoCount = PlayerWeaponStats.weaponClipAmmoCount[playerWeaponType];

        playerTotalAmmoCount = PlayerWeaponStats.weaponTotalAmmoCount[playerWeaponType];
        playerCurrentAmmoCount = playerClipAmmoCount;
        playerReloadingCooldown = PlayerWeaponStats.weaponReloadingCooldown[playerWeaponType];
        playerAmmoDropCount = PlayerWeaponStats.weaponAmmoDropCount[playerWeaponType];

        currentBodyAnimation = PlayerWeaponStats.playerBodyAnimations[playerWeaponType];
        currentWeaponIdleAnimation = PlayerWeaponStats.playerIdleAnimations[playerWeaponType];
        currentWeaponAttackAnimation = PlayerWeaponStats.playerAttackAnimations[playerWeaponType];
        currentLegsWalkingAnimation = PlayerWeaponStats.playerLegsWalkingAnimations[playerWeaponType];

        currentAdditionalWeaponIdleAnimation = PlayerWeaponStats.playerAdditionalIdleAnimations[playerWeaponType];
        currentAdditionalWeaponAttackAnimation = PlayerWeaponStats.playerAdditionalAttackAnimations[playerWeaponType];
        currentAdditionalLegsWalkingAnimation = PlayerWeaponStats.playerAdditionalLegsWalkingAnimations[playerWeaponType];

        if (playerWeaponType == 0)                         //pistol
        {
            int soundID = Random.Range(0, pistolSounds.Length);
            playerWeaponSound = pistolSounds[soundID];
        }
        else if (playerWeaponType == 1)                    //double pistol
        {
            int soundID = Random.Range(0, pistolSounds.Length);
            playerWeaponSound = pistolSounds[soundID];
        }
        else if (playerWeaponType == 2)                    //katana
        {
            int soundID = Random.Range(0, katanaSounds.Length);
            playerWeaponSound = katanaSounds[soundID];
        }
        else if (playerWeaponType == 3)                    //assault rifle
        {
            int soundID = Random.Range(0, assaultRifleSounds.Length);
            playerWeaponSound = assaultRifleSounds[soundID];
        }
        else if (playerWeaponType == 4)                    //shotgun
        {
            int soundID = Random.Range(0, shotgunSounds.Length);
            playerWeaponSound = shotgunSounds[soundID];
            shotgunProjectiles = PlayerWeaponStats.shotgunProjectiles;
        }
        else if (playerWeaponType == 5)                    //sniper rifle
        {
            int soundID = Random.Range(0, sniperRifleSounds.Length);
            playerWeaponSound = sniperRifleSounds[soundID];
        }
        else if (playerWeaponType == 6)                    //grenade launcher
        {
            int soundID = Random.Range(0, grenadeLauncherSounds.Length);
            playerWeaponSound = grenadeLauncherSounds[soundID];
        }
        canRestartWalkingAnimation = true;
        if ((currentAdditionalWeaponIdleAnimation != null && !canPlayAdditionalAnimation) || currentAdditionalWeaponIdleAnimation == null)
        {
            playerWeaponAnimator.Play(currentWeaponIdleAnimation);
        }
        else
        {
            playerWeaponAnimator.Play(currentAdditionalWeaponIdleAnimation);
        }


        playerAnimator.Play(currentBodyAnimation);



        currentPlayerWeaponType = playerWeaponType;
    }

    private void Attack()
    {
        if (canPlayerAttack && (playerCurrentAmmoCount > 0 || playerCurrentAmmoCount == -1))
        {
            Quaternion playerWeaponScatter = Quaternion.Euler(0, 0, Random.Range(-playerWeaponScatterAngle, playerWeaponScatterAngle));
            if (playerWeaponType == 2)         //katana
            {
                StartKatanaDash();
            }
            else if (playerWeaponType == 4)         //shotgun
            {
                if (shotgunProjectiles == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Quaternion projectileAdditionRotation = Quaternion.Euler(0, 0, -7 + (7 * i));

                        GameObject playerProjectile = ObjectPooler.GetPooledPlayerProjectile(playerWeaponProjectileType);
                        if (playerProjectile != null)
                        {
                            playerProjectile.transform.position = transform.position + transform.TransformDirection(new Vector3(0, 1, 0));
                            playerProjectile.transform.rotation = transform.rotation * playerWeaponScatter * projectileAdditionRotation;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileType = playerWeaponProjectileType;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileDamage = playerWeaponDamage;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileSpeed = playerWeaponProjectileSpeed;
                            playerProjectile.GetComponent<PlayerProjectileController>().canProjectileDamage = true;
                            playerProjectile.SetActive(true);
                            
                        }
                    }
                    playerCurrentAmmoCount--;
                }
                else if (shotgunProjectiles == 5)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Quaternion projectileAdditionRotation = Quaternion.Euler(0, 0, -10 + (5 * i));

                        GameObject playerProjectile = ObjectPooler.GetPooledPlayerProjectile(playerWeaponProjectileType);
                        if (playerProjectile != null)
                        {
                            playerProjectile.transform.position = transform.position + transform.TransformDirection(new Vector3(0, 1, 0));
                            playerProjectile.transform.rotation = transform.rotation * playerWeaponScatter * projectileAdditionRotation;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileType = playerWeaponProjectileType;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileDamage = playerWeaponDamage;
                            playerProjectile.GetComponent<PlayerProjectileController>().projectileSpeed = playerWeaponProjectileSpeed;
                            playerProjectile.GetComponent<PlayerProjectileController>().canProjectileDamage = true;
                            playerProjectile.SetActive(true);
                            
                        }
                    }
                    playerCurrentAmmoCount--;
                }
            }
            else
            {
                GameObject playerProjectile = ObjectPooler.GetPooledPlayerProjectile(playerWeaponProjectileType);
                if (playerProjectile != null)
                {
                    playerProjectile.transform.position = transform.position + transform.TransformDirection(new Vector3(0, 1, 0));
                    playerProjectile.transform.rotation = transform.rotation * playerWeaponScatter;
                    playerProjectile.GetComponent<PlayerProjectileController>().projectileType = playerWeaponProjectileType;
                    playerProjectile.GetComponent<PlayerProjectileController>().projectileDamage = playerWeaponDamage;
                    playerProjectile.GetComponent<PlayerProjectileController>().projectileSpeed = playerWeaponProjectileSpeed;
                    playerProjectile.GetComponent<PlayerProjectileController>().canProjectileDamage = true;
                    playerProjectile.SetActive(true);
                    playerCurrentAmmoCount--;
                }
            }



            

            if (currentAdditionalWeaponAttackAnimation != null && !canPlayAdditionalAnimation)
            {
                playerWeaponAnimator.Play(currentWeaponAttackAnimation);
                canPlayAdditionalAnimation = true;
                canRestartWalkingAnimation = true;
            }
            else if (currentAdditionalWeaponAttackAnimation != null && canPlayAdditionalAnimation)
            {
                playerWeaponAnimator.Play(currentAdditionalWeaponAttackAnimation);
                canPlayAdditionalAnimation = false;
                canRestartWalkingAnimation = true;
            }
            else
            {
                playerWeaponAnimator.Play(currentWeaponAttackAnimation);
            }
            if(playerAudioSource.isPlaying)
            {
                playerAudioSource.Stop();
            }
            playerWeaponAudioSource.PlayOneShot(playerWeaponSound);

            if (playerCurrentAmmoCount > 0 || playerCurrentAmmoCount == -1)
            {

                attackingCoroutine = StartCoroutine(AttackCooldown());
            }
            else
            {
                reloadingCoroutine = StartCoroutine(Reloading(playerReloadingCooldown));
            }
        }
    }

    private void DamageKatanaTargets()
    {
        List<GameObject> enemyTargets = attackZoneController.enemyTargets;

        for (int i = 0; i < enemyTargets.Count; i++)
        {
            if (!enemyTargets[i].GetComponent<EnemyController>().isEnemyDamaged)
            {
                enemyTargets[i].GetComponent<EnemyController>().enemyHealth -= playerWeaponDamage;
                enemyTargets[i].GetComponent<EnemyController>().isEnemyDamaged = true;
            }
        }
    }

    private void StartDash()
    {
        if (isPlayerMoving && canPlayerDash)
        {
            playerDashVector = playerMovementVector;
            dashCoroutine = StartCoroutine(Dash(dashTime, false));

        }
    }

    private void StartKatanaDash()
    {
        katanaAttack = true;
        if (!isPlayerImmortal)
        {
            isPlayerImmortal = true;
        }
        playerDashVector = transform.TransformDirection(Vector3.up);
        dashCoroutine = StartCoroutine(Dash(PlayerWeaponStats.katanaDashTime, true));
    }

    private IEnumerator Dash(float dashTime, bool katanaDash)
    {
        isPlayerDashing = true;
        yield return new WaitForSeconds(dashTime);
        isPlayerDashing = false;
        rb.velocity = Vector3.zero;
        if (isPlayerImmortal && immortalityCoroutine == null)
        {
            isPlayerImmortal = false;
        }
        if (katanaAttack)
        {
            katanaAttack = false;
        }
        if (!katanaDash)
        {
            dashCooldownCoroutine = StartCoroutine(DashCooldown());
        }
        dashCoroutine = null;
    }

    private IEnumerator DashCooldown()
    {
        canPlayerDash = false;
        yield return new WaitForSeconds(playerDashCooldown);
        canPlayerDash = true;
    }

    private IEnumerator AttackCooldown()
    {
        canPlayerAttack = false;
        yield return new WaitForSeconds(playerAttackCooldown);
        canPlayerAttack = true;
        if ((currentAdditionalWeaponIdleAnimation != null && !canPlayAdditionalAnimation) || currentAdditionalWeaponIdleAnimation == null)
        {
            playerWeaponAnimator.Play(currentWeaponIdleAnimation);
        }
        else
        {
            playerWeaponAnimator.Play(currentAdditionalWeaponIdleAnimation);
        }
        attackingCoroutine = null;
    }

    private IEnumerator PlayerFootstepsCooldown()
    {
        canPlayFootstepSound = false;
        yield return new WaitForSeconds(playerFootstepsCooldownTime);
        canPlayFootstepSound = true;
        playerFootstepsCooldownCoroutine = null;
    }

    private IEnumerator Reloading(float reloadingCooldown)
    {
        canPlayerAttack = false;
        yield return new WaitForSeconds(reloadingCooldown);
        if (playerTotalAmmoCount >= 0)
        {
            if (playerTotalAmmoCount >= playerClipAmmoCount)
            {
                playerCurrentAmmoCount = playerClipAmmoCount;
                playerTotalAmmoCount -= playerClipAmmoCount;
            }
            else
            {
                playerCurrentAmmoCount = playerTotalAmmoCount;
                playerTotalAmmoCount = 0;
            }
        }
        else if (playerTotalAmmoCount == -1)
        {
            playerCurrentAmmoCount = playerClipAmmoCount;
            
        }
        if ((currentAdditionalWeaponIdleAnimation != null && !canPlayAdditionalAnimation) || currentAdditionalWeaponIdleAnimation == null)
        {
            playerWeaponAnimator.Play(currentWeaponIdleAnimation);
        }
        else
        {
            playerWeaponAnimator.Play(currentAdditionalWeaponIdleAnimation);
        }
        canPlayerAttack = true;
        reloadingCoroutine = null;
    }

    private IEnumerator ShootingDelay()
    {
        canPlayerAttack = false;
        yield return new WaitForSeconds(0.1f);
        canPlayerAttack = true;
        shootingDelayCoroutine = null;
    }

    public void ActivateImmortality()
    {
        if (immortalityCoroutine != null)
        {
            StopCoroutine(immortalityCoroutine);
        }
        immortalityCoroutine = StartCoroutine(ImmortalityCooldown());
    }

    IEnumerator ImmortalityCooldown()
    {
        isPlayerImmortal = true;
        yield return new WaitForSeconds(immortalityTime);
        isPlayerImmortal = false;
        immortalityCoroutine = null;
    }

    public void StartGame()
    {
        SetWeaponSettings();
        GameManager.gameStarted = true;
    }
}
