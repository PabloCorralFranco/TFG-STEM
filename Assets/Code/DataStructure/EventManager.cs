using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public Dictionary<GameObject, int> npcsPhases;
    public GameObject Koke;
    public bool isTaskPending = true;

    private Player player;

    private void Start()
    {
        //Añadimos al inicio del juego la fase en la que se encuentra cada npc, para mostrar las conversaciones pertinentes
        //npcsPhases.Add(Koke, 0);
        player = GameObject.FindObjectOfType<Player>();
    }

    public void kokeFirstAct()
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        Koke npc = Koke.GetComponent<Koke>();
        StartCoroutine(waitForTaskEndFirstAct(npc));

    }

    public IEnumerator waitForTaskEndFirstAct(Koke npc)
    {
        //Triggereamos la primera conversación de koke.
        npc.popUpMeeting();
        //Esperamos a que acabe la conversacion
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Cuando acabe le cambiamos la fase de conversacion
        npc.changeNpcPhase(npc.gamePhase + 1);
        yield return new WaitForSeconds(1f);
        npc.destroyAndScalate();
        yield return new WaitForSeconds(1f);
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        npc.changeNpcPhase(npc.gamePhase + 1);
        //Lo movemos fuera del mapa
        npc.MoveToPosition();
        yield return new WaitForSeconds(6f);
        npc.gameObject.SetActive(false);
        Destroy(GameObject.FindGameObjectWithTag("FirstWall"));
        player.continueMoving();
        isTaskPending = true;
    }

    public void Wall(string tag)
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        NPC npc = GameObject.FindGameObjectWithTag(tag).GetComponent<NPC>();
        Debug.Log(npc.name);
        StartCoroutine(cantGoThroughWall(npc));
    }

    private IEnumerator cantGoThroughWall(NPC npc)
    {
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isTaskPending = false;
        player.continueMoving();
        yield return null;
    }

    public void kokeFirstActHouse()
    {
        //Congelamos el movimiento del jugador
        player.stopFromMoving();
        Koke npc = Koke.GetComponent<Koke>();
        StartCoroutine(waitForTaskEndFirstActHouse(npc));

    }

    private IEnumerator waitForTaskEndFirstActHouse(Koke npc)
    {
        //Triggereamos la primera conversación de koke.
        npc.popUpMeeting();
        //Esperamos a que acabe la conversacion
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Cuando acabe le cambiamos la fase de conversacion
        npc.changeNpcPhase(npc.gamePhase + 1);
        //Movemos a Koke y Barbara a sus posiciones
        player.continueMoving();
        //Termina la conversacion conjunta y ahora para desactivar las puertas de las granjas Olivia tiene que hablar con el Barbara
        yield return null;
    }




}
