using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModulesCount : MonoBehaviour
{
    public TextMeshProUGUI createdMods;
    public void setTextToBagSpace(int cuantity)
    {
        createdMods.text = cuantity.ToString();
    }
    public void diffBagSpace(int cuantity)
    {
        int value = int.Parse(createdMods.text) - cuantity;
        createdMods.text = value.ToString();
    }
}
