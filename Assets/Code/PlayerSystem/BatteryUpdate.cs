using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatteryUpdate : MonoBehaviour {

    public TextMeshProUGUI batteryPercentage;
    public Image panelLow, panel25, panel50, panelFull;
    public GameObject chargeParticle;

    private void Start()
    {
        batteryPercentage = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void changeBattery(float life)
    {

        GameObject chargeInstance = Instantiate(chargeParticle, transform.position, Quaternion.identity, transform);
        Destroy(chargeInstance, 0.2f);

        batteryPercentage.text = life.ToString() + "%";
        Color blue = new Color(0f, 188f, 255f);
        Color orange = new Color(255f, 141f, 0f);
        Color red = new Color(255f, 0f, 0f);

        if (life >= 75)
        {
            normalizePanelColor(panelFull, life, blue, 75, 100);
            normalizePanelColor(panel50, 100, blue, 75, 100);
            normalizePanelColor(panel25, 100, blue, 75, 100);
            normalizePanelColor(panelLow, 100, blue, 75, 100);
        }

        if (life >= 50 && life < 75)
        {
            normalizePanelColor(panel50, life,blue,50,75);
            normalizePanelColor(panel25, 75, blue, 50, 75);
            normalizePanelColor(panelLow, 75, blue, 50, 75);
        }
        if(life >= 25 && life < 50)
        {
            normalizePanelColor(panel25, life, orange,25,50);
            normalizePanelColor(panelLow, 50, orange, 25, 50);
        }
        if(life >= 0 && life < 25)
        {
            normalizePanelColor(panelLow, life, red,0,25);
        }
        panelFull.gameObject.SetActive(life >= 75);
        panel50.gameObject.SetActive(life >= 50);
        panel25.gameObject.SetActive(life >= 25);
        panelLow.gameObject.SetActive(life >= 0);
    }

    private void normalizePanelColor(Image panel, float life, Color combination,int min, int max)
    {
        float normalizedValue, result;
        normalizedValue = Mathf.InverseLerp(min, max, life);
        result = Mathf.Lerp(0, 1, normalizedValue);
        panel.color = new Color(combination.r,combination.g,combination.b, result);
    }

}
