using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Arista arista;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<PrimManager>().tryAristaActivation(arista);
    }
}
