using System;
using UnityEngine;
using UnityEngine.Events;

public class LightEmitter : MonoBehaviour
{
    public UnityEvent insideLight;
    public UnityEvent leftLight;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            insideLight.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            leftLight.Invoke();
        }
    }

    public void PlaySound()
    {
        Debug.Log("play sound");
        if (!_audioSource.isPlaying) _audioSource.Play();
    }

    public void StopSound()
    {
        Debug.Log("stop sound");
        _audioSource.Stop();
    }

}
