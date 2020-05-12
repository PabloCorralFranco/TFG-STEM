using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GeneticManager : MonoBehaviour
{
    public GameObject column1, column2, row1, row2;
    public Sprite notPushed, pushed;
    public TextMeshProUGUI information;
    [TextArea(1,10)]
    public string[] textos;
    public AudioClip correct, incorrect;
    private SpriteRenderer mySprite;
    private GameObject filialCanvas;
    private Player player;
    private PilarMatrix[] pilars;
    private SpawnGeneration spawner;
    private AudioSource myAudio;
    public int genPhase;

    private void Awake()
    {
        genPhase = 0;   
    }
    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        filialCanvas = transform.Find("FilialInformation").gameObject;
        player = GameObject.FindObjectOfType<Player>();
        spawner = FindObjectOfType<SpawnGeneration>();
        spawner.spawnByPhase(genPhase);
        information.text = textos[genPhase];
        myAudio = player.GetComponent<AudioSource>();
    }
    //Esta clase se encargara de la comprobacion de la validez de la solucion y como se va yendo paso a paso
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Lets start creating!");
        mySprite.sprite = pushed;
        //Impedimos el movimiento del jugador
        player.stopFromMoving();
        //Mostramos canvas con información de la generación actual y desactivamos los de player
        player.transform.Find("MnACanvas").gameObject.SetActive(false);
        player.transform.Find("Abilities").gameObject.SetActive(false);
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

    //Limitacion de colores - En una misma fila o columna solo puede estar el mismo color.
    //Se tendran que crear los genes. Eliminas enemigos, consigues sus esencias. Las transformas en Alelos del gen.

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
        checkSolution(matrix);
    }

    public void checkSolution(string[,] matrix)
    {
        int heterocigoticos, homocigoticosDominantes, homocigoticosRecesivos;
        //Primera solucion
        if (genPhase == 0)
        {
            heterocigoticos = 0;
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j].Equals("Ar"))
                    {
                        heterocigoticos += 1;
                    }
                }
            }
            if(heterocigoticos == 4)
            {
                genPhase++;
                //Llamar al spawnManager, crear segunda generacion y poner efectos y musica.
                Debug.Log("Primera Generacion CORRECTA");
                information.text = textos[genPhase];
                spawner.spawnByPhase(genPhase);
                myAudio.PlayOneShot(correct);
            }
            else
            {
                //Llamar al spawnManager con generacion erronea y no moverse de fase.
                Debug.Log("Primera Generacion INCORRECTA. SE VUELVE A GENERACION INICIAL");
                information.text = textos[genPhase];
                spawner.spawnByPhase(genPhase);
                myAudio.PlayOneShot(incorrect);
            }
            return;
        }
        //Segunda Solucion

        if(genPhase == 1)
        {
            heterocigoticos = 0;
            homocigoticosDominantes = 0;
            homocigoticosRecesivos = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j].Equals("Ar"))
                    {
                        heterocigoticos += 1;
                    }
                    if (matrix[i, j].Equals("AA"))
                    {
                        homocigoticosDominantes += 1;
                    }
                    if (matrix[i, j].Equals("rr"))
                    {
                        homocigoticosRecesivos += 1;
                    }
                }
            }

            if(heterocigoticos == 2 && homocigoticosRecesivos == 1 && homocigoticosDominantes == 1)
            {
                //Hemos alcanzado la relacion fenotipica deseada. Mandamos mensaje al event manager de que hemos ganado
                Debug.Log("HEMOS GANADO");
                genPhase++;
                information.text = textos[genPhase];
                myAudio.PlayOneShot(correct);
            }
            else
            {
                Debug.Log("Nos hemos equivocado, volvemos a la generacion cero");
                genPhase = 0;
                information.text = textos[genPhase];
                spawner.spawnByPhase(genPhase);
                myAudio.PlayOneShot(incorrect);
            }

        }

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
        player.transform.Find("MnACanvas").gameObject.SetActive(true);
        player.transform.Find("Abilities").gameObject.SetActive(true);
        player.continueMoving();
    }

}
