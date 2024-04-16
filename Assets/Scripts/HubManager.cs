using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> levelDoors;

    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetGameData", Time.fixedDeltaTime);
    }

    void GetGameData()
    {
        gameData = GameData.gameDataInstance;
        int currentLevel = gameData.GetCurrentLevel();
        DoorOpening door = levelDoors[currentLevel].GetComponent<DoorOpening>();
        door.OpenDoor();
        for (int i = currentLevel - 1; i >= 0; i--)
        {
            DoorOpening doorI = levelDoors[i].GetComponent<DoorOpening>();
            doorI.OpenWithoutAnimation();
        }
    }
}
