using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkController : MonoBehaviour
{
    //controllers   
    private EnemyController enemyController = null;
    private AudioSource berserkAudioSource = null;
    private ParticleSystem berserkParticleSystem = null;
    private Animator berserkAnimator = null;
    

    //statements
    private float berserkSpeed = 0;
    private bool allCoroutinesStopped = false;
    private bool isBerserkDestroyed = false;
    private bool isTakingDamage = false;

    private bool isAttacking = false;
    private bool canAttack = false;
    private Rigidbody2D rb;
    private Vector3 berserkMovementPosition = Vector3.zero;
    private GameObject target = null;
    private bool isPlayingWalkingAnimation = false;

    [SerializeField] private GameObject berserkBody = null;
    private bool isPlayingDeathAnimation = false;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        berserkAudioSource = GetComponent<AudioSource>();
        berserkAnimator = GetComponent<Animator>();

        berserkParticleSystem = GetComponent<ParticleSystem>();
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
            if (enemyController.isEnemyActive && !isBerserkDestroyed)
            {
                if (!GameManager.gameOver && !isTakingDamage && !isAttacking )
                {

                    if (rb.velocity.magnitude <= EnemyStats.berserkMaxSpeedLimit)
                    {
                        rb.AddForce((berserkMovementPosition - transform.position).normalized * berserkSpeed * Time.deltaTime);
                    }
                    if (!isPlayingWalkingAnimation)
                    {
                        berserkAnimator.Play("BerserkWalk");
                        isPlayingWalkingAnimation = true;
                    }

                }
                else
                {
                    rb.velocity = Vector3.zero;
                    if (isPlayingWalkingAnimation)
                    {
                        berserkAnimator.Play("BerserkIdle");
                        isPlayingWalkingAnimation = false;
                    }
                }
            }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }

    private void SetupDefaults()
    {
        berserkSpeed = 0;
        allCoroutinesStopped = false;
        isAttacking = false;
        canAttack = false;
        berserkMovementPosition = Vector3.zero;
        target = null;
        isBerserkDestroyed = false;
        isTakingDamage = false;
        isPlayingWalkingAnimation = false;
        isPlayingDeathAnimation = false;

        enemyController.ResetStats();
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.berserkType;
        enemyController.maxEnemyHealth = EnemyStats.maxBerserkHealth;
        enemyController.maxEnemySpeed = EnemyStats.maxBerserkSpeed;
        enemyController.enemyDestroyPrice = EnemyStats.berserkDestroyPrice;
        enemyController.takingDamageResetTime = EnemyStats.berserkTakingDamageResetTime;
    }

    private void CheckStats()
    {
        isBerserkDestroyed = enemyController.isEnemyDestroyed;
        if (isBerserkDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isPlayingDeathAnimation)
            {
                berserkAnimator.Play("BerserkDeath");
                isPlayingDeathAnimation = true;
            }
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }

            bool canBerserkBeDeactivated = !berserkAudioSource.isPlaying && !berserkParticleSystem.isPlaying;
            if (canBerserkBeDeactivated)
            {
                SetupDefaults();

            }
        }
        else
        {
            isTakingDamage = enemyController.isEnemyTakingDamage;
            berserkSpeed = enemyController.currentEnemySpeed;
            canAttack = enemyController.canAttack;
            target = enemyController.target;
            berserkMovementPosition = PlayerController.playerPosition;
            if (!isAttacking && !isBerserkDestroyed)
            {
                LookAtPlayer();
            }
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (!isAttacking && canAttack)
        {
            enemyController.canPlayAttackSound = true;
            if (!target.GetComponent<PlayerController>().isPlayerImmortal)
            {
                target.GetComponent<PlayerController>().playerHealth -= EnemyStats.berserkDamage;
            }
            StartCoroutine(MeleeCooldown());
        }
    }

    IEnumerator MeleeCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(EnemyStats.berserkMeleeCooldown);
        isAttacking = false;
    }

    private void LookAtPlayer()
    {
        if (!isTakingDamage)
        {
            if (berserkMovementPosition.x - transform.position.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (berserkMovementPosition - transform.position)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (berserkMovementPosition - transform.position)));
            }
        }
    }
}
