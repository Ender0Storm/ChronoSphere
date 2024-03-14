using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Détruit le projectile quand il rentre en contact avec un objet
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
