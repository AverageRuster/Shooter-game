using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private Coroutine deactivasionCoroutine = null;
    public bool deactivasionStarted = false;

    public void StartDeactivasion(float deactivasionTime)
    {
        if (!deactivasionStarted)
        {
            if (deactivasionCoroutine != null)
            {
                StopCoroutine(deactivasionCoroutine);
            }
            deactivasionCoroutine = StartCoroutine(DeactivasionTimer(deactivasionTime));
            deactivasionStarted = true;
        }
    }

    IEnumerator DeactivasionTimer(float deactivasionTime)
    {
        yield return new WaitForSeconds(deactivasionTime);
        deactivasionStarted = false;
        deactivasionCoroutine = null;
        gameObject.SetActive(false);
    }
}
