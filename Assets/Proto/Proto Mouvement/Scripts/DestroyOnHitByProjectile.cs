using System.Collections;
using UnityEngine;

public class DestroyOnHitByProjectile : MonoBehaviour
{
    public AudioSource hitSound;
    public GameObject head; 

    void OnCollisionEnter(Collision collision)
    {
        //Vérifie si l'objet entrant en collision est un projectile
        if (collision.gameObject.GetComponent<Projectile>() != null)
        {
            StartCoroutine(DestroyAfterSound());
            
            StartCoroutine(DelayHeadDetach(0.0f));
        }
    }

    //Permet au hitSound de se produire avant la destruction du scrip en même temps que l'objet
    IEnumerator DestroyAfterSound()
    {
        GetComponent<Renderer>().enabled = false;
        hitSound.Play();
        yield return new WaitForSeconds(hitSound.clip.length);
        Destroy(gameObject);
    }

    
    
    //Ajoute un rigid body à la tête, après un délai, pour
    //éviter qu'elle disparraise avec le corps ou parte dans une direction étrange.
    IEnumerator DelayHeadDetach(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (head != null)
        {
            Rigidbody headRb = head.AddComponent<Rigidbody>(); 
            head.transform.parent = null; 
            headRb.isKinematic = false; 
            headRb.mass = 0.5f; 
            Destroy(head, 1.5f); 
        }
    }
}