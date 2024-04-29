using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiblioManager : MonoBehaviour
{
    public List<EnemyHealth> enemyHealthList;
    public GameObject doorTrigger;
    [Header("Music Layer")]
    public AudioSource musicLayer1;
    public AudioSource musicLayer2;
    public AudioSource drumRoll;
    public BoxCollider gameZone;

    private float initialVolume1;
    private float initialVolume2;
    private float fadeDuration = 2.0f;

    private void Start()
    {
        initialVolume1 = musicLayer1.volume;
        initialVolume2 = musicLayer2.volume;
        musicLayer1.volume = 0;
        musicLayer2.volume = 0;
    }

    void Update()
    {
        int initialEnemyCount = enemyHealthList.Capacity;

        for (int i = enemyHealthList.Count - 1; i >= 0; i--)
        {
            if (enemyHealthList[i].isDead == true)
            {
                enemyHealthList.Remove(enemyHealthList[i]);

                if (enemyHealthList.Count == initialEnemyCount / 2)
                {
                    musicLayer2.mute = false;
                    drumRoll.Play();
                    StartCoroutine((PlayLayers2AfterDrumRoll()));
                }
            }

            if (enemyHealthList.Count == 0)
            {
                musicLayer1.loop = false;
                musicLayer2.loop = false;
                doorTrigger.SetActive(true);
            }
        }
    }

    IEnumerator PlayLayers2AfterDrumRoll()
    {
        yield return new WaitWhile(() => drumRoll.isPlaying);
        musicLayer1.Play();
        musicLayer2.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeAudioSource(musicLayer1, fadeDuration, initialVolume1));
            StartCoroutine(FadeAudioSource(musicLayer2, fadeDuration, initialVolume2));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeAudioSource(musicLayer1, fadeDuration, 0));
            StartCoroutine(FadeAudioSource(musicLayer2, fadeDuration, 0));
        }
    }

    IEnumerator FadeAudioSource(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;
        float rate = (targetVolume - startVolume) / duration;

        while (audioSource.volume != targetVolume)
        {
            audioSource.volume += rate * Time.deltaTime;
            audioSource.volume = Mathf.Clamp(audioSource.volume, 0, 0.3f);
            yield return null;
        }
    }
}