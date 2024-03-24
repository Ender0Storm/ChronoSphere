using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    
    public Texture2D cursorTexture;
    public Vector2 adjustAimPosition = Vector2.zero;
    private bool isBall= false;
    public GameObject pauseMenuUI;
    //Curseur menu pause
    private Texture2D previousCursorTexture;
    private Vector2 previousCursorHotspot;
    private CursorMode previousCursorMode;
    
    [HideInInspector]
    public bool gameIsPaused = false;
    public bool playerIsDead = false;
    
    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
    }
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
    }


    void Update()
    {
        if (playerIsDead)
        {
            Pause();
        }
        isBall = playerMovementScript.isBallMode;
        //Permet de changer de mode
        if (Input.GetKeyDown(KeyCode.F) && !gameIsPaused)
        {
            playerMovementScript.ChangeMode();

            
            if (isBall)
            {
                Cursor.visible = true;
                Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
            }
            else
            {
                Cursor.visible = false;
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    
    void Pause ()
    {
        //Retiens l'état du curseur en jeu
        previousCursorTexture = Cursor.visible ? cursorTexture : null;
        previousCursorHotspot = Cursor.visible ? adjustAimPosition : Vector2.zero;
        previousCursorMode = Cursor.visible ? CursorMode.Auto : CursorMode.ForceSoftware;

        //Met le curseur windows par defaut
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
    
    public void Resume ()
    {
        
        if (isBall)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.SetCursor(previousCursorTexture, previousCursorHotspot, previousCursorMode);
            Cursor.visible = previousCursorTexture != null;
            
        }


        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        if (playerIsDead == false)
        {
            playerIsDead = true;
        }

    }
}
