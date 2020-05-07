using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PilarMatrix : MonoBehaviour
{
    public GameObject Blue,Red,Green;
    public BoxCollider2D myCollider;
    public GameObject genCanvas;
    private Player player;
    private Inventory inv;
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        inv = GameObject.FindObjectOfType<Inventory>();
        //genCanvas = this.transform.Find("Genetics").gameObject;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Llamamos al canvas para que nos pida para elegir que esencia meter.
        player.stopFromMoving();
        //Debug.Log("Encendemos canvas segun condiciones de inventario");
        genCanvas.SetActive(true);
        if (inv.greenEsence < 2)
        {
            genCanvas.transform.Find("Verde").GetComponent<Button>().interactable = false;
        }
        if (inv.redEsence < 2)
        {
            genCanvas.transform.Find("Rojo").GetComponent<Button>().interactable = false;
        }
        if (inv.blueEsence < 2)
        {
            genCanvas.transform.Find("Azul").GetComponent<Button>().interactable = false;
        }
    }

    public void introduceEssence(GameObject essence)
    {
        Debug.Log("Funcionamos");
        inv.blueEsence -= 2;
        Vector3 position = this.transform.position + new Vector3(0, .1f, 0);
        GameObject essenceInfo = Instantiate(essence,position,Quaternion.identity);
        essenceInfo.transform.SetParent(this.transform);
        myCollider.enabled = false;
        exitGenetics();
    }

    public void exitGenetics()
    {
        genCanvas.SetActive(false);
        player.continueMoving();
    }
}
