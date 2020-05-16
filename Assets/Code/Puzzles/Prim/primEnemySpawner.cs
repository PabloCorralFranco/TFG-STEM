using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class primEnemySpawner : MonoBehaviour
{
    public bool canSpawn;
    public Enemy redEnemy, blueEnemy;

    private void Start()
    {
        canSpawn = false;
        InvokeRepeating("spawnRedEnemies", 0f, 5f);
        InvokeRepeating("spawnBlueEnemies", 0f, 5f);
    }

    private void spawnRedEnemies()
    {
        if (canSpawn)
        {
            instantiation(-1, 1, redEnemy);
        }
    }
    private void spawnBlueEnemies()
    {
        if (canSpawn)
        {
            instantiation(-1, 1, blueEnemy);
        }
    }

    public void instantiation(float min, float max, Enemy toInstance)
    {
        Vector3 position;
        float xOffset, yOffset;
        xOffset = Random.Range(min, max);
        yOffset = Random.Range(min, max);
        position = transform.position + new Vector3(xOffset, yOffset, 0);
        Enemy myEnemyInstance = Instantiate(toInstance, position, Quaternion.identity);
    }

}
