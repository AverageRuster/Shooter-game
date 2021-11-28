using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private AudioSource backgroundAudioSource;

    private void Start()
    {
        backgroundAudioSource = GetComponent<AudioSource>();
        
    }
    private void Update()
    {
        if (!backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Play();
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
