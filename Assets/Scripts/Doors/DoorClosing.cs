using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosing : MonoBehaviour
{
    private bool isOpen;
    private Vector3 _doorPosClose;
    private Vector3 _doorPosOpen;
    private bool _isMoving;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = true;
        _isMoving = false;
        _doorPosOpen = transform.position;
        _doorPosClose = transform.position + transform.localScale.y * Vector3.up;
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
