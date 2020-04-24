using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Necesario para poder editarlo como dato
[System.Serializable]
public class TextoConver
{
    public string[] characterName;
    [TextArea(1,10)]
    public string[] frases;
}
