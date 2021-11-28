using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrailController : MonoBehaviour
{
    private const float deactivasionTime = 0.15f;
    private Coroutine deactivasionCooldownCoroutine = null;

    public void StartDeactivasion()
    {
        if (deactivasionCooldownCoroutine != null)
        {
            StopCoroutine(deactivasionCooldownCoroutine);
        }
        deactivasionCooldownCoroutine = StartCoroutine(TrailDeactivasionCooldown());
    }

    private IEnumerator TrailDeactivasionCooldown()
    {
        yield return new WaitForSeconds(deactivasionTime);
        deactivasionCooldownCoroutine = null;
        gameObject.SetActive(false);       
    }
}
