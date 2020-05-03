using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityTree : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer secondPart;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
        }
        if (secondPart)
        {
            secondPart.color = new Color(1f, 1f, 1f, .5f);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (spriteRenderer)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        if (secondPart)
        {
            secondPart.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
