using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Load the next scene
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
