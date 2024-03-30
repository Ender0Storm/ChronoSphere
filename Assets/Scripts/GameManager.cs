using System;
using System.Collections;
using System.Collections.Generic;
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
        if (playerHealthInstance.playerIsDead)
        {
            GameOver();
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

        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
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


        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
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
        playerBody.SetActive(true);
        StartCoroutine(PlayerFalshing());
    }

    private IEnumerator PlayerFalshing()
    {
        playerBody.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        playerBody.SetActive(true);
        yield return new WaitForSeconds(0.25f);
    }
}
