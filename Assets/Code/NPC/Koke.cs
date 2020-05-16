using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koke : NPC
{
    public GameObject effect;
    //This scripts goberns the special movements and sprites Koke will use. Extends from NPC having all the possible capabilities.


    public void MoveToPosition()
    {
        StartCoroutine(routeToHouse());
    }

    private IEnumerator routeToHouse()
    {
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40,0));
        yield return new WaitForSeconds(2f);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, -40));
        yield return new WaitForSeconds(2f);
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(1f);
        Debug.Log("Hemos terminado");
    }

    public void destroyAndScalate()
    {
        Destroy(effect.gameObject);
        StartCoroutine(scaleKoke());

    }

    private IEnumerator scaleKoke()
    {
        while(transform.localScale.x < 1 && transform.localScale.y < 1)
        {
            transform.localScale = new Vector3(transform.localScale.x + .1f, transform.localScale.y + .1f, transform.localScale.z + .1f);
            yield return null;
        }
    }


    public void MoveToBed()
    {
        StartCoroutine(routeToBed());
    }

    private IEnumerator routeToBed()
    {
        //derecha-abajo-derecha-mirar a la izquierda
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("Horizontal", 1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(40, 0));
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
        rb.AddForce(new Vector2(0, -40));
        yield return new WaitForSeconds(.8f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", 1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(40,0));
        yield return new WaitForSeconds(1.5f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.isKinematic = true;
        yield return null;
    }

    public void MoveToGenerator()
    {
        StartCoroutine(routeToGenerator());
    }

    private IEnumerator routeToGenerator()
    {
        //abajo-izquierda-mirar abajo
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
        rb.AddForce(new Vector2(0, -40));
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
    }

    public void MoveToForest()
    {
        StartCoroutine(routeToForest());
    }

    private IEnumerator routeToForest()
    {
        //izquierda
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(4f);
    }

    public void exitForest()
    {
        StartCoroutine(exitForestRoute());
    }

    private IEnumerator exitForestRoute()
    {
        //izquierda
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(1f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 1);
        rb.AddForce(new Vector2(0, 40));
        yield return new WaitForSeconds(2f);
    }


}
