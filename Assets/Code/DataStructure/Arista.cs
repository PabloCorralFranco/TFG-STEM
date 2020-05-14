using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Arista
{
    public string vertex1, vertex2;
    public int weight;

    public Arista(string v1, string v2, int w)
    {
        vertex1 = v1;
        vertex2 = v2;
        weight = w;
    }

    public string _1()
    {
        return vertex1;
    }
    public string _2()
    {
        return vertex2;
    }
    public int _3()
    {
        return weight;
    }

    public string toString()
    {
        return "Arista(" + vertex1 + ", " + vertex2 + ", " + weight + ")";
    }



}
