using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PilarMatrix : MonoBehaviour
{
    public GameObject Blue,Red,Green;
    public BoxCollider2D myCollider;
    public GameObject genCanvas;
    public Sprite blueSprite;
    private Player player;
    private Inventory inv;
    private GeneticManager genManager;
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        inv = GameObject.FindObjectOfType<Inventory>();
        //genCanvas = this.transform.Find("Genetics").gameObject;
        genManager = GameObject.FindObjectOfType<GeneticManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Llamamos al canvas para que nos pida para elegir que esencia meter.
        player.stopFromMoving();
        player.transform.Find("MnACanvas").gameObject.SetActive(false);
        player.transform.Find("Abilities").gameObject.SetActive(false);
        //Debug.Log("Encendemos canvas segun condiciones de inventario");
        

        if (genManager.genPhase == 0)
        {
            genCanvas.transform.Find("Azul").GetComponentInChildren<TextMeshProUGUI>().text = "Esencia Azul - Alelo A";
            genCanvas.transform.Find("Rojo").GetComponentInChildren<TextMeshProUGUI>().text = "Esencia Roja - Alelo r";
        }
        else
        {
            genCanvas.transform.Find("Azul").GetComponentInChildren<TextMeshProUGUI>().text = "Esencia Azul - Alelo A";
            genCanvas.transform.Find("Rojo").GetComponentInChildren<TextMeshProUGUI>().text = "Esencia Azul - Alelo r";
        }
        genCanvas.SetActive(true);
        if (inv.greenEsence < 2)
        {
            genCanvas.transform.Find("Verde").GetComponent<Button>().interactable = false;
        }
        else
        {
            genCanvas.transform.Find("Verde").GetComponent<Button>().interactable = true;
        }
        if (inv.redEsence < 2)
        {
            if (genManager.genPhase == 1 && inv.blueEsence >= 2)
            {
                genCanvas.transform.Find("Rojo").GetComponent<Button>().interactable = true;
            }
            else
            {
                genCanvas.transform.Find("Rojo").GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            genCanvas.transform.Find("Rojo").GetComponent<Button>().interactable = true;
        }
        if (inv.blueEsence < 2)
        {
            genCanvas.transform.Find("Azul").GetComponent<Button>().interactable = false;
        }
        else
        {
            genCanvas.transform.Find("Azul").GetComponent<Button>().interactable = true;
        }
    }

    public void introduceEssence(GameObject essence)
    {
        Debug.Log("Funcionamos");
        Vector3 position = this.transform.position + new Vector3(0, .1f, 0);
        GameObject essenceInfo = Instantiate(essence, position, Quaternion.identity);
        essenceInfo.transform.SetParent(this.transform);
        SpriteRenderer spr = essenceInfo.GetComponent<SpriteRenderer>();

        if (genManager.genPhase == 1)
        {
            Debug.Log("entramos");
            spr.sprite = blueSprite;
        }
        //Tenemos que restar la esencia correcta
        Drop d = essence.GetComponent<Drop>();
        if (d.color.Equals("A"))
        {
            inv.blueEsence -= 2;
        }else if (d.color.Equals("r"))
        {
            if(genManager.genPhase == 1)
            {
                inv.blueEsence -= 2;
            }
            else
            {
                inv.redEsence -= 2;
            }
            
        }
        else
        {
            inv.greenEsence -= 2;
        }

        myCollider.enabled = false;
        exitGenetics();
    }

    public void exitGenetics()
    {
        player.transform.Find("MnACanvas").gameObject.SetActive(true);
        player.transform.Find("Abilities").gameObject.SetActive(true);
        genCanvas.SetActive(false);
        player.continueMoving();
    }

    public void reactivateCollider()
    {
        myCollider.enabled = true;
    }
}
