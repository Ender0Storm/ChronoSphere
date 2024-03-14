using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathOnHitByEnemyProjectile : MonoBehaviour
{


    void OnCollisionEnter(Collision collision)
    {
        //Vérifie si l'objet entrant en collision est un projectile
        if (collision.gameObject.GetComponent<ProjectileEnemy>() != null)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("Prototype"); 
        }
    }
}
    

