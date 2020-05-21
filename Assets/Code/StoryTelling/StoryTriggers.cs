using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryTriggers : MonoBehaviour
{
    public string phase;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Equals("Player"))
        {
            return;
        }
        switch (phase)
        {
            case "Olivia House":
                GameObject.FindObjectOfType<EventManager>().oliviaHouse();
                Destroy(this.gameObject);
                break;
            case "Go to CuteTown":
                GameObject.FindObjectOfType<EventManager>().LoadCuteTownLevel();
                break;
            case "Alcalde":
                GameObject.FindObjectOfType<EventManager>().alcaldeCuteTown();
                Destroy(this.gameObject);
                break;
            case "Load First Stage":
                GameObject.FindObjectOfType<EventManager>().loadFirstStage();
                break;
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
            case "Load House":
                GameObject.FindObjectOfType<EventManager>().LoadHouseLevel();
                break;
            case "Load Slimes":
                GameObject.FindObjectOfType<EventManager>().LoadSlimesLevel();
                break;
            case "Portal to Maria":
                GameObject.FindObjectOfType<EventManager>().transportToMaria();
                break;
            case "Koke in the Woods":
                GameObject.FindObjectOfType<EventManager>().kokeInTheWoods();
                Destroy(this.gameObject);
                break;
            case "Load Bosque":
                GameObject.FindObjectOfType<EventManager>().LoadBosque();
                break;
            case "Botas":
                GameObject.FindObjectOfType<EventManager>().botasConversation();
                break;
            case "Fire House":
                GameObject.FindObjectOfType<EventManager>().houseOnFire();
                Destroy(this.gameObject);
                break;
            case "Revelation":
                GameObject.FindObjectOfType<EventManager>().revelationArc();
                break;
            case "Final Boss":
                GameObject.FindObjectOfType<EventManager>().finalBossAct();
                Destroy(this.gameObject);
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Destroy(this.gameObject);
    }
}
