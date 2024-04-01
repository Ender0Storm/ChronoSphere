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
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (changeData) GameData.gameDataInstance.SetCurrentLevel(newLevel);
            //Load the next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
