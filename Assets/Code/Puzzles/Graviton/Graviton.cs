using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graviton : MonoBehaviour
{
    private Player player;
    private float timePassed;
    private bool canSuck;
    private Animator anim;
    private Rigidbody2D rb;
    private Vector3 lastPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        timePassed = 0;
        canSuck = false;
        player = FindObjectOfType <Player>();
        anim = GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        timePassed += Time.fixedDeltaTime;
        if(timePassed >= 5)
        {
            canSuck = !canSuck;
            timePassed = 0;
            if (canSuck)
            {
                anim.SetTrigger("open");
                lastPlayerPosition = player.transform.position;
            }
            else
            {
                anim.SetTrigger("close");
            }
        }
        if (canSuck)
        {
            
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = (myPos - rb.position).normalized;
            Vector2 force = direction * Time.fixedDeltaTime;
            float distance = (myPos - rb.position).magnitude;
            if(distance <= 1.5f)
            {
                rb.AddForce(direction * Time.fixedDeltaTime * 2500);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Robamos vida");
        if (collision.tag.Equals("Player"))
        {
            StartCoroutine(drainLife());
        }
    }

    private IEnumerator drainLife()
    {
        while (canSuck)
        {
            Debug.Log("in co");
            player.setLife(-2);
            yield return new WaitForSeconds(.5f);
        }
    }

}
