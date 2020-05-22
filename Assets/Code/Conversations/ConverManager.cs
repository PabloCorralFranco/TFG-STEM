using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConverManager : MonoBehaviour
{
    //Esta clase se va a encargar de ejecutar la conversacion pertinente siguiendo las pautas definidas en el GDD.
    public TextMeshProUGUI cuerpo, titulo;
    public GameObject conversationCanvas, next;
    public AudioClip talkEffect;

    private Queue<string> frasesQueue;
    private Queue<string> nombresQueue;
    private Queue<Color32> coloresQueue;
    public GameObject movementCanvas, abilityCanvas;
    private AudioSource mySource;
    private bool canPlay = true;

    private void Start()
    {
        frasesQueue = new Queue<string>();
        nombresQueue = new Queue<string>();
        coloresQueue = new Queue<Color32>();
    }

    public void showConversations(TextoConver texto, AudioSource sourcePlay)
    {
        //Nos aseguramos de que este vacio
        frasesQueue.Clear();
        nombresQueue.Clear();
        coloresQueue.Clear();
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
        titulo.text = nombre;
        titulo.color = color;
        StopAllCoroutines();
        StartCoroutine(speakEffect(frase,color));

    }

    private IEnumerator speakEffect(string frase, Color32 color)
    {
        Debug.Log("hehe1");
        cuerpo.text = "";
        foreach (char c in frase)
        {
            //Ponemos el texto en la caja de texto de la conversación
            cuerpo.text += c;
            cuerpo.color = color;
            if(canPlay) StartCoroutine(playtextAudio());
            yield return null;
        }
        canPlay = true;
    }
    private IEnumerator playtextAudio()
    {
        canPlay = false;
        mySource.PlayOneShot(talkEffect);
        yield return new WaitForSeconds(.1f);
        canPlay = true;
    }


    public void EndConversation()
    {
        cuerpo.text = "";
        GameObject.FindObjectOfType<EventManager>().isTaskPending = false;
        GameObject.FindObjectOfType<Player>().setToTalk(false, null);
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        movementCanvas.SetActive(!movementCanvas.activeSelf);
        abilityCanvas.SetActive(!abilityCanvas.activeSelf);
        cuerpo.text = "";
        conversationCanvas.SetActive(!conversationCanvas.activeSelf);
    }


}
