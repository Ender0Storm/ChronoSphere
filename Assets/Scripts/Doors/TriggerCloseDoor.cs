using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCloseDoor : MonoBehaviour
{
    [SerializeField] public GameObject door;
    [SerializeField] public GameObject doorGroup;
    [SerializeField] public GameManager gameManager;
    private DoorOpening doorOpeningInstance;
    private DoorClosing doorClosingInstance;
    

    private void Start()
    {
        doorOpeningInstance = door.GetComponent<DoorOpening>();
        doorClosingInstance = door.GetComponent<DoorClosing>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        // if (other.gameObject.CompareTag("Player") && (!doorOpeningInstance.IsdoorOpen() || !doorClosingInstance.IsdoorOpen()) &&
        //     other.transform.position.z < doorGroup.transform.position.z)
        // {
        //     doorOpeningInstance.OpenDoor();
        //     doorClosingInstance.OpenDoor();
        // }
        
        //Close door when collision with player ends
        if (other.gameObject.CompareTag("Player") && (doorOpeningInstance.IsdoorOpen() || doorClosingInstance.IsdoorOpen()))
        {
            doorOpeningInstance.CloseDoor();
            doorClosingInstance.CloseDoor();
        }
        
        gameManager.SetRespawnPoint();
    }
}
