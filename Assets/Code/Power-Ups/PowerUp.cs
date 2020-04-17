using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpName;
    public float time; //Also for cuantity
    private Player player;
    private PowerUpManager manager;
    private bool state;
    private Rigidbody2D rb;
    private int nDash;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        manager = GameObject.FindGameObjectWithTag("AbilitieManager").GetComponent<PowerUpManager>();
        rb = player.GetComponent<Rigidbody2D>();
        nDash = 0;
    }

    public void cast()
    {
        state = manager.getActive();
        if (!state)
        {
            manager.setActive(true);
            StartCoroutine(powerUpName, time);
        }
        
    }

    private IEnumerator boi(float boostTime)
    {
        Debug.Log("Boi");
        player.incSpeed(1f);
        yield return new WaitForSeconds(boostTime);
        player.resetSpeed();
        endProcess();
    }

    private IEnumerator lifefull(float cuantity)
    {
        //No hay espera de ningún tipo
        Debug.Log("lifefull");
        Debug.Log("old life: " + player.getLife());
        player.setLife(cuantity);
        Debug.Log("new life: " + player.getLife());
        endProcess();
        yield return null;
    }
    private IEnumerator lifeEmpty(float cuantity)
    {
        player.setLife(-cuantity);
        endProcess();
        yield return null;
    }

    private IEnumerator kordDash(float time)
    {
        if(nDash < 3)
        {
            player.setDashing(true);
            impulse();
            nDash++;
            manager.setActive(false);
        }
        if(nDash == 3)
        {
            nDash = 0;
            yield return new WaitForSeconds(.5f);
            player.setDashing(false);
            endProcess();
        }
        yield return new WaitForSeconds(.5f);
        player.setDashing(false);
    }

    private void impulse()
    {
        string zoneLoc = player.getZoneLoc();
        //Aplicamos una determinada fuerza que guardaremos en time.
        //Podrá dashear tres veces
        float thrust = player.getThrust();
        Debug.Log("entramos + nDash: " + zoneLoc);
        
        if (zoneLoc.Equals("bottom"))
        {
            //Debug.Log("Wenas");
            player.GetComponent<Rigidbody2D>().AddForce(player.transform.up * -1 * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("up"))
        {
            rb.AddForce(player.transform.up * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("right"))
        {
            rb.AddForce(player.transform.right * thrust, ForceMode2D.Impulse);
        }
        else if (zoneLoc.Equals("left"))
        {
            rb.AddForce(player.transform.right * -1 * thrust, ForceMode2D.Impulse);
        }
        
    }

    private IEnumerator kordDisuade(float time)
    {
        player.setDisuade(true);
        yield return new WaitForSeconds(time);
        player.setDisuade(false);
        endProcess();
    }

    private IEnumerator kordFullDamage(float cuantity)
    {
        player.setAttack(cuantity);
        yield return new WaitForSeconds(15f);
        player.resetAttack();
        player.setAttack(-(player.getAttack()/2));
        endProcess();
    }

    private IEnumerator kordRevive(float cuantity)
    {
        player.resetAttack();
        endProcess();
        yield return null;
    }

    private IEnumerator reviveFromDeath(float cuantity)
    {
        //Quizas debamos crear campos constantes de vida maxima?
        //El manager de partida se hará cargo de que al morir o revivir salga lo que haga falta.
        player.setLife(cuantity);
        endProcess();
        yield return null;
    }

    private IEnumerator dashFullLife(float cuantity)
    {
        //Damos un margen de error en la vida
        if(player.getLife() >= cuantity)
        {
            player.setDashing(true);
            impulse();
            yield return new WaitForSeconds(.5f);
            player.setDashing(false);
        }
        endProcess();
        
    }

    private IEnumerator disuadeFullLife(float cuantity)
    {
        yield return null;
    }

    private IEnumerator kordFullSpeed(float cuantity)
    {
        player.anim.speed = 3f;
        yield return new WaitForSeconds(cuantity);
        player.anim.speed = 1f;
        endProcess();
    }





    private void endProcess()
    {
        manager.setActive(false);
        Destroy(this.gameObject);
    }
}
