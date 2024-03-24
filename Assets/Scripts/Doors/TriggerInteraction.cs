using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject doorGroup;
    [SerializeField] public GameObject avatar;
    private DoorOpening doorOpeningInstance;

    private void Start()
    {
        doorOpeningInstance = door.GetComponent<DoorOpening>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !doorOpeningInstance.IsdoorOpen() && avatar.transform.position.z < doorGroup.transform.position.z)
        {
            doorOpeningInstance.OpenDoor();
        }

        if (other.gameObject.CompareTag("Player") && doorOpeningInstance.IsdoorOpen() &&
            avatar.transform.position.z > doorGroup.transform.position.z)
        {
            doorOpeningInstance.CloseDoor();
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
