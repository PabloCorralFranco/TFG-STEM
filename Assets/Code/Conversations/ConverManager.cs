using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConverManager : MonoBehaviour
{
    //Esta clase se va a encargar de ejecutar la conversacion pertinente siguiendo las pautas definidas en el GDD.
    public TextMeshProUGUI cuerpo, titulo;
    public GameObject conversationCanvas;

    private Queue<string> frasesQueue;
    private Queue<string> nombresQueue;

    private void Start()
    {
        frasesQueue = new Queue<string>();
        nombresQueue = new Queue<string>();
    }

    public void showConversations(TextoConver texto)
    {
        //Nos aseguramos de que este vacio
        frasesQueue.Clear();
        //Encolamos las frases y nombres de texto
        foreach(string frase in texto.frases)
        {
            frasesQueue.Enqueue(frase);
        }
        foreach (string nombre in texto.characterName)
        {
            nombresQueue.Enqueue(nombre);
        }
        nextIteration();
    }

    public void nextIteration()
    {

        if(frasesQueue.Count == 0 || nombresQueue.Count == 0)
        {
            EndConversation();
            return;
        }

        string frase = frasesQueue.Dequeue();
        string nombre = nombresQueue.Dequeue();
        titulo.text = nombre;
        StopAllCoroutines();
        StartCoroutine(speakEffect(frase));

    }

    private IEnumerator speakEffect(string frase)
    {
        cuerpo.text = "";
        foreach(char c in frase)
        {
            //Ponemos el texto en la caja de texto de la conversación
            cuerpo.text += c;
            yield return null;
        }
        
    }


    public void EndConversation()
    {
        cuerpo.text = "";
        conversationCanvas.SetActive(false);
    }


}
