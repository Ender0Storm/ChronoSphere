using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] private bool isOpen;
    [SerializeField] private Vector3 _doorPosClose;
    [SerializeField] private Vector3 _doorPosOpen;
    [SerializeField] private bool _isMoving;

    public DoorOpening instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _doorPosClose = transform.position;
        _doorPosOpen = transform.position + Vector3.down * 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            if (transform.position.y > _doorPosOpen.y)
            {
                transform.position += Vector3.down * 0.05f;
            }
        }
        
        else
        {
            if (transform.position.y < _doorPosClose.y)
            {
                transform.position += Vector3.up * 0.05f;
            }
        }
        
        if (transform.position.y >= _doorPosClose.y || transform.position.y <= _doorPosOpen.y)
        {
            _isMoving = false;
        }
    }
    
    public bool  IsdoorOpen()
    {
        return isOpen;
    }
    
    public void OpenDoor()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            isOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (!_isMoving)
        {
            _isMoving = true;
            isOpen = false;
        }
    }
}
