using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterController : MonoBehaviour
{
    [SerializeField] private GameObject attackZone;
    [SerializeField] private GameObject laser;
    [SerializeField] private CutterLaserController cutterLaserController = null;
    //controllers
    private EnemyController enemyController = null;
    private Rigidbody2D rb;
    private AudioSource cutterAudioSource = null;
    private ParticleSystem cutterParticleSystem = null;

    private Animator cutterAnimator = null;

    //statements
    private bool canCutterRotate = true;
    private bool isCutterDestroyed = false;
    private float enemySpeed = 0;
    private bool isTargetTouchesLaser = false;
    private bool canLaserDamage = true;
    private bool isTakingDamage = false;
    private bool isPlayingWalkingAnimation = false;
    private bool isPlayingDeathAnimation = false;
    private bool isAttacking = false;
    private bool canAttack = false;
    
    private Vector3 enemyMovementPosition = Vector3.zero;
    private GameObject target;
    private bool allCoroutinesStopped = false;

    [SerializeField] private GameObject cutterBody = null;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        cutterAudioSource = GetComponent<AudioSource>();
        cutterParticleSystem = GetComponent<ParticleSystem>();
        cutterAnimator = GetComponent<Animator>();
        SetupStatsController();
        SetupDefaults();    
    }

    private void SetupDefaults()
    {
        isCutterDestroyed = false;
        enemySpeed = 0;
        canCutterRotate = true;
        isAttacking = false;
        canAttack = false;
        isTargetTouchesLaser = false;
        enemyMovementPosition = Vector3.zero;
        target = null;
        allCoroutinesStopped = false;
        canLaserDamage = true;
        isTakingDamage = false;
        isPlayingWalkingAnimation = false;
        isPlayingDeathAnimation = false;
        if (laser.activeSelf)
        {
            laser.SetActive(false);
        }
        enemyController.ResetStats();
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.cutterType;
        enemyController.maxEnemyHealth = EnemyStats.maxCutterHealth;
        enemyController.maxEnemySpeed = EnemyStats.maxCutterSpeed;
        enemyController.enemyDestroyPrice = EnemyStats.cutterDestroyPrice;
        enemyController.takingDamageResetTime = EnemyStats.cutterTakingDamageResetTime;
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
    }

    private void FixedUpdate()
    {
        if (!isCutterDestroyed)
        {
            if (enemyController.isEnemyActive)
            {
                if (!GameManager.gameOver && !canAttack && !isAttacking && !isTakingDamage)
                {

                    rb.MovePosition(transform.position + (enemyMovementPosition - transform.position).normalized * enemySpeed * Time.fixedDeltaTime);
                    if (!isPlayingWalkingAnimation)
                    {
                        cutterAnimator.Play("CutterWalk");
                        isPlayingWalkingAnimation = true;
                    }

                }
                else
                {
                    if (isPlayingWalkingAnimation)
                    {
                        if (!isAttacking)
                        {
                            cutterAnimator.Play("CutterIdle");
                        }
                        isPlayingWalkingAnimation = false;
                    }
                }
            }
        }
    }

    

    private void CheckStats()
    {
        isCutterDestroyed = enemyController.isEnemyDestroyed;
        if (isCutterDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isPlayingDeathAnimation)
            {
                cutterAnimator.Play("CutterDeath");
                isPlayingDeathAnimation = true;
            }
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
            laser.SetActive(false);
            bool canCutterBeDeactivated = !cutterAudioSource.isPlaying && !cutterParticleSystem.isPlaying;
            if (canCutterBeDeactivated)
            {
                SetupDefaults();
            }
        }
        else
        {
            isTakingDamage = enemyController.isEnemyTakingDamage;
            enemySpeed = enemyController.currentEnemySpeed;
            canAttack = enemyController.canAttack;
            target = cutterLaserController.target;
            isTargetTouchesLaser = cutterLaserController.isTargetTouchesLaser;
            enemyMovementPosition = PlayerController.playerPosition;
            if (canCutterRotate && !isCutterDestroyed)
            {
                LookAtPlayer();
            }
            Attack();
            DamagePlayer();
        }
    }


    private void Attack()
    {
        if (!isAttacking && canAttack)
        {
            cutterAnimator.Play("CutterAttack");
            StartCoroutine(LaserActivasionCooldown());            
        }
    }

    private IEnumerator LaserActivasionCooldown()
    {        
        isAttacking = true;
        yield return new WaitForSeconds(EnemyStats.cutterRotationBlockCooldown);
        canCutterRotate = false;
        yield return new WaitForSeconds(EnemyStats.cutterLaserActivasionCooldown);     
        laser.SetActive(true);
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(EnemyStats.cutterAttackCooldown);
        laser.SetActive(false);
        canCutterRotate = true;
        isAttacking = false;
    }

    public void DamagePlayer()
    {
        if (canLaserDamage && isTargetTouchesLaser && target != null && !target.GetComponent<PlayerController>().isPlayerImmortal)
        {
            target.GetComponent<PlayerController>().playerHealth -= EnemyStats.cutterDamage;
            StartCoroutine(LaserDamageCooldown());
        }
    }
    private IEnumerator LaserDamageCooldown()
    {
        canLaserDamage = false;
        yield return new WaitForSeconds(EnemyStats.cutterLaserDamageCooldown);
        canLaserDamage = true;
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
        }
    }
}
