using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public GameObject playerBody;
    public GameObject playerInSpaceWorld;
    
    public Texture2D cursorTexture;
    public Vector2 adjustAimPosition = Vector2.zero;
    private bool isBall= false;
    public GameObject gameOverMenuUI;
    public GameObject pauseMenuUI;
    public GameObject endDemoMenuUI;
    //Curseur menu pause
    private Texture2D previousCursorTexture;
    private Vector2 previousCursorHotspot;
    private CursorMode previousCursorMode;
    //Pour respawn player
    public Vector3 respawnPoint;
    public PlayerHealth playerHealthInstance;
    public PlayerMovement playerMovementInstance;
    
    
    [HideInInspector]
    public bool gameIsPaused = false;
    
    
    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
    }
    
    void Start()
    {
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
        respawnPoint = playerInSpaceWorld.transform.position;
        
    }


    void Update()
    {
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
        
        if (playerHealthInstance.playerIsDead)
        {
            GameOver();
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
        gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        playerMovementInstance.gameIsPaused = true;
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
        AudioListener.pause = false;
        Time.timeScale = 1f;
        gameIsPaused = false;
        playerMovementInstance.gameIsPaused = false;

    }
    
    public void SetRespawnPoint()
    {
        respawnPoint = playerInSpaceWorld.transform.position;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        playerHealthInstance.playerIsDead = false;
        playerInSpaceWorld.transform.position = respawnPoint;
        playerHealthInstance.Respawn();
        StartCoroutine(PlayerFalshing());
    }

    private IEnumerator PlayerFalshing()
    {
        yield return new WaitForSeconds(0.3f);
        playerHealthInstance.SetRendererEnabled(false);
        yield return new WaitForSeconds(0.1f);
        playerHealthInstance.SetRendererEnabled(true);
        yield return new WaitForSeconds(0.3f);
        playerHealthInstance.SetRendererEnabled(false);
        yield return new WaitForSeconds(0.1f);
        playerHealthInstance.SetRendererEnabled(true);
        yield return new WaitForSeconds(0.3f);
        playerHealthInstance.SetRendererEnabled(false);
        yield return new WaitForSeconds(0.1f);
        playerHealthInstance.SetRendererEnabled(true);
    }

    public void EndDemo()
    {
        //Retiens l'état du curseur en jeu
        previousCursorTexture = Cursor.visible ? cursorTexture : null;
        previousCursorHotspot = Cursor.visible ? adjustAimPosition : Vector2.zero;
        previousCursorMode = Cursor.visible ? CursorMode.Auto : CursorMode.ForceSoftware;

        //Met le curseur windows par defaut
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
        gameIsPaused = true;
        AudioListener.pause = true;
        playerMovementInstance.gameIsPaused = true;
        endDemoMenuUI.SetActive(true);

    }
    
}
