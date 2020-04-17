using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Clase que represente el inventario de la jugadora
    //Los circuitos serán los drops enemigos y cada color tiene una función.
    public int blueEsence = 100;
    public int redEsence = 100;
    public int greenEsence = 100;

    public void addEssences(int blue, int red, int green)
    {
        blueEsence += blue;
        redEsence += red;
        greenEsence += green;
    }
}
