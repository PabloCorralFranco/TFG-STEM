using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticManager : MonoBehaviour
{
    public GameObject column1, column2, row1, row2;
    public Sprite notPushed, pushed;
    private SpriteRenderer mySprite;
    private GameObject filialCanvas;
    private Player player;
    private PilarMatrix[] pilars;
    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        filialCanvas = transform.Find("FilialInformation").gameObject;
        player = GameObject.FindObjectOfType<Player>();
    }
    //Esta clase se encargara de la comprobacion de la validez de la solucion y como se va yendo paso a paso
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Lets start creating!");
        mySprite.sprite = pushed;
        //Impedimos el movimiento del jugador
        player.stopFromMoving();
        //Mostramos canvas con información de la generación actual
        filialCanvas.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mySprite.sprite = notPushed;
        getOut();
    }

    public void cleanPilars()
    {
        cleanAndRecover(true);
    }

    public void newGeneration()
    {
        //Aqui es donde manejamos la generacion.
        //Dependiendo de un resultado u otro llamaremos a un spawnManager para que nos genere un porcentaje concreto de enemigos y de colores segun la generacion
        string[,] matrix = new string[2, 2];
        string colorCol1 = column1.GetComponentInChildren<Drop>().color;
        string colorCol2 = column2.GetComponentInChildren<Drop>().color;
        string colorRow1 = row1.GetComponentInChildren<Drop>().color;
        string colorRow2 = row2.GetComponentInChildren<Drop>().color;
        if(colorRow1.CompareTo(colorCol1) < 0) matrix[0, 0] = colorRow1 + colorCol1; else matrix[0, 0] = colorCol1 + colorRow1;
        if (colorRow1.CompareTo(colorCol2) < 0) matrix[0, 1] = colorRow1 + colorCol2; else matrix[0, 1] = colorCol2 + colorRow1;
        if (colorRow2.CompareTo(colorCol1) < 0) matrix[1, 0] = colorRow2 + colorCol1; else matrix[1, 0] = colorCol1 + colorRow2;
        if (colorRow2.CompareTo(colorCol2) < 0) matrix[1, 1] = colorRow2 + colorCol2; else matrix[1, 1] = colorCol2 + colorRow2;
        Debug.Log("0,0:" + matrix[0, 0]);
        Debug.Log("0,1:" + matrix[0, 1]);
        Debug.Log("1,0:" + matrix[1, 0]);
        Debug.Log("1,1:" + matrix[1, 1]);
        //Borramos los elementos de adn y no los recuperamos
        cleanAndRecover(false);
        //Ahora que tenemos la tabla de prunett nos hace falta interpretarla
        //Recorremos la matriz. Vemos si contiene la generacion filial F1 heterocigotica Ar
        
    }

    private void cleanAndRecover(bool canRecover)
    {
        pilars = GameObject.FindObjectsOfType<PilarMatrix>();

        foreach (PilarMatrix p in pilars)
        {
            p.reactivateCollider();
            Drop d = p.GetComponentInChildren<Drop>();
            if (d && canRecover)
            {
                d.recoverEssences();
                
            }else if (d)
            {
                Destroy(d.gameObject);
            }
        
        }
        getOut();
    }

    public void getOut()
    {
        filialCanvas.SetActive(false);
        player.continueMoving();
    }

}
