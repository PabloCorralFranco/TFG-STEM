using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGeneration : MonoBehaviour
{
    public Enemy redEnemy, blueEnemy;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void spawnNewGeneration(int green, int red, int blue, int puzzlePhase)
    {
        int i = 0;
        
        
        for(i = 0; i < red; i++)
        {
            instantiation(-2f, -3.5f,redEnemy,puzzlePhase,true);
            instantiation(2f, 3.5f,redEnemy,puzzlePhase,false);
        }
        for (i = 0; i < blue; i++)
        {
            instantiation(-2f, -3.5f,blueEnemy,puzzlePhase,true);
            instantiation(2f, 3.5f,blueEnemy,puzzlePhase,false);
        }
    }
    public void instantiation(float min, float max, Enemy toInstance, int puzzlePhase, bool canChange)
    {
        Vector3 position;
        float xOffset, yOffset;
        xOffset = Random.Range(min, max);
        yOffset = Random.Range(-.5f, 1f);
        position = transform.position + new Vector3(xOffset, yOffset, 0);
        Enemy myEnemyInstance = Instantiate(toInstance, position, Quaternion.identity);
        if(puzzlePhase == 1 && canChange)
        {
            //Debug.Log(myEnemyInstance.GetComponent<Drop>().color);
            myEnemyInstance.copyEssence = Instantiate(myEnemyInstance.essence);
            myEnemyInstance.copyEssence.GetComponent<Drop>().color = "r";
        }
    }
}
