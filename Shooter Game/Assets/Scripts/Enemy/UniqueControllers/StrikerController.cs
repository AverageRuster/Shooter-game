using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerController : MonoBehaviour
{
    //controlers
    private EnemyController enemyController = null;
    private Animator strikerAnimator;



    private AudioSource strikerAudioSource = null;
    private ParticleSystem strikerParticleSystem = null;

    //statements
    private bool isStrikerDestroyed = false;
    private float strikerSpeed = 0;
    private bool isTakingDamage = false;
    private bool isPlayingWalkingAnimation = false;
    private bool isPlayingDeathAnimation = false;

    private bool isAttacking = false;
    private bool canAttack = false;
    private Rigidbody2D rb;
    private Vector3 strikerMovementPosition = Vector3.zero;
    private GameObject target;
    private bool allCoroutinesStopped = false;


    [SerializeField] private GameObject attackZone;
    [SerializeField] private GameObject strikerBody;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        strikerAudioSource = GetComponent<AudioSource>();
        strikerParticleSystem = GetComponent<ParticleSystem>();
        strikerAnimator = GetComponent<Animator>();
        SetupStatsController();
        SetupDefaults();      
    }

    void Update()
    {

        if (!GameManager.gameOver)
        {
            if (enemyController.isEnemyActive)
            {
                CheckStats();
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

    private void FixedUpdate()
    {
        if (!isStrikerDestroyed)
        {
            if (enemyController.isEnemyActive)
            {
                if (!GameManager.gameOver && !isAttacking && !canAttack && !isTakingDamage)
                {
                    rb.MovePosition(transform.position + (strikerMovementPosition - transform.position).normalized * strikerSpeed * Time.fixedDeltaTime);
                    if (!isPlayingWalkingAnimation)
                    {
                        strikerAnimator.Play("StrikerWalk");
                        isPlayingWalkingAnimation = true;
                    }
                }
                else
                {
                    if (isPlayingWalkingAnimation)
                    {
                        strikerAnimator.Play("StrikerIdle");
                        isPlayingWalkingAnimation = false;
                    }
                }
            }
        }
    }

    private void SetupDefaults()
    {     
        strikerSpeed = 0;
        isAttacking = false;
        canAttack = false;
        strikerMovementPosition = Vector3.zero;
        target = null;
        allCoroutinesStopped = false;
        isStrikerDestroyed = false;
        isTakingDamage = false;
        isPlayingDeathAnimation = false;
        isPlayingWalkingAnimation = false;
        enemyController.ResetStats();        
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.strikerType;
        enemyController.maxEnemyHealth = EnemyStats.maxStrikerHealth;
        enemyController.maxEnemySpeed = EnemyStats.maxStrikerSpeed;
        enemyController.enemyDestroyPrice = EnemyStats.strikerDestroyPrice;
        enemyController.takingDamageResetTime = EnemyStats.strikerTakingDamageResetTime;
    }

    private void CheckStats()
    {
        isStrikerDestroyed = enemyController.isEnemyDestroyed;

        if (isStrikerDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isPlayingDeathAnimation)
            {
                strikerAnimator.Play("StrikerDeath");
                isPlayingDeathAnimation = true;
            }
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }

            bool canStrikerBeDeactivated = !strikerAudioSource.isPlaying && !strikerParticleSystem.isPlaying;
            if (canStrikerBeDeactivated)
            {
                SetupDefaults();

            }
        }
        else 
        {
            isTakingDamage = enemyController.isEnemyTakingDamage;
            strikerSpeed = enemyController.currentEnemySpeed;
            canAttack = enemyController.canAttack;
            target = enemyController.target;
            strikerMovementPosition = PlayerController.playerPosition;
            LookAtPlayer();
            Shoot();
        }      
    }

    public void Shoot()
    {
        if (!isAttacking && canAttack)
        {
            GameObject enemyProjectile = ObjectPooler.GetPooledEnemyProjectile(EnemyStats.strikerProjectileType);
            if (enemyProjectile != null)
            {
                enemyController.canPlayAttackSound = true;
                enemyProjectile.transform.position = transform.position;
                enemyProjectile.transform.rotation = transform.rotation;
                enemyProjectile.GetComponent<StrikerProjectileController>().projectileDamage = EnemyStats.strikerDamage;
                enemyProjectile.GetComponent<StrikerProjectileController>().projectileSpeed = EnemyStats.strikerProjectileSpeed;
                enemyProjectile.SetActive(true);
                StartCoroutine(ShootingCooldown());
            }
        }
    }

    IEnumerator ShootingCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(EnemyStats.strikerShootingCooldown);
        isAttacking = false;
    }

    private void LookAtPlayer()
    {
        if (!isTakingDamage && !isAttacking)
        {
            if (strikerMovementPosition.x - transform.position.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (strikerMovementPosition - transform.position)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (strikerMovementPosition - transform.position)));
            }
        }
    }
}

