using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //sound
    [SerializeField] private AudioSource cameraAudioSource = null;
    [SerializeField] private AudioClip shoppingBackgroundSound = null;
    [SerializeField] private AudioClip gameplayBackgroundSound = null;

    //UI
    [SerializeField] private TextMeshProUGUI gameScoreText = null;
    [SerializeField] private TextMeshProUGUI enemiesLeftText = null;
    [SerializeField] private TextMeshProUGUI wavesCounterText = null;
    [SerializeField] private TextMeshProUGUI playerMoneyText = null;
    [SerializeField] private TextMeshProUGUI shoppingTimerText = null;
    [SerializeField] private GameObject shoppingTimer = null;
    [SerializeField] private GameObject enemiesCounter = null;

    [SerializeField] private GameObject restartMenuButton = null;
    [SerializeField] private GameObject continueMenuButton = null;
    [SerializeField] private GameObject mainUI = null;
    [SerializeField] private GameObject menuUI = null;
    [SerializeField] private GameObject shopUI = null;
    [SerializeField] private GameObject upgradeUI = null;
    [SerializeField] private GameObject mainMenuMainUI = null;
    [SerializeField] private GameObject mainMenuMainGamemodesUI = null;
    [SerializeField] private GameObject mainMenuMissionsGamemodeUI = null;
    [SerializeField] private GameObject shopButton = null;
    private GameObject currentWeaponUpgradeMenu = null;

    [SerializeField] private GameObject spawnController = null;

    private float shoppingTime = 30;

    //difficult
    [HideInInspector] public static int gameDifficultLevel = 0;
    private bool canRaiseDifficultLevel = false;
    private float raiseDifficultLevelCooldownTime = 30;

    //scores
    [HideInInspector] public static float playerSouls = 0;
    public static int enemiesLeft = 0;

    [HideInInspector] public static bool gameOver = false;
    
    private static bool gameOverMenuOpened = false;
    private bool allCoroutinesStopped = false;


    public static bool gameStarted = false;
    private bool gameActivated = false;
    private int gamemodeID = 0;                                             //0 - survival, 1 - missions
    private int missionsID = 0;

    public static bool[] weaponPurchased = new bool[7];

    private void Start()
    {
        SetupDefaults();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            if (gameStarted && !gameActivated)
            {
                if (!spawnController.activeSelf)
                {
                    spawnController.SetActive(true);
                }
                if (!mainUI.activeSelf)
                {
                    mainUI.SetActive(true);
                }
                gameActivated = true;
            }
            CheckGameOver();
            //gameScoreText.SetText(gameScore.ToString());
            enemiesLeftText.SetText(enemiesLeft.ToString());
            wavesCounterText.SetText("WAVE " + SpawnController.wave.ToString() + "I5");
            playerMoneyText.SetText(playerSouls.ToString());

            RaiseDifficultLevel();
        }
    }

    private void SetupDefaults()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            cameraAudioSource.PlayOneShot(gameplayBackgroundSound);
        }
        gameStarted = false;
        gameActivated = false;
        gameOver = false;
        gameOverMenuOpened = false;
        allCoroutinesStopped = false;
        gamemodeID = 0;
        missionsID = 0;
        playerSouls = 0;
        gameDifficultLevel = 0;
        canRaiseDifficultLevel = false;
        Time.timeScale = 1;
        shoppingTime = 30;
        for (int i = 1; i < 7; i++)
        {
            weaponPurchased[i] = false;
        }
        weaponPurchased[0] = true;
        StartCoroutine(RaiseDifficultLevelCooldown());
    }

    private void RaiseDifficultLevel()
    {
        //Debug.Log("DIFF" + gameDifficultLevel);
        if(canRaiseDifficultLevel)
        {
            gameDifficultLevel++;
            StartCoroutine(RaiseDifficultLevelCooldown());
        }
    }

    IEnumerator RaiseDifficultLevelCooldown()
    {
        canRaiseDifficultLevel = false;
        yield return new WaitForSeconds(raiseDifficultLevelCooldownTime);
        canRaiseDifficultLevel = true;
    }

    private void CheckGameOver()
    {
        if (gameOver)
        {
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
            if (!restartMenuButton.activeSelf)
            {
                restartMenuButton.SetActive(true);
            }
            if (continueMenuButton.activeSelf)
            {
                continueMenuButton.SetActive(false);
            }
            if (!gameOverMenuOpened)
            {
                OpenMenuUI();
                gameOverMenuOpened = true;

            }
            if (BackgroundController.isFloorUpdated)
            {
                BackgroundController.isFloorUpdated = false;
            }
        }
    }

    public void StartNewWave()
    {
        enemiesCounter.SetActive(false);
        shoppingTimer.SetActive(true);
        if (!shopButton.activeSelf)
        {
            shopButton.SetActive(true);
        }
        StartCoroutine(BackgroundSondsSwitch(shoppingBackgroundSound));
        StartCoroutine(ShoppingCooldown());

    }

    private IEnumerator BackgroundSondsSwitch(AudioClip backgroundSound)
    {
        while (cameraAudioSource.volume > 0)
        {
            cameraAudioSource.volume -= 0.01f;
            yield return new WaitForSeconds(0.15f);
        }
        cameraAudioSource.Stop();
        cameraAudioSource.PlayOneShot(backgroundSound);
        while (cameraAudioSource.volume < 0.1f)
        {
            cameraAudioSource.volume += 0.01f;
            yield return new WaitForSeconds(0.15f);
        }

        if (cameraAudioSource.volume > 0.1f)
        {
            cameraAudioSource.volume = 0.1f;
        }
    }

    private IEnumerator ShoppingCooldown()
    {
        while (shoppingTime > 0)
        {
            shoppingTimerText.SetText(shoppingTime.ToString());
            yield return new WaitForSeconds(1);
            shoppingTime--;
        }
        shoppingTime = 30;
        CloseShopUI();
        SpawnController.enemiesSpawnedInWave = 0;
        if (shopButton.activeSelf)
        {
            shopButton.SetActive(false);
            
        }
        if (shopUI.activeSelf)
        {
            shopUI.SetActive(false);

        }
        if (upgradeUI.activeSelf)
        {
            upgradeUI.SetActive(false);

        }
        if (!mainUI.activeSelf)
        {
            mainUI.SetActive(true);
        }

        enemiesCounter.SetActive(true);
        shoppingTimer.SetActive(false);
        StartCoroutine(BackgroundSondsSwitch(gameplayBackgroundSound));
        SpawnController.newWaveStarting = false;

    }

    #region Gameplay Buttons

    public void OpenMenuUI()
    {
        Time.timeScale = 0;
        if (mainUI.activeSelf)
        {
            mainUI.SetActive(false);
        }
        if (shopUI.activeSelf)
        {
            shopUI.SetActive(false);
        }
        PlayerController.touchBlocked = true;
        menuUI.SetActive(true);       
    }

    public void OpenShopUI()
    {
        if (mainUI.activeSelf)
        {
            mainUI.SetActive(false);
        }
        if (menuUI.activeSelf)
        {
            menuUI.SetActive(false);
        }
        PlayerController.touchBlocked = true;
        shopUI.SetActive(true);
    }

    public void CloseMenuUI()
    {
        PlayerController.touchBlocked = false;
        menuUI.SetActive(false);
        mainUI.SetActive(true);
        Time.timeScale = 1;
    }

    public void CloseShopUI()
    {
        PlayerController.touchBlocked = false;
        shopUI.SetActive(false);
        mainUI.SetActive(true);
    }

    public void BuyOrSelectWeapon(int weaponType)
    {
        if (!weaponPurchased[weaponType] && playerSouls >= PlayerWeaponStats.weaponPrices[weaponType])
        {
            playerSouls -= PlayerWeaponStats.weaponPrices[weaponType];
            weaponPurchased[weaponType] = true;
            PlayerController.playerWeaponType = weaponType;
        }
        else if (weaponPurchased[weaponType] && PlayerController.playerWeaponType != weaponType)
        {
            PlayerController.playerWeaponType = weaponType;
        }
    }

    public void OpenWeaponUpgradeMenu(GameObject upgradeMenu)
    {
        shopUI.SetActive(false);
        upgradeUI.SetActive(true);
        currentWeaponUpgradeMenu = upgradeMenu;
        currentWeaponUpgradeMenu.SetActive(true);
    }

    public void CloseWeaponUpgradeMenu()
    {
        currentWeaponUpgradeMenu.SetActive(false);
        upgradeUI.SetActive(false);       
        shopUI.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        
        SceneManager.LoadScene(currentScene.name);       
    }

    #endregion

    #region Main Menu Buttons

    public void OpenGamemodesUI()
    {
        mainMenuMainUI.SetActive(false);
        mainMenuMainGamemodesUI.SetActive(true);
    }

    public void CloseGamemodesUI()
    {

        mainMenuMainGamemodesUI.SetActive(false);
        mainMenuMainUI.SetActive(true);
    }

    public void OpenMissionsGamemodeUI()
    {
        mainMenuMainGamemodesUI.SetActive(false);
        mainMenuMissionsGamemodeUI.SetActive(true);
    }

    public void CloseMissionsGamemodeUI()
    {
        mainMenuMissionsGamemodeUI.SetActive(false);
        mainMenuMainGamemodesUI.SetActive(true);
    }

    public void SetupAndLaunchSurvivalMode()
    {
        gamemodeID = 0;
        SceneManager.LoadScene("Gameplay");
    }

    public void SetupAndLaunchMissionsMode()
    {
        gamemodeID = 1;
        SceneManager.LoadScene("Gameplay");
    }
   
    #endregion
}
