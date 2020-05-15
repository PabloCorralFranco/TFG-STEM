using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    public Arista arista;
    private Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    public void changeColliderState(bool isActive)
    {
        myCollider.enabled = isActive;
    }

    
}
