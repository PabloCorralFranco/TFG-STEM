using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public string vertexName;
    public Sprite pushed, notPushed;
    private bool isPushed;
    private SpriteRenderer myRender;

    private void Start()
    {
        isPushed = false;
        myRender = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPushed)
        {
            myRender.sprite = pushed;
            isPushed = true;
        }
        else
        {
            isPushed = false;
        }
        FindObjectOfType<PrimManager>().setInitialVertex(vertexName);
    }

    //Se hace con motivo de dar un efecto mas realista
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPushed)
        {
            myRender.sprite = notPushed;
        }
    }
}
