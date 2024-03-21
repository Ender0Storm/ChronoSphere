using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    void Start()
    {

    }


    void Update()
    {
        //Permet de changer de mode
        if (Input.GetKeyDown(KeyCode.F))
        {
            playerMovementScript.ChangeMode();
        }
    }
}
