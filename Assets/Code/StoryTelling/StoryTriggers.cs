﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public string phase;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (phase)
        {
            case "Phase 1":
                Debug.Log("Story trigger succesful");
                GameObject.FindObjectOfType<EventManager>().kokeFirstAct();
                Destroy(this.gameObject);
                break;
            case "Wall Before Koke":
                GameObject.FindObjectOfType<EventManager>().Wall("FirstWall");
                Destroy(this.gameObject);
                break;
            case "Dont Come Back":
                GameObject.FindObjectOfType<EventManager>().Wall("DontComeBack");
                //Destroy(this.gameObject);
                break;
            case "Phase 1 House":
                GameObject.FindObjectOfType<EventManager>().kokeFirstActHouse();
                Destroy(this.gameObject);
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}