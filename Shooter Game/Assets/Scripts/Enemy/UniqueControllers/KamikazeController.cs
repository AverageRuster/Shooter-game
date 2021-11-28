using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeController : MonoBehaviour
{
    //controllers
    private EnemyController enemyController = null;
    [SerializeField] private AttackZoneController attackZoneController;
    private AudioSource kamikazeAudioSource = null;
    private ParticleSystem kamikazeParticleSystem = null;
    private Animator kamikazeAnimator = null;  

    private bool isPlayingWalkAnimation = false;
    private bool isKamikazeDestroyed = false;
    private float kamikazeSpeed = 0;
    private bool allCoroutinesStopped = false;
    private bool isPlayingDeathAnimation = false;

    private bool isAttacking = false;
    private bool canAttack = false;
    private Rigidbody2D rb;
    private Vector3 kamikazeMovementPosition = Vector3.zero;
    private bool canDetonate = false;

    

    [SerializeField] private GameObject kamikazeBody;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        kamikazeAudioSource = GetComponent<AudioSource>();
        kamikazeParticleSystem = GetComponent<ParticleSystem>();
        kamikazeAnimator = GetComponent<Animator>();
        SetupStatsController();
        SetupDefaults();        
    }
    private void SetupDefaults()
    {
        kamikazeSpeed = 0;
        allCoroutinesStopped = false;
        isAttacking = false;
        canAttack = false;
        canDetonate = false;
        kamikazeMovementPosition = Vector3.zero;
        isPlayingWalkAnimation = false;
        isPlayingDeathAnimation = false;
        enemyController.ResetStats();
    }

    private void SetupStatsController()
    {
        enemyController.enemyType = EnemyStats.kamikazeType;
        enemyController.maxEnemyHealth = EnemyStats.maxKamikazeHealth;
        enemyController.maxEnemySpeed = EnemyStats.maxKamikazeSpeed;
        enemyController.enemyDestroyPrice = EnemyStats.kamikazeDestroyPrice;
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
        if (!GameManager.gameOver && !isKamikazeDestroyed)
        {
            if (enemyController.isEnemyActive)
            {
                if (!isAttacking)
                {
                    rb.MovePosition(transform.position + (kamikazeMovementPosition - transform.position).normalized * kamikazeSpeed * Time.fixedDeltaTime);
                    if (!isPlayingWalkAnimation)
                    {
                        kamikazeAnimator.Play("KamikazeWalk");
                        isPlayingWalkAnimation = true;
                    }
                }
            }
        }
    }


    private void CheckStats()
    {       
        isKamikazeDestroyed = enemyController.isEnemyDestroyed;
        if (isKamikazeDestroyed)
        {
            enemyController.enemyBodyPosition = transform.position;
            enemyController.enemyBodyRotation = transform.rotation;
            if (!isAttacking)
            {
                if (!isPlayingDeathAnimation)
                {
                    kamikazeAnimator.Play("KamikazeDeath");
                    isPlayingDeathAnimation = true;
                }
            }
            else
            {
                if (!isPlayingDeathAnimation)
                {
                    kamikazeAnimator.Play("KamikazeAttackDeath");
                    isPlayingDeathAnimation = true;
                }
            }

            if (!canDetonate)
            {
                canDetonate = true;
                CheckDetonation();
            }
                           
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
            bool canKamikazeBeDeactivated = !kamikazeAudioSource.isPlaying && !kamikazeParticleSystem.isPlaying;
            if (canKamikazeBeDeactivated)
            {
                SetupDefaults();

            }          
        }
        else
        {
            if (canAttack && !isAttacking)
            {
                kamikazeAnimator.Play("KamikazeAttack");                
                StartCoroutine(StartDetonation());
                isAttacking = true;
            }
            kamikazeSpeed = enemyController.currentEnemySpeed;
            canAttack = enemyController.canAttack;
            kamikazeMovementPosition = PlayerController.playerPosition;
            if (!isAttacking)
            {
                LookAtPlayer();
            }
        }       
    }


    private void CheckDetonation()
    {        
        if (canDetonate)
        {

            enemyController.canPlayAttackSound = true;
            List<GameObject> playerTargets = attackZoneController.playerTargets;
            List<GameObject> enemyTargets = attackZoneController.enemyTargets;
            for (int i = 0; i < playerTargets.Count; i++)
            {
                if (!playerTargets[i].GetComponent<PlayerController>().isPlayerImmortal)
                {
                    playerTargets[i].GetComponent<PlayerController>().playerHealth -= EnemyStats.kamikazeDamageToPlayer;
                }
            }
            for (int i = 0; i < enemyTargets.Count; i++)
            {
                enemyTargets[i].GetComponent<EnemyController>().enemyHealth -= EnemyStats.kamikazeDamageToEnemy;
            }
            attackZoneController.ResetLists();
        }       
    }


    IEnumerator StartDetonation()
    {
        yield return new WaitForSeconds(EnemyStats.kamikazeDetonationTime);
        enemyController.enemyHealth = 0;
    }

    private void LookAtPlayer()
    {
        if (kamikazeMovementPosition.x - transform.position.x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, (kamikazeMovementPosition - transform.position)));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, (kamikazeMovementPosition - transform.position)));
        }
    }
}
