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
        if (isPushed) return;
        
        if (!isPushed)
        {
            Debug.Log("entering2");
            myRender.sprite = pushed;
            isPushed = true;
            if(primManager.isInitialVertexNull()) primManager.setInitialVertex(vertexName);
            primManager.connectedPort(vertexName, true);
        }
        
    }

    public void depush()
    {
        myRender.sprite = notPushed;
        isPushed = false;
    }
}
