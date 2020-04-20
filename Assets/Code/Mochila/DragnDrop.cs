using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragnDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,IDropHandler
{
    private RectTransform position;
    public GameObject canvas, originalParent;
    public AudioClip dragAudio;

    private AudioSource playerAudio;
    private Transform originalPosition;
    private CanvasGroup canvasGroup;
    private bool inModule = false;
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("STEM");
        position = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }
    //Cuando empieza el Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        playerAudio.PlayOneShot(dragAudio);
        originalParent = this.transform.parent.gameObject;
        this.transform.parent = canvas.transform;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    //Interfaz que nos permite mover los objetos
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        //eventData.delta contine la cantidad que se ha movido el objeto desde el frame anterior
        position.anchoredPosition += eventData.delta;
    }

    //Cuando acaba
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        checkNextState();
    }
    //Set para variable que nos dice si está ocupando un modulo
    public void setInModule(bool state)
    {
        inModule = state;
    }
    //Comprobamos nuevo estado. Si no está en el módulo lo devolvemos a su sitio, si esta, lo mantenemos en el módulo
    public void checkNextState()
    {
        if (!inModule)
        {
            this.transform.position = originalParent.transform.position;
            this.transform.parent = originalParent.transform;
        }
        setInModule(false);

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
