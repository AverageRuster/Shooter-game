using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    public int soulCount = 0;
    public bool canSoulMoveToPlayer = false;
    private float deactivasionTime = 30;
    private Rigidbody2D rb;
    private float maxSoulSpeed = 5;
    public Coroutine soulDeactivasionCooldown = null;
    public Animator soulAnimator = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        soulAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (canSoulMoveToPlayer)
        {
            Vector3 movementVector = (PlayerController.playerPosition - transform.position).normalized;
            rb.MovePosition(transform.position + movementVector * maxSoulSpeed * Time.fixedDeltaTime);
        }
    }

    public IEnumerator DeactivasionCooldown()
    {
        yield return new WaitForSeconds(deactivasionTime);
        soulAnimator.Play("SoulDestruction");
    }

    public void StopAllActiveCoroutines()
    {
        StopAllCoroutines();
    }

    public void PlayIdleAnimation()
    {
        soulAnimator.Play("SoulIdle");
    }

    public void DeactivateSoul()
    {
        gameObject.SetActive(false);
    }
}
