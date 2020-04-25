using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConverManager : MonoBehaviour
{
    //Esta clase se va a encargar de ejecutar la conversacion pertinente siguiendo las pautas definidas en el GDD.
    public TextMeshProUGUI cuerpo, titulo;
    public GameObject conversationCanvas, next;

    private Queue<string> frasesQueue;
    private Queue<string> nombresQueue;
    private Queue<AudioClip> sonidosQueue;
    private Queue<Color32> coloresQueue;
    public GameObject movementCanvas, abilityCanvas;
    private AudioSource mySource;

    private void Start()
    {
        frasesQueue = new Queue<string>();
        nombresQueue = new Queue<string>();
        sonidosQueue = new Queue<AudioClip>();
        coloresQueue = new Queue<Color32>();
    }

    public void showConversations(TextoConver texto, AudioSource sourcePlay)
    {
        //Nos aseguramos de que este vacio
        frasesQueue.Clear();
        mySource = sourcePlay;
        //Encolamos las frases y nombres de texto
        foreach (string frase in texto.frases)
        {
            frasesQueue.Enqueue(frase);
        }
        foreach (string nombre in texto.characterName)
        {
            nombresQueue.Enqueue(nombre);
        }
        foreach(AudioClip clip in texto.sonidoFrases)
        {
            sonidosQueue.Enqueue(clip);
        }
        foreach(Color color in texto.colorFrases)
        {
            coloresQueue.Enqueue(color);
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
        Color color = coloresQueue.Dequeue();
        AudioClip audio = sonidosQueue.Dequeue();
        titulo.text = nombre;
        titulo.color = color;
        StopAllCoroutines();
        StartCoroutine(speakEffect(frase,color,audio));

    }

    private IEnumerator speakEffect(string frase, Color32 color, AudioClip audio)
    {
        cuerpo.text = "";
        mySource.PlayOneShot(audio);
        foreach (char c in frase)
        {
            //Ponemos el texto en la caja de texto de la conversación
            cuerpo.text += c;
            cuerpo.color = color;
            yield return null;
        }
        
    }


    public void EndConversation()
    {
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        movementCanvas.SetActive(!movementCanvas.activeSelf);
        abilityCanvas.SetActive(!abilityCanvas.activeSelf);
        cuerpo.text = "";
        conversationCanvas.SetActive(!conversationCanvas.activeSelf);
    }


}
