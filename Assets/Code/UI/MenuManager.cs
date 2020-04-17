using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject generator, compiler;
    public void openClose()
    {
        menu.SetActive(!menu.activeSelf);
    }
    public void openCloseGenerator()
    {
        compiler.SetActive(!compiler.activeSelf);
        generator.SetActive(!generator.activeSelf);
        openClose();
    }
    public void openCloseCompiler()
    {
        compiler.SetActive(!compiler.activeSelf);
        openClose();
    }
}
