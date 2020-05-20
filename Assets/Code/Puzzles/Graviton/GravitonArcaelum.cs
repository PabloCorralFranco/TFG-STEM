using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitonArcaelum : MonoBehaviour
{
    public bool canDeflect;
    public bool deflected, opened;
    private Player player;
    private float timePassed;
    private bool canSuck;
    private Animator anim;
    private Rigidbody2D rb, arcRb, myRb;
    private Vector3 lastPlayerPosition;
    private Arcaelum arcaelum;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        canSuck = false;
        player = FindObjectOfType<Player>();
        anim = GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        arcaelum = FindObjectOfType<Arcaelum>();
        arcRb = arcaelum.GetComponent<Rigidbody2D>();
        myRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;
        if (timePassed >= 3)
        {
            canSuck = !canSuck;
            timePassed = 0;
            if (canSuck)
            {
                anim.SetTrigger("open");
                opened = true;
            }
            else
            {
                opened = false;
                deflected = false;
                anim.SetTrigger("close");
            }
        }
        if (canSuck)
        {

            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = (myPos - rb.position).normalized;
            Vector2 force = direction * Time.fixedDeltaTime;
            float distance = (myPos - rb.position).magnitude;
            float distanceToArc = (myPos - arcRb.position).magnitude;
            Vector2 directionToArc = (myPos - arcRb.position).normalized;
            if(distance <= 2f && deflected)
            {
                Debug.Log("Applying Force to Arcaelum");
                arcRb.velocity = Vector3.zero;
                arcRb.AddForce(directionToArc * Time.fixedDeltaTime * 2500);
            }
            if (distance <= 1.5f && !deflected)
            {
                rb.AddForce(direction * Time.fixedDeltaTime * 2500);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Robamos vida");
        if (!deflected)
        {
            StartCoroutine(drainLife());
        }
        else
        {
            StartCoroutine(drainMyself());
        }
    }

    private IEnumerator drainLife()
    {
        while (canSuck)
        {
            player.setLife(-2);
            yield return new WaitForSeconds(.5f);
        }
    }
    private IEnumerator drainMyself()
    {
        while (canSuck)
        {
            arcaelum.drainLife(-5);
            yield return new WaitForSeconds(.5f);
        }
    }

    public void deflect()
    {
        //Nos han atacado asi que deflectamos en la direccion que este mirando la jugadora
        if (!canDeflect && !deflected && !opened) return;
        string zoneLoc = player.getZoneLoc();
        switch (zoneLoc)
        {
            case "bottom":
                myRb.velocity = Vector3.zero;
                deflected = true;
                myRb.AddForce(Vector2.down * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "up":
                myRb.velocity = Vector3.zero;
                deflected = true;
                myRb.AddForce(Vector2.up * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "right":
                myRb.velocity = Vector3.zero;
                deflected = true;
                myRb.AddForce(Vector2.right * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "left":
                myRb.velocity = Vector3.zero;
                deflected = true;
                myRb.AddForce(Vector2.left * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            default:
                Debug.Log("error");
                break;
        }
    }

}
