using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject shootingPrefab;
    private GameObject currentPrefab;
    public FollowPlayer cameraFollowScript;
    public List<EnemyMovement> enemyMovementScripts;
    private bool isBall = false;
    public Texture2D cursorTexture;
    public Vector2 adjustAimPosition = Vector2.zero;
    public static bool GameIsPaused;
    public GameObject pauseMenuUI;
    //Curseur menu pause
    private Texture2D previousCursorTexture;
    private Vector2 previousCursorHotspot;
    private CursorMode previousCursorMode;

    private void Awake()
    {
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
    }

    void Start()
    {
        currentPrefab = Instantiate(shootingPrefab, transform.position, Quaternion.identity);
        cameraFollowScript.playerTransform = currentPrefab.transform;

        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
    }

    void Update()
    {
        //Permet de changer de mode
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SwitchPrefab();
        }
        
        //Menu pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        //Quand le perso tombe en bas du niveau, le niveau restart
        if (currentPrefab.transform.position.y < -4) 
        {
            RestartLevel();
        }
    }

    void SwitchPrefab()
    {
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;

        if (currentPrefab != null)
        {
            currentPosition = currentPrefab.transform.position;
            Destroy(currentPrefab);
        }

        if (isBall)
        {
            currentPrefab = Instantiate(shootingPrefab, currentPosition, Quaternion.Euler(0, 0, 0));
            isBall = false;
            Cursor.visible = true;
            Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
        }
        else
        {
            currentPrefab = Instantiate(ballPrefab, currentPosition, Quaternion.Euler(0, 0, 0));
            isBall = true;
            Cursor.visible = false;
        }

        cameraFollowScript.playerTransform = currentPrefab.transform;
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
        GameIsPaused = true;
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
        GameIsPaused = false;

    }
    
    public void QuitGame ()
    {
        Application.Quit();
    }

    public void RestartLevel ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false; 
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, adjustAimPosition, CursorMode.Auto);
        SceneManager.LoadScene("Prototype"); 
        
    }
}