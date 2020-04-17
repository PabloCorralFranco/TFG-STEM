using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public string genName;
    public int need;
    public GameObject modButton;

    private Inventory inventory;
    private ModuleSlot[] contentBag;
    private int have;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    public void GenerateModule()
    {
        int index = findSlot();
        //Comprobamos el hueco libre
        if(index != -1)
        {
            //Si hay hueco libre lanzamos funcion
            StartCoroutine(genName,index);
        }
        
    }

    private IEnumerator green(int index)
    {
        //You need the specified number in the public field
        have = inventory.greenEsence;
        if(have >= need)
        {
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.greenEsence -= need;
        }
        yield return null;
    }
    private IEnumerator red(int index)
    {
        //You need the specified number in the public field
        have = inventory.redEsence;
        if (have >= need)
        {
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.redEsence -= need;
        }
        yield return null;
    }
    private IEnumerator blue(int index)
    {
        //You need the specified number in the public field
        have = inventory.blueEsence;
        if (have >= need)
        {
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.blueEsence -= need;
        }
        yield return null;
    }

    private int findSlot()
    {
        //Se actualizan en cada llamada
        contentBag = GameObject.FindGameObjectWithTag("ContentBag").GetComponentsInChildren<ModuleSlot>();
        int i = 0;
        int index = -1;
        bool outCheck = false;
        while(i < contentBag.Length && !outCheck)
        {
            if(contentBag[i].tag == "Slot" && contentBag[i].GetComponentInChildren<DragnDrop>() == null)
            {
                //GameObject instance = Instantiate(modButton, contentBag[i].transform.position, Quaternion.identity);
                outCheck = true;
                index = i;
            }
            i++;
        }
        return index;
    }

    private void instantiateMod(int index)
    {
        //Dejar asi aunque salga doble clon
        Debug.Log("hi");
        contentBag = GameObject.FindGameObjectWithTag("ContentBag").GetComponentsInChildren<ModuleSlot>();
        //Generamos en el hueco libre
        GameObject nmod = Instantiate(modButton, contentBag[index].transform.position, Quaternion.identity, contentBag[index].transform);
        //nmod.transform.SetParent(contentBag[index].transform, true);
        //modButton.transform.position = contentBag[index].transform.position;
        //nmod.transform.SetParent(contentBag[index].transform, true);
    }
}
