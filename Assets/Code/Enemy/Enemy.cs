using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //Clase enemigo. Primera implementación Dummy.
    public float enemyLife, normalDeffense, movementSpeed, attackSpeed;
    public bool isInmune;
    public GameObject essence;
    public AudioClip deathClip, damageClip;

    private Rigidbody2D rb;
    private Player player;
    private AudioSource playerAudio;

    /* AI RELATED VARIABLES */
    public Transform target;
    public Vector2 originalTransform;
    public float nextWayPointDistance = 6f;
    Path path;
    int currentWayPoint = 0;
    bool reachedEnd = false;
    Seeker seeker;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Just in case we need it
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerAudio = player.GetComponent<AudioSource>();
        seeker = GetComponent<Seeker>();
        target = player.transform;
        originalTransform = this.transform.position;
        InvokeRepeating("UpdatePath", 0f, .1f);
    }

    void UpdatePath()
    {
        float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);
        float distanceToOriginalPosition = Vector2.Distance(rb.position, originalTransform);
        Debug.Log("Distancia a la posicion original: " + distanceToOriginalPosition);

        //Logica -> Si la distancia al jugador es una determinada usa A* para perseguirlo.
        // Si te pasas de tu rango de accion vuelves a tu zona y aunque el jugador este a tu lado pasas de el.

        if (distanceToPlayer <= 2.5f && distanceToOriginalPosition <= 3.5f && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            if(distanceToOriginalPosition > 0.1f)
            {
                seeker.StartPath(rb.position, originalTransform, OnPathComplete);
            }
            
        }
    }

    private void UpdateBackPath()
    {
        seeker.StartPath(rb.position, originalTransform, OnPathComplete);
    }


    void OnPathComplete(Path p)
    {
        //Comprobamos que no haya ningun error y si es así nuestro camino nuevo es el calculado
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }



    }

    private void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        //Comprobamos que no haya mas waypoints. Si los hay paramos el movimiento
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEnd = true;
            return;
        }
        else
        {
            reachedEnd = false;
        }

        Vector2 direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime;
        rb.AddForce(force);
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);
        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }
    }

    public void takeDamage(float amount, string zoneLoc, float thrust)
    {
        playerAudio.PlayOneShot(damageClip);
        enemyLife -= (amount - normalDeffense);
        Debug.Log(player.transform.forward);
        if (zoneLoc.Equals("bottom"))
        {
            rb.AddForce(this.transform.up * -1 * thrust, ForceMode2D.Impulse);
        }else if (zoneLoc.Equals("up"))
        {
            rb.AddForce(this.transform.up * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("right"))
        {
            rb.AddForce(this.transform.right * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("left"))
        {
            rb.AddForce(this.transform.right * -1 * thrust, ForceMode2D.Impulse);
        }
        if (enemyLife <= 0 || (!isInmune && player.getDisuade()))
        {
            deathLogic();
        }
    }

    private void deathLogic()
    {
        //Efecto de partículas

        //Efecto de sonido
        playerAudio.PlayOneShot(deathClip);
        //Drop
        GameObject instance = Instantiate(essence);
        instance.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
