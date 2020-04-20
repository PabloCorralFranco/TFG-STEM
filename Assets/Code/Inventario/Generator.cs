using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Generator : MonoBehaviour
{
    public string genName;
    public int need;
    public GameObject modButton;
    public TextMeshProUGUI haveTxT;
    public AudioClip buttonAudio;

    private AudioSource playerAudio;
    private Inventory inventory;
    private ModuleSlot[] contentBag;
    private int have;
    private ModulesCount mc;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        mc = GameObject.FindGameObjectWithTag("Generator").GetComponent<ModulesCount>();
        playerAudio = inventory.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        checkInventory();
    }

    public void checkInventory()
    {
        if (genName.Equals("blue"))
        {
            haveTxT.text = inventory.blueEsence.ToString();
        }
        else if (genName.Equals("green"))
        {
            haveTxT.text = inventory.greenEsence.ToString();
        }
        else if (genName.Equals("red"))
        {
            haveTxT.text = inventory.redEsence.ToString();
        }
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
        if(index == -1)
        {
            //Reproducimos pitido de que no se puede crear más.
            //Quizas una frase de que no se puede mas

        }
        
    }

    private IEnumerator green(int index)
    {
        //You need the specified number in the public field
        have = inventory.greenEsence;
        if(have >= need)
        {
            playSound();
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.greenEsence -= need;
            haveTxT.text = inventory.greenEsence.ToString();
        }
        yield return null;
    }
    private IEnumerator red(int index)
    {
        //You need the specified number in the public field
        have = inventory.redEsence;
        if (have >= need)
        {
            playSound();
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.redEsence -= need;
            haveTxT.text = inventory.redEsence.ToString();
        }
        yield return null;
    }
    private IEnumerator blue(int index)
    {
        //You need the specified number in the public field
        have = inventory.blueEsence;
        if (have >= need)
        {
            playSound();
            instantiateMod(index);
            //Quitamos del inventario need. Cambiar estos campos por metodos
            inventory.blueEsence -= need;
            haveTxT.text = inventory.blueEsence.ToString();
        }
        yield return null;
    }

    private int findSlot()
    {
        int occupiedSlots = 0;
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
        findOccupiedSlots();
        return index;
    }

    private void findOccupiedSlots()
    {
        int occupiedSlots = 1;
        int i = 0;
        while (i < contentBag.Length)
        {
            if (contentBag[i].tag == "Slot" && contentBag[i].GetComponentInChildren<DragnDrop>() != null)
            {
                occupiedSlots++;
            }
            i++;
        }
        if (occupiedSlots > 10)
        {
            occupiedSlots = 10;
        }
        mc.setTextToBagSpace(occupiedSlots);
    }

    private void playSound()
    {
        playerAudio.PlayOneShot(buttonAudio);
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
