﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public Dictionary<GameObject, int> npcsPhases;
    public Koke Koke;
    public Barbara Barbara;
    public bool isTaskPending = true;

    private Player player;
    private GameObject loadScreen;
    private bool canActivatePuzzle;
    private Vector3 barbHouse, kokeHouse;
    private int[] kokePhases, barbaraPhases;
    private bool barbExtinted, kokeExtinted;
    private int puzzleGen;

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
        //Debug.Log(npc.name);
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
        canActivatePuzzle = true;
        yield return new WaitForSeconds(3f);
        //Cogemos sus posiciones para la carga.
        kokeHouse = npc.transform.position;
        barbHouse = barbnpc.transform.position;
        //Activamos el menu para la wiki
        GameObject.FindGameObjectWithTag("Movement").transform.Find("MenuButton").gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("Movement").transform.Find("Life").gameObject.SetActive(true);
        player.continueMoving();
        player.canAttack = true;
        //Termina la conversacion conjunta y ahora para desactivar las puertas de las granjas Olivia tiene que hablar con el Barbara
        yield return null;
    }

    public void genPuzzleFinished()
    {
        GameObject whereToSpawn = GameObject.FindGameObjectWithTag("spawnLocation");
        whereToSpawn.transform.Find("Koke").gameObject.SetActive(true);
        Koke kokenpc = whereToSpawn.transform.Find("Koke").GetComponent<Koke>();
        whereToSpawn.transform.Find("Barbara").gameObject.SetActive(true);
        Barbara barbnpc = whereToSpawn.transform.Find("Barbara").GetComponent<Barbara>();
        StartCoroutine(genPuzzleEvent(kokenpc,barbnpc));
    }

    private IEnumerator genPuzzleEvent(Koke kokenpc, Barbara barbnpc)
    {
        player.stopFromMoving();
        kokenpc.conversationPhase = 4;
        yield return new WaitForSeconds(2);
        kokenpc.MoveToGenerator();        
        yield return new WaitForSeconds(1.5f);
        barbnpc.MoveToGenerator();
        yield return new WaitForSeconds(1f);
        kokenpc.popUpMeeting();
        isTaskPending = true;
        while (isTaskPending)
        {
            yield return new WaitForSeconds(0.1f);
        }
        kokenpc.conversationPhase += 1;
        kokenpc.extinted = false;
        //Metemos a Barbara en casa. Cerramos la puerta, abrimos la del bosque y mandamos a Koke al Bosque.
        barbnpc.backToHouse();
        kokenpc.MoveToForest();
        yield return new WaitForSeconds(3f);
        barbnpc.gameObject.SetActive(false);
        kokenpc.gameObject.SetActive(false);
        GameObject barreras = GameObject.FindGameObjectWithTag("barrerasHolder");
        for(int i = 0; i < barreras.transform.childCount; i++)
        {
            barreras.transform.GetChild(i).gameObject.SetActive(true);
        }
        GameObject.FindGameObjectWithTag("BarreraBosque").SetActive(false);
        player.continueMoving();
        //Cuando acabemos le damos los modulos de generador y compilador
        GameObject generalMenu = player.transform.Find("GeneralMenu").gameObject;
        Button generator = generalMenu.transform.Find("BotonGenerador").GetComponent<Button>();
        generator.interactable = true;
        generator.GetComponentInChildren<TextMeshProUGUI>().text = "GENERADOR";
        Button compilador = generalMenu.transform.Find("BotonCompilador").GetComponent<Button>();
        compilador.interactable = true;
        compilador.GetComponentInChildren<TextMeshProUGUI>().text = "COMPILADOR";

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

        //Antes de salir cogemos en que estado de conversacion se han quedado
        if (lvlName.Equals("FirstStage"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
            kokePhases = new int[] { Koke.conversationPhase, Koke.startingPhase, Koke.repeatedPhase, Koke.negationPhase };
            kokeExtinted = Koke.extinted;
            barbaraPhases = new int[] { Barbara.conversationPhase, Barbara.startingPhase, Barbara.repeatedPhase, Barbara.negationPhase };
            barbExtinted = Barbara.extinted;
        }
        if (lvlName.Equals("FirstStageHouse"))
        {
            puzzleGen = FindObjectOfType<GeneticManager>().genPhase;
        }
        yield return new WaitForSeconds(1f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        SceneManager.LoadSceneAsync(lvlName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += findActualNPCs;
        yield return new WaitForSeconds(3f);
        loadScreen.SetActive(!loadScreen.activeSelf);
        player.continueMoving();
        yield return null;
    }

    private void findActualNPCs(Scene scene, LoadSceneMode mode)
    {
        //Con esto mantenemos el estado de la escena abierta
        if (canActivatePuzzle && scene.name.Equals("FirstStage"))
        {
            //Reactivamos los colisionadores para los pilares
            Debug.Log("Conseguimos los pilares");
            PilarMatrix[] pilars = FindObjectsOfType<PilarMatrix>();
            for (int i = 0; i < pilars.Length; i++)
            {
                pilars[i].reactivateCollider();
            }
            //Activamos el collider para el generador
            BoxCollider2D genManagerCollider = FindObjectOfType<GeneticManager>().gameObject.GetComponent<BoxCollider2D>();
            genManagerCollider.enabled = true;
            GameObject[] barrerasGranja = GameObject.FindGameObjectsWithTag("barreraGranja");
            for(int i = 0; i < barrerasGranja.Length; i++)
            {
                barrerasGranja[i].SetActive(!barrerasGranja[i].gameObject.activeSelf);
            }
            GameObject[] storyTriggers = GameObject.FindGameObjectsWithTag("DontComeBack");
            for (int i = 0; i < storyTriggers.Length; i++)
            {
                storyTriggers[i].SetActive(!storyTriggers[i].gameObject.activeSelf);
            }
            GameObject[] firstWalls = GameObject.FindGameObjectsWithTag("FirstWall");
            for(int i = 0; i < firstWalls.Length; i++)
            {
                firstWalls[i].SetActive(false);
            }
            //Koke = GameObject.FindObjectOfType<Koke>();
            //Koke.gameObject.SetActive(false);
            FindObjectOfType<GeneticManager>().genPhase = puzzleGen;
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            //Ponemos a koke en su sitio para el evento de finalizacion
            Koke = GameObject.FindObjectOfType<Koke>();
            Koke.gameObject.SetActive(false);
            GameObject whereToSpawn = GameObject.FindGameObjectWithTag("spawnLocation");
            Koke.gameObject.transform.position = whereToSpawn.transform.position;
            Koke.gameObject.transform.SetParent(whereToSpawn.transform);
            //canActivatePuzzle = false;

        }
        if(canActivatePuzzle && scene.name.Equals("FirstStageHouse"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
            Koke.transform.position = kokeHouse;
            Barbara.transform.position = barbHouse;
            Koke.GetComponent<Animator>().SetFloat("Horizontal", -1);
            Koke.GetComponent<Animator>().SetFloat("Vertical", 0);
            GameObject.FindGameObjectWithTag("DontComeBack").SetActive(false);
            Koke.extinted = kokeExtinted;
            Barbara.extinted = barbExtinted;
            kokePhases[0] = Koke.conversationPhase;
            kokePhases[1] = Koke.startingPhase;
            kokePhases[2] = Koke.repeatedPhase;
            kokePhases[3] = Koke.negationPhase;
            barbaraPhases[0] = Barbara.conversationPhase;
            barbaraPhases[1] = Barbara.startingPhase;
            barbaraPhases[2] = Barbara.repeatedPhase;
            barbaraPhases[3] = Barbara.negationPhase;
        }
        if (!canActivatePuzzle && scene.name.Equals("FirstStageHouse"))
        {
            Koke = GameObject.FindObjectOfType<Koke>();
            Barbara = GameObject.FindObjectOfType<Barbara>();
        }
        if(!canActivatePuzzle && scene.name.Equals("FirstStage"))
        {
            player.transform.Find("VirtualCam").GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
            GameObject[] storyTriggers = GameObject.FindGameObjectsWithTag("DontComeBack");
            for (int i = 0; i < storyTriggers.Length; i++)
            {
                storyTriggers[i].SetActive(!storyTriggers[i].gameObject.activeSelf);
            }
            GameObject[] firstWalls = GameObject.FindGameObjectsWithTag("FirstWall");
            for (int i = 0; i < firstWalls.Length; i++)
            {
                firstWalls[i].SetActive(false);
            }
            Koke = GameObject.FindObjectOfType<Koke>();
            Koke.gameObject.SetActive(false);

        }
        player.transform.position = GameObject.FindGameObjectWithTag("spawnLocation").gameObject.transform.position;
    }

    public void transportToMaria()
    {
        StartCoroutine(transitionToNewLevel("SecondStage"));
    }

    




}
