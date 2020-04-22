using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //Clase enemigo. Primera implementación Dummy.
    public float enemyLife, normalDeffense, movementSpeed, attackSpeed, attackPower, thrustPower;
    public bool isInmune;
    public GameObject essence, deathParticle;
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
    bool reachedEnd;
    bool reachedEndOfAction = false;
    Seeker seeker;
    bool attacking = false;


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
        InvokeRepeating("UpdatePath", 0f, .1f);
    }

    void UpdatePath()
    {
        float distanceToPlayer = Vector2.Distance(rb.position, player.transform.position);
        float distanceToOriginalPosition = Vector2.Distance(rb.position, originalTransform);
        //Debug.Log("Distancia a la posicion original: " + distanceToOriginalPosition);

        //Logica -> Si la distancia al jugador es una determinada usa A* para perseguirlo.
        // Si te pasas de tu rango de accion vuelves a tu zona y aunque el jugador este a tu lado pasas de el.

        if(distanceToOriginalPosition < 5f && !reachedEndOfAction)
        {
            //Comprobamos distancia al jugador y si seeker ha acabado.
            if(distanceToPlayer <= 2.5f && seeker.IsDone())
            {
                //Debug.Log("We found player");
                seeker.StartPath(rb.position, target.position, OnPathComplete);
                if(distanceToPlayer <= 1 && !attacking)
                {
                    StartCoroutine("attackPlayer");
                    attacking = true;
                }
            }
        }
        else if(distanceToOriginalPosition >= 5f)
        {
            //Debug.Log("Reached end");
            reachedEndOfAction = true;
        }

        if (reachedEndOfAction && seeker.IsDone())
        {
            //Debug.Log("Back To Home");
            seeker.StartPath(rb.position, originalTransform, OnPathComplete);
            if (distanceToOriginalPosition <= .5f)
            {
                reachedEndOfAction = false;
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


    public IEnumerator attackPlayer()
    {
        //Hay que telegrafiar el salto. Tendra que vibrar o dar a entender que va a atacar al jugador.
        //Cuando este telegrafiando no podrá moverse y dará tiempo al jugador de moverse (La direccion debe cogerse antes de la espera)
        Debug.Log("Atacamos");
        float originalSpeed = movementSpeed;
        movementSpeed = 0;
        //Cogemos el vector direccion con el jugador
        Vector2 attackDirection = ((Vector2)player.transform.position - rb.position).normalized;
        Vector2 attackForce = attackDirection * thrustPower;
        yield return new WaitForSeconds(1f);
        movementSpeed = originalSpeed;
        rb.AddForce(attackForce,ForceMode2D.Impulse);
        yield return new WaitForSeconds(2f);
        attacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.ToString());
        if(collision.gameObject.tag == "Player" && attacking)
        {
            player.setLife(-attackPower);
        }
    }


    public void takeDamage(float amount, string zoneLoc, float thrust)
    {
        playerAudio.PlayOneShot(damageClip);
        enemyLife -= (amount - normalDeffense);
        Debug.Log(player.transform.forward);
        impulse(zoneLoc, thrust);
        if (enemyLife <= 0 || (!isInmune && player.getDisuade()))
        {
            Debug.Log("Entramos aqui");
            StartCoroutine("waitSecondsBeforeDeath", .5f);
        }
    }

    private IEnumerator waitSecondsBeforeDeath(float time)
    {
        yield return new WaitForSeconds(time);
        deathLogic();
    }

    private void impulse(string zoneLoc, float thrust)
    {
        if (zoneLoc.Equals("bottom"))
        {
            rb.AddForce(this.transform.up * -1 * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("up"))
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
    }

    private void deathLogic()
    {
        //Efecto de partículas
        Debug.Log("Entro");
        GameObject instanceParticle = Instantiate(deathParticle);
        instanceParticle.transform.position = this.transform.position;
        Object.Destroy(instanceParticle, .4f);
        //Efecto de sonido
        playerAudio.PlayOneShot(deathClip);
        //Drop
        GameObject instance = Instantiate(essence);
        instance.transform.position = this.transform.position;
        Destroy(this.gameObject);
    }
}
