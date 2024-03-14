using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script utilisé pour que la caméra suive le joueur
public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - playerTransform.position;
    }

    private void LateUpdate()
    {
        transform.position = playerTransform.position + offset;
    }
}
