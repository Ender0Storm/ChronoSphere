using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteraction : MonoBehaviour
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
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && (!doorOpeningInstance.IsdoorOpen() || !doorClosingInstance.IsdoorOpen()) &&
            other.transform.position.z < doorGroup.transform.position.z)
        {
            doorOpeningInstance.OpenDoor();
            doorClosingInstance.OpenDoor();
        }

        if (other.gameObject.CompareTag("Player") && (doorOpeningInstance.IsdoorOpen() || doorClosingInstance.IsdoorOpen()) &&
            other.transform.position.z > doorGroup.transform.position.z)
        {
            doorOpeningInstance.CloseDoor();
            doorClosingInstance.CloseDoor();
        }
    }
    // {
        // if (other.gameObject.CompareTag("Player") && doorOpeningInstance.IsdoorOpen() && avatar.transform.position.z < door.transform.position.z)
        // {
        //     doorOpeningInstance.CloseDoor();
        // }
        //
        // if (other.gameObject.CompareTag("Player") && doorOpeningInstance.IsdoorOpen() &&
        //     avatar.transform.position.z > door.transform.position.z)
        // {
        //     doorOpeningInstance.OpenDoor();
        // }
    // }
}
