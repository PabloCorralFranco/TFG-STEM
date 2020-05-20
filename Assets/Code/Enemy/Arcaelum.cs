using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Arcaelum : MonoBehaviour
{
    //Clase enemigo. Primera implementación Dummy.
    public float enemyLife, normalDeffense, movementSpeed, attackSpeed, attackPower, thrustPower;
    public bool isInmune;
    public AudioClip deathClip, damageClip;
    public LayerMask playerLabel;
    public float attackRange;
    public GameObject graviton;

    private Rigidbody2D rb;
    private Player player;
    private AudioSource playerAudio;
    private Animator anim;

    /* AI RELATED VARIABLES */
    public Transform target;
    public Vector2 originalTransform;
    public float nextWayPointDistance = 6f;
    Path path;
    int currentWayPoint = 0;
    bool reachedEnd;
    bool reachedEndOfAction = false;
    Seeker seeker;
    bool attacking = false;
    bool physical = true;
    bool range = false;
    public bool dead = false;
    int timesDead = 0;
    float timePassed;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Just in case we need it
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerAudio = player.GetComponent<AudioSource>();
        seeker = GetComponent<Seeker>();
        target = player.transform;
        originalTransform = this.transform.position;
        reachedEnd = false;
        timePassed = 0;
        anim = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .1f);
    }

    void UpdatePath()
    {
        if (dead) return;
        float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);
        //Debug.Log("We found player");
        Debug.Log("calling");
        seeker.StartPath(rb.position, (Vector2) target.position, OnPathComplete);
        if(!attacking && range)
        {
            timePassed += Time.fixedDeltaTime;
        }
        if (distanceToPlayer <= 1f && !attacking && physical)
        {
            StartCoroutine("attackPlayer");
            attacking = true;
        }else if(timePassed >= .5f && !attacking && range)
        {
            timePassed = 0;
            StartCoroutine("attackPlayer");
            attacking = true;
        }
    }

    private void UpdateBackPath()
    {
        seeker.StartPath(rb.position, originalTransform, OnPathComplete);
    }


    void OnPathComplete(Path p)
    {
        Debug.Log("entramos en PathComplete");
        //Comprobamos que no haya ningun error y si es así nuestro camino nuevo es el calculado
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
        {
            Debug.Log("error en el path");
            return;
        }

        //Comprobamos que no haya mas waypoints. Si los hay paramos el movimiento
        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else
        {
            reachedEnd = false;
        }
        Vector2 direction = new Vector2(0,0);
        if (range)
        {
            direction = -((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        }
        else
        {
            direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        }
        
        Vector2 force = direction * movementSpeed * Time.deltaTime;
        Debug.Log("Estamos aplicando fuerza");
        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if (distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }

    public IEnumerator attackPlayer()
    {
        if (physical)
        {
            Debug.Log("Physical Attack");
            float originalSpeed = movementSpeed;
            movementSpeed = 0;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            anim.SetTrigger("attack");
            yield return new WaitForSeconds(.8f);
            Collider2D[] objectives = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLabel);
            for (int i = 0; i < objectives.Length; i++)
            {
                objectives[i].GetComponent<Player>().setLife(-attackPower);
            }
            yield return new WaitForSeconds(.2f);
            rb.isKinematic = false;
            movementSpeed = originalSpeed;
            attacking = false;
            physical = false;
            range = true;
        }else if (range)
        {
            Debug.Log("Range Attack");
            float originalSpeed = movementSpeed;
            movementSpeed = 0;
            rb.velocity = Vector3.zero;
            Vector2 direction = ((Vector2) rb.position - (Vector2) player.transform.position).normalized;
            GameObject bounceGraviton = Instantiate(graviton, (Vector2) transform.position - direction, Quaternion.identity);
            Destroy(bounceGraviton, 8);
            Vector2 force = -direction * Time.deltaTime * 50;
            bounceGraviton.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
            yield return new WaitForSeconds(3f);
            bounceGraviton.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            attacking = false;
            physical = true;
            range = false;
            movementSpeed = originalSpeed;
        }
       
    }

    public void drainLife(float cuantity)
    {
        if(!physical) anim.SetTrigger("flinch");
        enemyLife -= cuantity;
        if(enemyLife <= 0 && !dead)
        {
            timesDead += 1;
            Debug.Log(timesDead);
            if(timesDead == 2)
            {
                anim.SetTrigger("death");
                Destroy(gameObject, 1);
                //Llamamos al evento de los creditos.
                Debug.Log("muerte definitiva");
            }
            else
            {
                dead = true;
                enemyLife = 100;
                //Llamamos a evento de muerte Arcaelum.
                Debug.Log("Llamamos a Awaken");
                movementSpeed = 0;
                rb.velocity = Vector3.zero;
                FindObjectOfType<EventManager>().awakenEvent();
            }
        }
    }

}
