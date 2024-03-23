using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collapsing : MonoBehaviour
{
    private Rigidbody rb;
    private Renderer rend;
    private BoxCollider boxCollider;
    private bool isCollapsing = false;
    private Vector3 _startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        boxCollider = GetComponent<BoxCollider>();
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!isCollapsing)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isCollapsing = true;
                StartCoroutine(Collapse());
            }
        }
    }

    IEnumerator Flashing()
    {
        rend.enabled = false;
        yield return new WaitForSeconds(0.25f);
        rend.enabled = true;
        yield return new WaitForSeconds(0.25f);
    }
    
    private void GavityEnabled()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        boxCollider.enabled = false;
    }

    private void GravityDisabled()
    {
        rb.useGravity = false;
        rb.isKinematic = true;

    }
    
    private void ActivateCollider()
    {
        rend.enabled = true;
        boxCollider.enabled = true;
    }
    
    private void DisactivateCollider()
    {
        rend.enabled = false;

    }
    
    IEnumerator Collapse()
    {
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(Flashing());
        yield return StartCoroutine(Flashing());
        yield return StartCoroutine(Flashing());
        GavityEnabled();
        yield return new WaitForSeconds(2);
        GravityDisabled();
        DisactivateCollider();
        yield return new WaitForSeconds(3);
        transform.position = _startPos;
        ActivateCollider();
        isCollapsing = false;
    }
}
