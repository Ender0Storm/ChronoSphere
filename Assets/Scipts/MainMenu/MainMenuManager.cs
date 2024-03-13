using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HubScene");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
