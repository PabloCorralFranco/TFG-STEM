using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mobileButton, titulo, texto, fondo, compilar, modulos, OCbutton;
    public GameObject programmingPanel;
    public GameObject abilityCanvas,mnaCanvas,generatorCanvas;
    bool panelState;

    public void StemTrigger()
    {
        if(programmingPanel != null)
        {
            panelState = programmingPanel.activeSelf;
            //abilityCanvas.SetActive(panelState);
            //mnaCanvas.SetActive(panelState);
            //generatorCanvas.SetActive(!panelState);
            OCbutton.SetActive(!panelState);
            programmingPanel.SetActive(!panelState);
            titulo.SetActive(!panelState);
            texto.SetActive(!panelState);
            fondo.SetActive(!panelState);
            compilar.SetActive(!panelState);
            modulos.SetActive(!panelState);
        }
    }
}
