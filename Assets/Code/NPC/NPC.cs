﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int conversationPhase, startingPhase, repeatedPhase, negationPhase;
    public TextoConver[] conversationsByPhases;
    public TextoConver[] negationByPhases;
    public TextoConver[] startingConversations;
    public TextoConver[] repeated;

    private ConverManager manager;
    private Player player;
    //UI managment to trigger the UI.
    private Button hablar, nada, next;
    private GameObject conversationCanvas;
    private GameObject movementCanvas, abilityCanvas;
    public bool extinted = false;
    private AudioSource npcAudio;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        manager = GameObject.FindObjectOfType<ConverManager>();
        conversationCanvas = player.transform.Find("Conversations").gameObject;
        hablar = conversationCanvas.transform.Find("Hablar").GetComponent<Button>();
        nada = conversationCanvas.transform.Find("Nada").GetComponent<Button>();
        next = conversationCanvas.transform.Find("NextButton").GetComponent<Button>();
        movementCanvas = player.transform.Find("MnACanvas").gameObject;
        abilityCanvas = player.transform.Find("Abilities").gameObject;
        npcAudio = GetComponent<AudioSource>();
    }

    //Revisar esta funcion - cuando se hace pop up de la historia se pone a true y da errores cuando no deberia
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Cogemos una referencia en Start a player.
        //Mandamos a player la variable verdadera de que puede hablar con un NPC y a su vez asignamos el npc.
        //Una vez se ha asignado si el jugador ataca pero esta para hablar se triggereara la conversación y no hara el slash
        if(collision.tag == "Player")
        {
            player.setToTalk(true, this);
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.setToTalk(false, this);
        }
    }


    public void awakeConversationMethods()
    {
        //En este metodo necesitamos activar la interfaz de conversación junto con los botones
        //Necesitamos poner tiempo de espera para que no se solapen las instrucciones
        StartCoroutine(waitBeforeOnClickSettings());
        
    }

    private IEnumerator waitBeforeOnClickSettings()
    {
        //Por ahora queda comentado puesto que si desactivamos el movement canvas desactivamos al npc temporal
        movementCanvas.SetActive(!movementCanvas.activeSelf);
        abilityCanvas.SetActive(!abilityCanvas.activeSelf);
        conversationCanvas.SetActive(!conversationCanvas.activeSelf);
        yield return new WaitForSeconds(.1f);
        if (extinted)
        {
            //changeButtonsState();
            next.gameObject.SetActive(!next.gameObject.activeSelf);
            manager.showConversations(repeated[repeatedPhase], npcAudio);
            yield return null;
        }
        else
        {
            hablar.onClick.RemoveAllListeners();
            hablar.onClick.AddListener(delegate { startConversation(); });
            nada.onClick.RemoveAllListeners();
            nada.onClick.AddListener(delegate { endConversation(); });
            if (manager.gameObject.activeSelf)
            {
                manager.showConversations(startingConversations[startingPhase], npcAudio);
            }
            yield return new WaitForSeconds(1f);
            changeButtonsState();
        }
        
        
    }

    public void popUpMeeting()
    {
        StartCoroutine(meetings());
    }

    private IEnumerator meetings()
    {
        //Por ahora queda comentado puesto que si desactivamos el movement canvas desactivamos al npc temporal
        movementCanvas.SetActive(!movementCanvas.activeSelf);
        abilityCanvas.SetActive(!abilityCanvas.activeSelf);
        conversationCanvas.SetActive(!conversationCanvas.activeSelf);
        yield return new WaitForSeconds(.1f);
        changeButtonsState();
        startConversation();
    }


    //Estas dos funciones nos representan los botones de HABLAR y NADA.
    public void startConversation()
    {
        //gamePhase tendrá que ser consultado al manager del juego. De primeras se dejará en 0.
        changeButtonsState();
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        manager.showConversations(conversationsByPhases[conversationPhase],npcAudio);
        extinted = true;
    }
    public void endConversation()
    {
        changeButtonsState();
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        manager.showConversations(negationByPhases[negationPhase],npcAudio);
    }

    private void changeButtonsState()
    {
        //Debug.Log(!hablar.gameObject.activeSelf);
        hablar.gameObject.SetActive(!hablar.gameObject.activeSelf);
        nada.gameObject.SetActive(!nada.gameObject.activeSelf);
    }
}
