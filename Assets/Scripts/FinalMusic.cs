using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMusic : MonoBehaviour
{
    [Header("Music Layer")] 
    public AudioSource musicLayer1;
    public AudioSource musicLayer2;
    public AudioSource musicLayer3;
    private float fadeDuration = 2;
    private bool alreadyTriggered = false;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && alreadyTriggered == false)
        {
            alreadyTriggered = true;
            musicLayer3.Play();
            StartCoroutine(FadeOutAudioSource(musicLayer1, fadeDuration));
            StartCoroutine(FadeOutAudioSource(musicLayer2, fadeDuration));
            StartCoroutine(FadeAudioSource(musicLayer3, fadeDuration, 0.2f));
        }
    }
    IEnumerator FadeAudioSource(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float rate = (targetVolume - startVolume) / duration;

        while (audioSource.volume != targetVolume)
        {
            audioSource.volume += rate * Time.deltaTime;
            audioSource.volume = Mathf.Clamp(audioSource.volume, 0, targetVolume);
            yield return null;
        }
    }
    IEnumerator FadeOutAudioSource(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float rate = startVolume / duration;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= rate * Time.deltaTime;
            audioSource.volume = Mathf.Clamp(audioSource.volume, 0, startVolume);
            yield return null;
        }
    }
}
