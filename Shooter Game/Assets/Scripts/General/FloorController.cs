using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    bool allCoroutinesStopped = false;
    private const float animationDelayTime = 60;

    private void Start()
    {
        allCoroutinesStopped = false;
    }

    private void Update()
    {
        if (GameManager.gameOver)
        {
            if (!allCoroutinesStopped)
            {
                StopAllCoroutines();
                allCoroutinesStopped = true;
            }
        }
    }
    public void StartAnimationDelay(string animationName)
    {
        StartCoroutine(AnimationDelay(Random.Range(0, animationDelayTime), animationName));
    }

    IEnumerator AnimationDelay(float animationDelayTime, string animationName)
    {
        yield return new WaitForSeconds(animationDelayTime);
        GetComponent<Animator>().Play(animationName);
    }
}
