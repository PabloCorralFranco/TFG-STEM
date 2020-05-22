using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public Joystick joystick;
    public GameObject attackZone;
    public float attackPower;
    public bool disuadePower;
    public float playerLife;
    public float range;
    public float thrust;
    public bool canAttack;
    public LayerMask enemyLabel, cableLayer, gravitonLayer, arcLayer;
    public AudioSource mySource;
    public AudioClip attackClip;
    public GameObject attackParticles;
    public BatteryUpdate batteryCount;

    private float movSpeed;
    private Vector2 coordinates;
    private string zoneLoc;
    private bool isDashing;
    private bool wannaTalk = false;
    private NPC npcToTalk;
    private bool cantMove = false;
    private static Player playerInstance;

    public float originalSpeed, originalAttack;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //Inicializamos variables referentes al movimiento y ataque
        movSpeed = originalSpeed;
        attackPower = originalAttack;
        isDashing = false;
        cantMove = false;
        wannaTalk = false;
        canAttack = false;
        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (cantMove)
        {
            anim.SetFloat("Speed", 0);
            return;
        }
        //Input
        coordinates.x = joystick.Horizontal;
        coordinates.y = joystick.Vertical;
        //Normalizamos para evitar el doble de velocidad en diagonal.
        coordinates = coordinates.normalized;
        anim.SetFloat("Horizontal", coordinates.x);
        anim.SetFloat("Vertical", coordinates.y);
        anim.SetFloat("Speed", coordinates.sqrMagnitude);
        //Debug.Log("Coordenadas en X: " + coordinates.x + " ,Coordenadas en Y: " + coordinates.y);
        if(coordinates.x != 0)
        {
            anim.SetFloat("IdleState", coordinates.x);
            anim.SetFloat("IdleStateY", coordinates.y);
        }
        if (coordinates.y != 0)
        {
            anim.SetFloat("IdleStateY", coordinates.y);
            anim.SetFloat("IdleState", coordinates.x);
        }
        //Lógica para posicionar la caja de daño
        if(coordinates.x > 0.5f)
        {
            attackZone.transform.position = transform.position + new Vector3(0.218f, 0, 0);
            zoneLoc = "right";
        }else if (coordinates.x < -0.5f)
        {
            attackZone.transform.position = transform.position - new Vector3(0.218f, 0, 0);
            zoneLoc = "left";
        }else if (coordinates.y > 0.5f)
        {
            attackZone.transform.position = transform.position + new Vector3(0f, 0.218f, 0);
            zoneLoc = "up";
        }
        else if (coordinates.y < -0.5f)
        {
            attackZone.transform.position = transform.position - new Vector3(0, 0.218f, 0);
            zoneLoc = "bottom";
        }

    }

    //Para trabajar con físicas es mejor usar un temporizador fijo
    private void FixedUpdate()
    {
        //Movimiento
        if (!isDashing && !cantMove)
        {
            rb.MovePosition(rb.position + coordinates * movSpeed * Time.fixedDeltaTime);
        }
    }

    public void stopFromMoving()
    {
        rb.velocity = Vector2.zero;
        cantMove = true;
    }
    public void continueMoving()
    {
        cantMove = false;
    }

    private void instanceFromOrientation(GameObject toInstance, float time)
    {

        GameObject instanceParticle = Instantiate(toInstance);
        instanceParticle.transform.position = this.transform.position;
        
        if (zoneLoc.Equals("up"))
        {
            instanceParticle.transform.localScale = new Vector2(-instanceParticle.transform.localScale.x, -instanceParticle.transform.localScale.y);
        }
        else if (zoneLoc.Equals("right"))
        {
            instanceParticle.transform.localScale = new Vector2(-instanceParticle.transform.localScale.x, instanceParticle.transform.localScale.y);
        }

        Destroy(instanceParticle, time);
    }

    //CONVERSACION
    public void setToTalk(bool talkState, NPC npc)
    {
        wannaTalk = talkState;
        npcToTalk = npc;
    }


    //ATAQUE
    public void Attack()
    {
        
        Debug.Log(wannaTalk);
        if (wannaTalk)
        {
            wannaTalk = false;
            npcToTalk.awakeConversationMethods();
            return;
        }
        if (!canAttack) return;

        if (!mySource.isPlaying)
        {
            mySource.PlayOneShot(attackClip);
        }
        anim.SetTrigger("attack");
        //Activate particle
        instanceFromOrientation(attackParticles, .2f);

        Collider2D[] enemigos = Physics2D.OverlapCircleAll(attackZone.transform.position, range, enemyLabel);
        for(int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].GetComponent<Enemy>().takeDamage(this.getAttack(),zoneLoc, thrust);
        }

        //Para el puzzle de electricidad y Prim
        Collider2D[] cables = Physics2D.OverlapCircleAll(attackZone.transform.position, range, cableLayer);
        PrimManager pm = FindObjectOfType<PrimManager>();
        if (pm != null && cables.Length != 0)
        {
            pm.setCableToActive(cables[0].gameObject);
            pm.tryAristaActivation(cables[0].GetComponent<Cable>().arista);
        }
        //Para el graviton de Arcaelum
        Collider2D[] graviton = Physics2D.OverlapCircleAll(attackZone.transform.position, range, gravitonLayer);
        for(int i = 0; i < graviton.Length; i++)
        {
            Debug.Log("Deflecting");
            graviton[i].GetComponent<GravitonArcaelum>().deflect();
        }
        //Para el daño a Arcaelum
        Collider2D[] arc = Physics2D.OverlapCircleAll(attackZone.transform.position, range, arcLayer);
        for (int i = 0; i < arc.Length; i++)
        {
            arc[i].GetComponent<Arcaelum>().drainLife(attackPower);
        }

    }
    public float getAttack()
    {
        return attackPower;
    }
    public void setAttack(float buff)
    {
        attackPower += buff;
    }
    public void resetAttack()
    {
        attackPower = originalAttack;
    }
    public float getThrust()
    {
        return thrust;
    }
    public bool getDisuade()
    {
        return disuadePower;
    }
    public void setDisuade(bool state)
    {
        disuadePower = state;
    }


    //VIDA
    public float getLife()
    {
        return playerLife;
    }

    public void setLife(float diff)
    {
        playerLife += diff;
        if(playerLife > 100)
        {
            playerLife = 100;
        }
        //Comprobar muerte
        if(playerLife <= 0)
        {
            Debug.Log("Death");
            playerLife = 100;
            StartCoroutine(FindObjectOfType<EventManager>().transitionToNewLevel(SceneManager.GetActiveScene().name)); 
            return;
        }
        batteryCount.changeBattery(playerLife);
    }

    //VELOCIDAD
    public float getSpeed()
    {
        return movSpeed;
    }
    public void incSpeed(float nSpeed)
    {
        movSpeed += nSpeed;
    }
    public void resetSpeed()
    {
        movSpeed = originalSpeed;
    }
 

    //ZONE LOCATION AND BUFF RELATED
    public string getZoneLoc()
    {
        return zoneLoc;
    }
    public void setDashing(bool state)
    {
        isDashing = state;
    }

}
