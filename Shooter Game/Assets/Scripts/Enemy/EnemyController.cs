using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer enemySpriteRenderer = null;

    [HideInInspector] public int enemyType = 0;
    [HideInInspector] public float maxEnemyHealth = 0;
    [HideInInspector] public float maxEnemySpeed = 0;

    [HideInInspector] public int enemyDestroyPrice = 0;
    [HideInInspector] public float enemyHealth = 0;
    [HideInInspector] public float currentEnemyHealth = 0;
    [HideInInspector] public float currentEnemySpeed = 0;
    [HideInInspector] public bool canAttack = false;
    [HideInInspector] public GameObject target = null;
    [HideInInspector] public bool isEnemyDestroyed = false;
    [HideInInspector] public bool canPlayAttackSound = false;
    [HideInInspector] public bool isEnemyTakingDamage = false;
    [HideInInspector] public float takingDamageResetTime = 0;
    [HideInInspector] public bool isEnemyDamaged = false;

    [SerializeField] private GameObject enemyBody;
    [HideInInspector] public Vector3 enemyBodyPosition = Vector3.zero;
    [HideInInspector] public Quaternion enemyBodyRotation = Quaternion.Euler(0, 0, 0);

    private const float appearanceTime = 2;



    private Color currentEnemyColor;
    private Collider2D enemyCollider = null;
    private ParticleSystem enemyParticleSystem = null;
    private Coroutine takingDamageResetCoroutine = null;
    private Coroutine enemyColorChangeCooldown = null;
    private AudioSource enemyAudioSource = null;
    [SerializeField] private AudioClip enemyAttackSound = null;
    [SerializeField] private AudioClip[] enemyHitSound = null;
    private bool allCoroutinesStopped = false;
    public bool isEnemyActive = false;
    [SerializeField] private GameObject enemyAppearanceLocation = null;


    private void Start()
    {
        enemyParticleSystem = GetComponent<ParticleSystem>();
        enemyAudioSource = GetComponent<AudioSource>();
        enemyCollider = GetComponent<Collider2D>();
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
        //------------------------------------------------------//
        enemyAppearanceLocation.SetActive(true);
        currentEnemyColor = enemySpriteRenderer.color;
        currentEnemyColor.a = 0;
        enemySpriteRenderer.color = currentEnemyColor;
        //------------------------------------------------------//   
        enemySpriteRenderer.sortingLayerName = "Enemy";
    }

    private void Update()
    {
        if (!GameManager.gameOver)
        {
            if (enemySpriteRenderer.color.a < 1 && enemyColorChangeCooldown == null)
            {
                enemyColorChangeCooldown = StartCoroutine(EnemyColorChangeCooldown());
            }

            if (!PlayerController.katanaAttack && isEnemyDamaged)
            {
                isEnemyDamaged = false;
            }

            if (currentEnemyHealth != enemyHealth)
            {
                if (isEnemyTakingDamage)
                {
                    StopCoroutine(takingDamageResetCoroutine);

                    takingDamageResetCoroutine = StartCoroutine(TakingDamageReset());
                }
                else
                {
                    takingDamageResetCoroutine = StartCoroutine(TakingDamageReset());
                    isEnemyTakingDamage = true;
                }

                enemyParticleSystem.Play();
                if (enemyHitSound != null)
                {
                    AudioClip randomEnemyHitSound = enemyHitSound[Random.Range(0, enemyHitSound.Length)];
                    if (!enemyAudioSource.isPlaying)
                    {
                        enemyAudioSource.PlayOneShot(randomEnemyHitSound);
                    }
                }
                currentEnemyHealth = enemyHealth;
            }
            if (canPlayAttackSound)
            {
                if (enemyAudioSource.isPlaying)
                {
                    enemyAudioSource.Stop();
                }
                enemyAudioSource.PlayOneShot(enemyAttackSound);
                canPlayAttackSound = false;
            }
            if (currentEnemyHealth <= 0 && !isEnemyDestroyed)
            {
                DropSoul();
                //CheckLootDrop();
                enemyCollider.enabled = false;
                enemySpriteRenderer.sortingLayerName = "Body";
                
                SpawnController.enemiesDestroyed++;
                isEnemyDestroyed = true;
            }
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

    private IEnumerator TakingDamageReset()
    {
        yield return new WaitForSeconds(takingDamageResetTime);
        isEnemyTakingDamage = false;
    }

    public void ResetStats()
    {
        isEnemyActive = false;
        enemyHealth = maxEnemyHealth;
        currentEnemyHealth = enemyHealth;
        currentEnemySpeed = maxEnemySpeed;
        isEnemyDamaged = false;
        target = null;
        canAttack = false;
        isEnemyDestroyed = false;
        canPlayAttackSound = false;
        isEnemyTakingDamage = false;
        
        if (!enemyCollider.enabled)
        {
            enemyCollider.enabled = true;
        }

            
    }

    private void CheckLootDrop()
    {
        int dropChance = Random.Range(1, 101);
        int dropType = Random.Range(0, 3); //fix
        if (dropChance <= 10) //fix
        {    
            /*
            GameObject enemyDrop = ObjectPooler.GetPooledDrop(dropType);
            if (enemyDrop != null)
            {
                enemyDrop.transform.position = transform.position;
                enemyDrop.SetActive(true);
            }
            */
        }
    }

    public void StartAppearance()
    {
        /*
        if (enemyAppearanceCoroutine != null)
        {
            StopCoroutine(enemyAppearanceCoroutine);
        }
        enemyAppearanceCoroutine = StartCoroutine(EnemyAppearanceCooldown());
        */
    }

    public void DeactivateEnemy()
    {
        Instantiate(enemyBody, enemyBodyPosition, enemyBodyRotation);
        
        //------------------------------------------------------//
        enemyAppearanceLocation.SetActive(true);
        currentEnemyColor = enemySpriteRenderer.color;
        currentEnemyColor.a = 0;
        enemySpriteRenderer.color = currentEnemyColor;
        //------------------------------------------------------//   
        enemySpriteRenderer.sortingLayerName = "Enemy";
        gameObject.SetActive(false);
    }

    /*
    public IEnumerator DeactivateEnemy(GameObject body, Vector3 bodyPosition, Quaternion bodyRotation)
    {
        yield return new WaitForSeconds(3);
        Instantiate(body, bodyPosition, bodyRotation);
        //------------------------------------------------------//
        enemyAppearanceLocation.SetActive(true);
        currentEnemyColor = enemySpriteRenderer.color;
        currentEnemyColor.a = 0;
        enemySpriteRenderer.color = currentEnemyColor;
        //------------------------------------------------------//   
        enemySpriteRenderer.sortingLayerName = "Enemy";
        gameObject.SetActive(false);
    }
    */

    private void DropSoul()
    {
        GameObject soul = ObjectPooler.GetPooledSoul();
        SoulController soulController = soul.GetComponent<SoulController>();
        soul.transform.position = transform.position;
        soulController.soulCount = enemyDestroyPrice;
        soul.SetActive(true);
        soulController.StartCoroutine(soulController.DeactivasionCooldown());
    }

    private IEnumerator EnemyColorChangeCooldown()
    {
        
        yield return new WaitForSeconds(appearanceTime/10);
        currentEnemyColor = enemySpriteRenderer.color;
        currentEnemyColor.a += appearanceTime / 10;
        enemySpriteRenderer.color = currentEnemyColor;
        if (enemySpriteRenderer.color.a >= 1)
        {
            enemyAppearanceLocation.SetActive(false);
            isEnemyActive = true;           
        }
        enemyColorChangeCooldown = null;
    }
}
