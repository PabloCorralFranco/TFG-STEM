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
        StartCoroutine(waitForTaskEnd(npc));

    }

    public IEnumerator waitForTaskEnd(Koke npc)
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
        yield return new WaitForSeconds(5f);
        player.continueMoving();
        isTaskPending = true;
    }




}
