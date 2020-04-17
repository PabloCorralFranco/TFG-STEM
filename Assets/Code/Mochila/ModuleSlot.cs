using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModuleSlot : MonoBehaviour, IDropHandler
{
    private GameObject droppedObj;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        //Cogemos el objeto que ha sido movido hasta aquí.
        droppedObj = eventData.pointerDrag;
        Debug.Log(droppedObj.GetType());
        if(droppedObj != null && droppedObj.tag == "Mod")
        {
            droppedObj.transform.parent = this.transform;
            droppedObj.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            droppedObj.transform.position = this.transform.position;
            Debug.Log(this.transform);
            droppedObj.GetComponent<DragnDrop>().setInModule(true);
        }

    }
}
