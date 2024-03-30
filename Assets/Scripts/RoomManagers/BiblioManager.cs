using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiblioManager : MonoBehaviour
{
    public List<EnemyHealth> enemyHealthList;
    

    public GameObject doorTrigger;
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        for (int i = enemyHealthList.Count-1; i >= 0; i--)
        {
            if (enemyHealthList[i].isDead == true)
            {
                enemyHealthList.Remove(enemyHealthList[i]);
            }
        }
        
        if (enemyHealthList.Count == 0)
        {
            doorTrigger.SetActive(true);
        }
    }
    
}
