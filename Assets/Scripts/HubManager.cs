using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HubManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> levelDoors;
    [SerializeField]
    private List<GameObject> levelObjectives;

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
        for (int i = currentLevel - 1; i >= 0; i--)
        {
            GameObject objective = levelObjectives[i];
            var emission = objective.GetComponentInChildren<ParticleSystem>().emission;
            emission.enabled = false;
            var renderers = objective.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.SetFloat("_ShaderLerp", 0);
            }
        }
    }
}
