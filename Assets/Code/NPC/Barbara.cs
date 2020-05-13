using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbara : NPC
{
    public void MoveToTable()
    {
        StartCoroutine(routeToTable());
    }

    private IEnumerator routeToTable()
    {
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("speed", 1);
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(.5f);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 1);
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 40));
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("speed", 0);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
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
        anim.SetFloat("speed", 1);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
        rb.AddForce(new Vector2(0, -40));
        yield return new WaitForSeconds(.8f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", -1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(-40, 0));
        yield return new WaitForSeconds(.5f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("speed", 0);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", -1);
    }
    public void backToHouse()
    {
        StartCoroutine(routeToHouse());
    }
    private IEnumerator routeToHouse()
    {
        //abajo-izquierda-mirar abajo
        Animator anim = GetComponent<Animator>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        anim.SetFloat("speed", 1);
        anim.SetFloat("Horizontal", 0);
        anim.SetFloat("Vertical", 1);
        rb.AddForce(new Vector2(0, 40));
        yield return new WaitForSeconds(.8f);
        rb.velocity = Vector2.zero;
        anim.SetFloat("Horizontal", 1);
        anim.SetFloat("Vertical", 0);
        rb.AddForce(new Vector2(40, 0));
        yield return new WaitForSeconds(1f);
        anim.SetFloat("speed", 0);
        rb.velocity = Vector2.zero;
    }
}
