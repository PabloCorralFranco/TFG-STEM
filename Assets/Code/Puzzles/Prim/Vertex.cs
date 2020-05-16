using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    public string vertexName;
    public Sprite pushed, notPushed;
    private bool isPushed;
    private SpriteRenderer myRender;
    private PrimManager primManager;

    private void Start()
    {
        isPushed = false;
        myRender = GetComponent<SpriteRenderer>();
        primManager = FindObjectOfType<PrimManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPushed)
        {
            myRender.sprite = pushed;
            isPushed = true;
            if(primManager.isInitialVertexNull()) primManager.setInitialVertex(vertexName);
            primManager.connectedPort(vertexName, true);
        }
        else
        {
            isPushed = false;
        }
        
    }

    //Se hace con motivo de dar un efecto mas realista
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isPushed)
        {
            depush();
            primManager.connectedPort(vertexName, false);
        }
    }

    public void depush()
    {
        myRender.sprite = notPushed;
        isPushed = false;
    }
}
