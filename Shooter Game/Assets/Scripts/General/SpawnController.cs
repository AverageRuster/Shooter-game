using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    //enemies
    private const int totalEnemyTypes = 6;

    public static int wave = 1;
    public static int enemiesSpawnedInWave = 0;
    public static bool newWaveStarting = false;
    public static int enemiesDestroyed = 0;

    private int enemiesToSpawnAtOnce = 0;
    private int enemiesCount = 0;
    private float enemySpawnCooldownTime = 1.5f;
    private bool canSpawnEnemy = true;
    private bool allCoroutinesStopped = false;
    
    private int enemyTypesInWave = 0;
    private int enemiesToSpawnInWave = 0;
    
    private int[] enemyTypesSpawnedInWave = new int[totalEnemyTypes];
    private int[] enemyTypesToSpawnInWave = new int[totalEnemyTypes];
    
    private bool[] isEnemyTypeSpawnDisabled = new bool[totalEnemyTypes];
    int[] enemyTypes = new int[totalEnemyTypes];



    private void Start()
    {
        SetupDefaults();
        
    }

    private void Update()
    {
        if (!GameManager.gameOver)
        {
            enemiesCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            GameManager.enemiesLeft = enemiesToSpawnInWave - enemiesDestroyed;
            SpawnEnemy();
        }
        else
        {
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
        }
    }

    private void SetupDefaults()
    {
        wave = 1;
        enemiesToSpawnAtOnce = 10;
        enemiesCount = 0;
        enemySpawnCooldownTime = 1.5f;
        enemiesSpawnedInWave = 0;
        enemiesToSpawnInWave = 0;
        enemiesDestroyed = 0;
        enemyTypesInWave = 0;
        canSpawnEnemy = true;
        allCoroutinesStopped = false;
        newWaveStarting = false;
        SetEnemiesToSpawnCount();
    }

    private void SpawnEnemy()
    {
        if (enemiesSpawnedInWave >= enemiesToSpawnInWave && enemiesCount == 0 && !newWaveStarting)
        {
            if (wave < 6)
            {
                newWaveStarting = true;
                wave++;
                enemiesDestroyed = 0;
                for (int i = 0; i < totalEnemyTypes; i++)
                {
                    enemyTypes[i] = i;
                    isEnemyTypeSpawnDisabled[i] = false;
                    enemyTypesSpawnedInWave[i] = 0;
                }
                SetEnemiesToSpawnCount();
                gameManager.StartNewWave();             
            }
            else
            {
                GameManager.gameOver = true;
            }
        }


        if (canSpawnEnemy && enemiesCount < enemiesToSpawnAtOnce && enemiesSpawnedInWave < enemiesToSpawnInWave && !newWaveStarting)
            {
                int enemyType = SetEnemyType();
                GameObject enemy = ObjectPooler.GetPooledEnemy(enemyType);
                if (enemy != null)
                {
                    enemy.transform.position = SelectSpawnPosition();
                    enemy.transform.rotation = transform.transform.rotation;
                    enemy.SetActive(true);
                    //enemy.GetComponent<EnemyController>().StartAppearance();
                    enemiesSpawnedInWave++;
                }
            }
        
    }

    private Vector3 SelectSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        float xPos = Random.Range(-24, 25);
        float yPos = Random.Range(-24, 25);
        spawnPosition = new Vector3(xPos, yPos);
        return spawnPosition;
    }


    private int SetEnemyType()
    {
        // 0 - walker

        // 1 - striker
        // 2 - berserk
        // 3 - kamikaze
        // 4 - cutter
        // 5 - grenadier    
        int enemyTypesCounter = 0;
        for (int i = 0; i < totalEnemyTypes; i++)
        {
            if (enemyTypesToSpawnInWave[i] > 0)
            {
                enemyTypes[enemyTypesCounter] = i;
                enemyTypesCounter++;
                if (enemyTypesSpawnedInWave[i] >= enemyTypesToSpawnInWave[i] && !isEnemyTypeSpawnDisabled[i])
                {
                    for (int j = i; j < totalEnemyTypes; j++)
                    {
                        enemyTypes[j]++;
                    }
                    enemyTypesInWave--;
                    isEnemyTypeSpawnDisabled[i] = true;
                }
            }
        }

        
        int currentEnemyType = Random.Range(0, enemyTypesInWave);

        enemyTypesSpawnedInWave[enemyTypes[currentEnemyType]]++;
        return enemyTypes[currentEnemyType];
    }

    private void SetEnemiesToSpawnCount()
    {
        enemyTypesInWave = 0;
        enemiesToSpawnInWave = 0;
        if (wave == 1)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[1] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[2] = 0;
            enemyTypesToSpawnInWave[3] = 0;
            enemyTypesToSpawnInWave[4] = 0;
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 5;
        }
        else if (wave == 2)
        {
            enemyTypesToSpawnInWave[0] = 0;
            enemyTypesToSpawnInWave[1] = 0;
            enemyTypesToSpawnInWave[2] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[3] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[4] = 0;
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 5;
        }
        else if (wave == 3)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[1] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[2] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[3] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[4] = 0;
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 10;
        }
        else if (wave == 4)
        {
            enemyTypesToSpawnInWave[0] = 0;
            enemyTypesToSpawnInWave[1] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[2] = 0;
            enemyTypesToSpawnInWave[3] = 0;
            enemyTypesToSpawnInWave[4] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 10;
        }
        else if (wave == 5)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[1] = 0;
            enemyTypesToSpawnInWave[2] = 0;
            enemyTypesToSpawnInWave[3] = 0;
            enemyTypesToSpawnInWave[4] = Random.Range(15, 21);
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 10;
        }

        
        else if (wave == 6)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[1] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[2] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[3] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[4] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[5] = 0;
            enemiesToSpawnAtOnce = 10;
        }
        else if (wave == 7)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[1] = 0;
            enemyTypesToSpawnInWave[2] = 0;
            enemyTypesToSpawnInWave[3] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[4] = 0;
            enemyTypesToSpawnInWave[5] = Random.Range(5, 11);
            enemiesToSpawnAtOnce = 20;
        }
        else if (wave == 8)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[1] = 0;
            enemyTypesToSpawnInWave[2] = 0;
            enemyTypesToSpawnInWave[3] = 0;
            enemyTypesToSpawnInWave[4] = 0;
            enemyTypesToSpawnInWave[5] = Random.Range(5, 11);
            enemiesToSpawnAtOnce = 20;
        }
        else if (wave == 9)
        {
            enemyTypesToSpawnInWave[0] = 0;
            enemyTypesToSpawnInWave[1] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[2] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[3] = 0;
            enemyTypesToSpawnInWave[4] = Random.Range(10, 16);
            enemyTypesToSpawnInWave[5] = Random.Range(5, 11);
            enemiesToSpawnAtOnce = 20;
        }
        else if (wave == 10)
        {
            enemyTypesToSpawnInWave[0] = Random.Range(5, 11);
            enemyTypesToSpawnInWave[1] = Random.Range(5, 11);
            enemyTypesToSpawnInWave[2] = Random.Range(5, 11);
            enemyTypesToSpawnInWave[3] = Random.Range(5, 11);
            enemyTypesToSpawnInWave[4] = Random.Range(5, 11);
            enemyTypesToSpawnInWave[5] = Random.Range(5, 11);
            enemiesToSpawnAtOnce = 20;
        }
        
        for (int i = 0; i < totalEnemyTypes; i++)
        {
            enemiesToSpawnInWave += enemyTypesToSpawnInWave[i];
            if (enemyTypesToSpawnInWave[i] > 0)
            {
                enemyTypesInWave++;
            }
        }
        
    }
    private void SetEnemiesSpawnCoooldown()
    {
        if (GameManager.gameDifficultLevel > 10 && enemySpawnCooldownTime != 1.5f - (GameManager.gameDifficultLevel - 10) / 10 && enemySpawnCooldownTime > 0.5f)
        {
            enemySpawnCooldownTime -= 0.1f;
        }
        else if (GameManager.gameDifficultLevel < 10 && enemySpawnCooldownTime != 1.5f)
        {
            enemySpawnCooldownTime = 1.5f;
        }
    }
}
