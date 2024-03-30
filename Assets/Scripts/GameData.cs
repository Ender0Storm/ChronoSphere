using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData gameDataInstance;

    private int currentLevel;
    private int currentCheckpoint;

    // Start is called before the first frame update
    void Awake() {
        if (gameDataInstance != null) {
            Destroy(gameObject);
        } else {
            gameDataInstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        currentLevel = 0;
        currentCheckpoint = 0;
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public int GetCurrentCheckpoint() {
        return currentCheckpoint;
    }

    public void SetCurrentLevel(int level) {
        currentLevel = level;
    }

    public void SetCurrentCheckpoint(int checkpoint) {
        currentCheckpoint = checkpoint;
    }
}
