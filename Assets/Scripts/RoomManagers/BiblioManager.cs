using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiblioManager : MonoBehaviour
{
    public List<EnemyHealth> enemyHealthList;
    public List<EnemyAI> enemyAiList;
    public GameObject doorTrigger;
    [Header("Music Layer")] public AudioSource musicLayer1;
    public AudioSource musicLayer2;
    public BoxCollider gameZone;

    private float initialVolume1;
    private float initialVolume2;
    private float fadeDuration = 20;
    private bool musicLayer2Playing = false;
    private int deadEnemiesCount;
    

    private void Start()
    {
        initialVolume1 = musicLayer1.volume;
        initialVolume2 = musicLayer2.volume;
        musicLayer1.volume = 0;
        musicLayer2.volume = 0;
        musicLayer1.Play();
        musicLayer2.Play();
    }

    void FixedUpdate()
    {
        
        for (int i = enemyHealthList.Count - 1; i >= 0; i--)
        {
            if (enemyHealthList[i].isDead == true)
            {
                enemyHealthList.Remove(enemyHealthList[i]);
                enemyAiList.Remove(enemyAiList[i]);
                deadEnemiesCount++;

            }

            if (enemyHealthList.Count == 0)
            {
                musicLayer1.loop = false;
                musicLayer2.loop = false;
                doorTrigger.SetActive(true);
            }
        }

        musicLayer2Playing = false;

        for (int i = enemyAiList.Count - 1; i >= 0; i--)
        {
            if (enemyAiList[i].currentTarget != null)
            {
                musicLayer2Playing = true;
            }
        }

        if (musicLayer2Playing == true && deadEnemiesCount >= 2)
        {
            StartCoroutine(FadeAudioSource(musicLayer2, 50, initialVolume2));
        }
        else
        {
            StartCoroutine(FadeOutAudioSource(musicLayer2, 50));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeAudioSource(musicLayer1, fadeDuration, initialVolume1));
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

