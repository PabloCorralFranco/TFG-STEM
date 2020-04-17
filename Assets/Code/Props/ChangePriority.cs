using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePriority : MonoBehaviour
{
    public BoxCollider2D priorityTrigger;
    public string layerBack;
    public string layerFront;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        /*if (spriteRenderer)
        {
            spriteRenderer.sortingLayerName = layerBack;
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spriteRenderer){
            spriteRenderer.sortingLayerName = layerFront;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spriteRenderer)
        {
            spriteRenderer.sortingLayerName = layerBack;
        }
    }

}
