using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compiler : MonoBehaviour
{
    public GameObject[] slots;
    public AudioClip compilerClip;
    public AudioSource playerAudio;

    private string composition = "";
    private GameObject abilityCanvas;
    private PowerUpManager manager;
    private ModulesCount mc;

    

    private void Start()
    {
        abilityCanvas = GameObject.FindGameObjectsWithTag("AbilitieManager")[0]; ;
        manager = abilityCanvas.GetComponent<PowerUpManager>();
        mc = GameObject.FindGameObjectWithTag("Generator").GetComponent<ModulesCount>();
    }

    public void startCompiling()
    {
        //Accedemos a la composición de palabras
        composition = "";
        int i = 0;
        int slotCount = 0;
        while(i < slots.Length) {
            if (slots[i].GetComponentInChildren<Module>() != null)
            {
                composition += slots[i].GetComponentInChildren<Module>().modName;
                //No existe problema de integridad en el borrado
                Destroy(slots[i].GetComponentInChildren<Module>().gameObject);
                slotCount++;
            }
            i++;
        }
        mc.diffBagSpace(slotCount);

        //Analizamos la composición
        GameObject temp = manager.returnPowerUpInstance(composition);

        //Accedemos al canvas de habilidades y a sus hijos y ponemos el boton donde corresponda
        CanvasRenderer[] ability = abilityCanvas.GetComponentsInChildren<CanvasRenderer>();
        i = 0;
        bool outCheck = false;
        while(i < ability.Length && !outCheck)
        {
            if (ability[i].GetComponentInChildren<PowerUp>() == null && ability[i].tag == "Slot")
            {
                outCheck = true;
                playerAudio.PlayOneShot(compilerClip);
                GameObject nbutton = Instantiate(temp,ability[i].transform.position,Quaternion.identity, ability[i].transform);
                //nbutton.transform.SetParent(ability[i].transform);
            }
            i++;
        }
    }

    
}
