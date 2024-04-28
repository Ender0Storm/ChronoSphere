using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    [SerializeField]
    private bool changeData;
    [SerializeField]
    private int newLevel;
    [SerializeField]
    private float waitTime;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (changeData) GameData.gameDataInstance.SetCurrentLevel(newLevel);

            Invoke(nameof(LoadNewScene), waitTime);
        }
    }

    //Load the next scene
    private void LoadNewScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
