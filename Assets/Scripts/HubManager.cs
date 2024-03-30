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
        gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        GameObject door = levelDoors[gameData.GetCurrentLevel()];
        door.transform.Find("doorOpenTrigger").gameObject.SetActive(true);
        door.transform.Find("doorCloseTrigger").gameObject.SetActive(true);
    }
}
