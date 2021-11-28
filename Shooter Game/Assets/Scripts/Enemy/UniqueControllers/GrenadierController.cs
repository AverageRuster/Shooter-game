using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierController : MonoBehaviour
{
    //controllers
    private EnemyController enemyController = null;
    private Rigidbody2D rb;
    private AudioSource grenadierAudioSource = null;
    private ParticleSystem grenadierParticleSystem = null;
    private Animator grenadierAnimator = null;

    //stats
    private float grenadierSpeed = 0;
    private bool isGrenadierDestroyed = false;

    //statements
    private bool isAttacking = false;

    private bool allCoroutinesStopped = false;
    private bool isTakingDamage = false;

    private Vector3 enemyMovementPosition = Vector3.zero;

    private bool isPlayingDeathAnimation = false;

    [SerializeField] private GameObject grenadierLegs = null;



    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        grenadierAudioSource = GetComponent<AudioSource>();
        grenadierParticleSystem = GetComponent<ParticleSystem>();
        grenadierAnimator = GetComponent<Animator>();
        SetupStatsController();
        SetupDefaults();
        StartCoroutine(ThrowingCooldown());
    }

    private void Update()
    {
        if (!GameManager.gameOver)
        {
            if (enemyController.isEnemyActive)
            {
                CheckStats();
            }
            grenadierLegs.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
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
        enemyMovementPosition = Vector3.zero;
        isGrenadierDestroyed = false;
        grenadierSpeed = 0;
        isAttacking = false;
        isPlayingDeathAnimation = false;
        allCoroutinesStopped = false;
        isTakingDamage = false;
        enemyController.ResetStats();
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.grenadierType;
        enemyController.maxEnemyHealth = EnemyStats.maxGrenadierHealth;
        enemyController.enemyDestroyPrice = EnemyStats.grenadierDestroyPrice;
    }

    private void CheckStats()
    {
        isGrenadierDestroyed = enemyController.isEnemyDestroyed;        
        if (isGrenadierDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isPlayingDeathAnimation)
            {
                grenadierAnimator.Play("GrenadierDeath");
                isPlayingDeathAnimation = true;
            }
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
            bool allSystemsStopped = !grenadierAudioSource.isPlaying && !grenadierParticleSystem.isPlaying;
            if (allSystemsStopped)
            {
                SetupDefaults();
            }
        }
        else
        {
            isTakingDamage = enemyController.isEnemyTakingDamage;
            enemyMovementPosition = PlayerController.playerPosition;                        
            grenadierSpeed = enemyController.currentEnemySpeed;

            LookAtPlayer();
            ThrowGrenade();
        }     
    }

    private void ThrowGrenade()
    {
        if (!isAttacking)
        {
            GameObject grenade = ObjectPooler.GetPooledEnemyProjectile(EnemyStats.grenadierProjectileType);
            if (grenade != null)
            {
 
                grenadierAnimator.Play("GrenadierAttackEnd");
                enemyController.canPlayAttackSound = true;
                grenade.transform.position = transform.position;
                grenade.transform.rotation = transform.rotation;
                grenade.GetComponent<GrenadierProjectileController>().projectileDamage = EnemyStats.grenadierDamage;

                grenade.GetComponent<GrenadierProjectileController>().explosionCooldownTime = 1.5f;
                grenade.SetActive(true);
                StartCoroutine(ThrowingCooldown());
            }
        }
    }

    IEnumerator ThrowingCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(EnemyStats.grenadierThrowingCooldown);
        isAttacking = false;
    }

    private void LookAtPlayer()
    {
        if (!isTakingDamage)
        {
            if (enemyMovementPosition.x - transform.position.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (enemyMovementPosition - transform.position)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (enemyMovementPosition - transform.position)));
            }

            grenadierLegs.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void PlayAttackIdleAnimation()
    {
        grenadierAnimator.Play("GrenadierAttackIdle");
    }

    public void PlayAttackAnimation()
    {
        Debug.Log("C");
        grenadierAnimator.Play("GrenadierAttack");
    }
    
}

