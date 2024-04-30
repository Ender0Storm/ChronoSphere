using System.Collections;
using UnityEngine;

public class Level1MusicManager : MonoBehaviour
{
    public AudioSource musicLayer1;
    public AudioSource musicLayer2;
    public AudioSource musicLayer3;
    //public AudioSource musicLayer4;
    private float fadeDuration = 5.0f;
    private float targetVolume = 0.15f;

    private void Start()
    {
        // Initialise le volume de chaque AudioSource à 0
        musicLayer1.volume = 0;
        musicLayer2.volume = 0;
        musicLayer3.volume = 0;
        //musicLayer4.volume = 0;
    }

    public void PlayMusicLayer1()
    {
        musicLayer1.Play();
        musicLayer2.Play();
        musicLayer3.Play();
        //musicLayer4.Play();
        StartCoroutine(FadeAudioSource(musicLayer1, fadeDuration, targetVolume));
    }

    public void PlayMusicLayer2()
    {
        StartCoroutine(FadeAudioSource(musicLayer2, fadeDuration, targetVolume));
    }

    public void PlayMusicLayer3()
    {
        StartCoroutine(FadeAudioSource(musicLayer3, fadeDuration, targetVolume));
    }
/*
    public void PlayMusicLayer4()
    {
        StartCoroutine(FadeAudioSource(musicLayer4, fadeDuration, targetVolume));
    }
*/
    public void StopAllMusicLayers()
    {
        musicLayer1.loop = false;
        musicLayer2.loop = false;
        musicLayer3.loop = false;  
        StartCoroutine(FadeStopAudioSource(musicLayer1, fadeDuration, 0));
        StartCoroutine(FadeStopAudioSource(musicLayer2, fadeDuration, 0));
        StartCoroutine(FadeStopAudioSource(musicLayer3, fadeDuration, 0));
    }
    IEnumerator FadeStopAudioSource(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float rate = (startVolume - targetVolume) / duration;

        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= rate * Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();
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
}