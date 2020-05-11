﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public Dictionary<GameObject, int> npcsPhases;
    public Koke Koke;
    public Barbara Barbara;
    public bool isTaskPending = true;

    private Player player;
    private GameObject loadScreen;

    private void Start()
    {
        //Añadimos al inicio del juego la fase en la que se encuentra cada npc, para mostrar las conversaciones pertinentes
        player = GameObject.FindObjectOfType<Player>();
        Koke = GameObject.FindObjectOfType<Koke>();
        Barbara = GameObject.FindObjectOfType<Barbara>();
        loadScreen = player.transform.Find("LoadScreen").gameObject;
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
        npc.conversationPhase += 1;
        yield return new WaitForSeconds(1f);
        npc.destroyAndScalate();
        yield return new WaitForSeconds(1f);
        npc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        npc.conversationPhase += 1;
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
        Barbara barbnpc = Barbara.GetComponent<Barbara>();
        StartCoroutine(waitForTaskEndFirstActHouse(npc,barbnpc));

    }

    private IEnumerator waitForTaskEndFirstActHouse(Koke npc, Barbara barbnpc)
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
        npc.conversationPhase += 1;
        npc.extinted = false;
        //Movemos a Koke y Barbara a sus posiciones
        barbnpc.MoveToTable();
        npc.MoveToBed();
        yield return new WaitForSeconds(3f);
        player.continueMoving();
        //Termina la conversacion conjunta y ahora para desactivar las puertas de las granjas Olivia tiene que hablar con el Barbara
        yield return null;
    }


    public void LoadHouseLevel()
    {
        StartCoroutine(transitionToNewLevel("FirstStageHouse"));
    }
    public void LoadSlimesLevel()
    {
        StartCoroutine(transitionToNewLevel("FirstStage"));
    }

    private IEnumerator transitionToNewLevel(string lvlName)
    {
        player.stopFromMoving();
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        SceneManager.LoadSceneAsync(lvlName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += findActualNPCs;
        yield return new WaitForSeconds(2f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();
        yield return null;
    }

    private void findActualNPCs(Scene scene, LoadSceneMode mode)
    {
        Koke = GameObject.FindObjectOfType<Koke>();
        Barbara = GameObject.FindObjectOfType<Barbara>();
        player.transform.position = GameObject.FindGameObjectWithTag("spawnLocation").gameObject.transform.position;
    }

    




}
