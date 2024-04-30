using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicTrigger : MonoBehaviour
{
    public Level1MusicManager level1MusicManager;
    public int functionNumber1To4;
    private bool alreadyTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            switch (functionNumber1To4)
            {
                case 1:
                    level1MusicManager.PlayMusicLayer1();
                    break;
                case 2:
                    level1MusicManager.PlayMusicLayer2();
                    break;
                case 3:
                    level1MusicManager.PlayMusicLayer3();
                    break;
                case 4 :
                    level1MusicManager.StopAllMusicLayers();
                    break;
            }

        }
    }
}