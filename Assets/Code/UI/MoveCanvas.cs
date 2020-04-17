using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCanvas : MonoBehaviour
{
    public GameObject canvas;
    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        //originalPosition = canvas.transform.position;
    }

    public void Move()
    {
        Debug.Log("Holi");
        canvas.SetActive(!canvas.activeSelf);
    }
    
}
