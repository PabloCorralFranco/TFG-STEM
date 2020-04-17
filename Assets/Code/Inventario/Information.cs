using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour
{
    public GameObject panel;

    public void onOpenClose()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
}
