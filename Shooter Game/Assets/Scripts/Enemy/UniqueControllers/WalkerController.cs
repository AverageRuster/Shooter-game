using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{
    //controllers   
    private EnemyController enemyController = null;
    [SerializeField] private WalkerAttackZoneController walkerAttackZoneController = null;
    [SerializeField] private GameObject walkerBody;
    private AudioSource walkerAudioSource = null;
    private ParticleSystem walkerParticleSystem = null;
    private Animator walkerAnimator = null;


    //statements
    private float walkerSpeed = 0;
    private float walkerAttackSpeed = 5000;
    private bool allCoroutinesStopped = false;
    private bool isWalkerDestroyed = false;
    private bool isTakingDamage = false;
    private float attackTime = 0.5f;
    private float attackPreparationTime = 0.5f;
    private float attackCooldownTime = 1;
    private bool isPreparing = false;
    private bool isDamaging = false;
    private bool isAttacking = false;
    private bool canAttack = false;
    private bool attackCooldown = false;
    private Rigidbody2D rb;
    private Vector3 walkerMovementPosition = Vector3.zero;
    private Vector3 walkerAttackVector = Vector3.zero;
    private GameObject target = null;
    private bool isPlayingWalkingAnimation = false;
    private bool isPlayingDeathAnimation = false;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        walkerAudioSource = GetComponent<AudioSource>();
        walkerAnimator = GetComponent<Animator>();

        walkerParticleSystem = GetComponent<ParticleSystem>();
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

            if (enemyController.isEnemyActive && !isWalkerDestroyed)
            {
                if (!GameManager.gameOver && !isPreparing && !attackCooldown && !isTakingDamage )
                {
                        if (!isAttacking)
                        {
                            rb.MovePosition(transform.position + (walkerMovementPosition - transform.position).normalized * walkerSpeed * Time.fixedDeltaTime);

                            if (!isPlayingWalkingAnimation)
                            {
                                walkerAnimator.Play("WalkerWalk");
                                isPlayingWalkingAnimation = true;
                            }
                        }
                        else
                        {
                            if (rb.velocity.magnitude < 10)
                            {
                                rb.AddForce(walkerAttackVector * walkerAttackSpeed * Time.fixedDeltaTime);
                            }
                        }
                    
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    if (isPlayingWalkingAnimation)
                    {
                        walkerAnimator.Play("WalkerIdle");
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
        isPlayingDeathAnimation = false;

        walkerSpeed = 0;
        walkerAttackSpeed = 5000;
        allCoroutinesStopped = false;
        isWalkerDestroyed = false;
        isTakingDamage = false;

        isPreparing = false;
        isDamaging = false;
        isAttacking = false;
        canAttack = false;
        attackCooldown = false;

        walkerMovementPosition = Vector3.zero;
        walkerAttackVector = Vector3.zero;
        target = null;
        isPlayingWalkingAnimation = false;


        enemyController.ResetStats();
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.walkerType;
        enemyController.maxEnemyHealth = EnemyStats.maxWalkerHealth;
        enemyController.maxEnemySpeed = EnemyStats.maxWalkerSpeed;
        enemyController.enemyDestroyPrice = EnemyStats.walkerDestroyPrice;
        enemyController.takingDamageResetTime = EnemyStats.walkerTakingDamageResetTime;
        attackTime = EnemyStats.walkerAttackTime;
        attackPreparationTime = EnemyStats.walkerAttackPreparationTime;
        attackCooldownTime = EnemyStats.walkerAttackCooldownTime;
    }

    private void CheckStats()
    {
        isWalkerDestroyed = enemyController.isEnemyDestroyed;
        if (isWalkerDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isPlayingDeathAnimation)
            {
                walkerAnimator.Play("WalkerDeath");
                isPlayingDeathAnimation = true;
            }
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }

            bool canWalkerBeDeactivated = !walkerAudioSource.isPlaying && !walkerParticleSystem.isPlaying;
            if (canWalkerBeDeactivated)
            {
                SetupDefaults();

            }
        }
        else
        {
            isTakingDamage = enemyController.isEnemyTakingDamage;
            walkerSpeed = enemyController.currentEnemySpeed;
            canAttack = enemyController.canAttack;
            target = enemyController.target;
            walkerMovementPosition = PlayerController.playerPosition;
            
            if (!isAttacking && !isPreparing && !attackCooldown)
            {

                LookAtPlayer();
            }
            StartAttack();
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (!isDamaging && canAttack && isAttacking)
        {
            enemyController.canPlayAttackSound = true;
            target.GetComponent<PlayerController>().playerHealth -= EnemyStats.walkerDamage;
            isDamaging = true;
        }
    }

    private void LookAtPlayer()
    {
        if (!isTakingDamage)
        {
            if (walkerMovementPosition.x - transform.position.x >= 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (walkerMovementPosition - transform.position)));
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (walkerMovementPosition - transform.position)));
            }
        }
    }

    private void StartAttack()
    {
        if (walkerAttackZoneController.canWalkerStartAttack && !isAttacking && !isPreparing && !attackCooldown)
        {
            walkerAttackVector = (walkerMovementPosition - transform.position).normalized;
            StartCoroutine(AttackPreparation());
        }
    }

    private IEnumerator AttackTime()
    {
        isAttacking = true;
        walkerAnimator.Play("WalkerAttack");
        yield return new WaitForSeconds(attackTime);
        rb.velocity = Vector3.zero;
        isAttacking = false;
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackPreparation()
    {
        isPreparing = true;
        yield return new WaitForSeconds(attackPreparationTime);
        isPreparing = false;
        StartCoroutine(AttackTime());
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(attackCooldownTime);
        attackCooldown = false;
        if (isDamaging)
        {
            isDamaging = false;
        }
        walkerAnimator.Play("WalkerIdle"); //fix
    }
}
