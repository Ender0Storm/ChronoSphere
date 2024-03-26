using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOpenDoor : MonoBehaviour
{
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject doorGroup;
    private DoorOpening doorOpeningInstance;
    private DoorClosing doorClosingInstance;

    private void Start()
    {
        doorOpeningInstance = door.GetComponent<DoorOpening>();
        doorClosingInstance = door.GetComponent<DoorClosing>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Open door when collision with player happens
        if (other.gameObject.CompareTag("Player") && (!doorOpeningInstance.IsdoorOpen() || !doorClosingInstance.IsdoorOpen()))
        {
            doorOpeningInstance.OpenDoor();
            doorClosingInstance.OpenDoor();
        }
        //Close door when collision with player happens
        // if (other.gameObject.CompareTag("Player") && (doorOpeningInstance.IsdoorOpen() || doorClosingInstance.IsdoorOpen()) &&
        //     other.transform.position.z > doorGroup.transform.position.z)
        // {
        //     doorOpeningInstance.CloseDoor();
        //     doorClosingInstance.CloseDoor();
        // }
    }
}