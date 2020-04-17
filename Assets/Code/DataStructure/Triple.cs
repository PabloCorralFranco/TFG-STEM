using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple
{
    private string powerUpName;
    private float time, cuantity;

    public Triple(string first, float time, float cuantity)
    {
        this.powerUpName = first;
        this.time = time;
        this.cuantity = cuantity;
    }

    public string _1()
    {
        return powerUpName;
    }
    public float _2()
    {
        return time;
    }
    public float _3()
    {
        return cuantity;
    }


}
