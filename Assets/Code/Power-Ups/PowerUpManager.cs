using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private bool isActive;
    private Dictionary<string, GameObject> powerups;
    public string[] frases;
    public GameObject[] pinstances;

    //Mirar para controlar la activación pero del mismo tipo, si se pueden activar de distintos tipos
    private void Start()
    {
        isActive = false;
        powerups = new Dictionary<string, GameObject>();
        //Inicialización de diccionario frase,instancia
        for(int i = 0; i < pinstances.Length; i++)
        {
            //GameObject ninstance = Instantiate(pinstances[i]);
            powerups.Add(frases[i], pinstances[i]);
        }
    }

    public GameObject returnPowerUpInstance(string pname)
    {
        GameObject npinstance;
        if (powerups.ContainsKey(pname) && powerups.TryGetValue(pname, out npinstance))
        {
            return npinstance;
        }
        //En caso de no existir devolvemos un powerup aleatorio negativo
        return null;
    }

    public void setActive(bool state)
    {
        isActive = state;
    }
    public bool getActive()
    {
        return isActive;
    }
}
