using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public int gamePhase = 0;
    public TextoConver[] conversationsByPhases;
    public TextoConver[] negationByPhases;
    public TextoConver[] startingConversations;
    public ConverManager manager;
    public Button hablar, nada;

    //UI managment to trigger the UI.
    public GameObject conversationCanvas;

    private void Start()
    {
        //conversationCanvas = GameObject.FindGameObjectWithTag("Conversation");
    }


    public void awakeConversationMethods()
    {
        //En este metodo necesitamos activar la interfaz de conversación junto con los botones
        //Necesitamos poner tiempo de espera para que no se solapen las instrucciones
        StartCoroutine(waitBeforeOnClickSettings());
        
    }

    private IEnumerator waitBeforeOnClickSettings()
    {
        conversationCanvas.SetActive(true);
        yield return new WaitForSeconds(.1f);
        hablar.onClick.RemoveAllListeners();
        hablar.onClick.AddListener(delegate { startConversation(); });
        nada.onClick.RemoveAllListeners();
        nada.onClick.AddListener(delegate { endConversation(); });
        if (manager.gameObject.activeSelf)
        {
            manager.showConversations(startingConversations[gamePhase]);
        }
        yield return new WaitForSeconds(1f);
        changeButtonsState();
        
    }

    //Estas dos funciones nos representan los botones de HABLAR y NADA.
    public void startConversation()
    {
        //gamePhase tendrá que ser consultado al manager del juego. De primeras se dejará en 0.
        changeButtonsState();
        manager.showConversations(conversationsByPhases[gamePhase]);
    }
    public void endConversation()
    {
        changeButtonsState();
        manager.showConversations(negationByPhases[gamePhase]);
    }

    private void changeButtonsState()
    {
        //Debug.Log(!hablar.gameObject.activeSelf);
        hablar.gameObject.SetActive(!hablar.gameObject.activeSelf);
        nada.gameObject.SetActive(!nada.gameObject.activeSelf);
    }
}
