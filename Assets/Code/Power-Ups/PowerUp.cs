using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerUpName;
    public float time; //Also for cuantity
    private AudioSource playerAudio;
    public AudioClip powerUpClip;
    public GameObject fullParticles, abilityActive, batteryFullParticles, healSth, dashParticles, magicCircle, stopTimeEffect, bulletAwakenEffect;

    private Player player;
    private PowerUpManager manager;
    private bool state;
    private Rigidbody2D rb;
    private int nDash;
    private bool onCoolDown = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        manager = GameObject.FindGameObjectWithTag("AbilitieManager").GetComponent<PowerUpManager>();
        rb = player.GetComponent<Rigidbody2D>();
        nDash = 0;
        playerAudio = player.GetComponent<AudioSource>();
    }

    public void cast()
    {
        /*
        state = manager.getActive();
        if (!state)
        {
            manager.setActive(true);
            StartCoroutine(powerUpName, time);
        }
        */
        StartCoroutine(powerUpName, time);

    }

    private void abilitieActivation1()
    {
        Instantiate(abilityActive,transform.position,Quaternion.identity, this.transform);
    }

    private void abilitieActivationHeal()
    {
        Debug.Log("HEAL");
        GameObject heal = Instantiate(healSth, transform.position, Quaternion.identity,this.transform);
        Destroy(heal, .5f);
    }

    private void dashInstance()
    {
        string zoneLoc = player.getZoneLoc();
        GameObject lightning, magicBarrier;
        Vector2 particlesPosition = player.transform.position;
        if (zoneLoc.Equals("left"))
        {
            particlesPosition = (Vector2)player.transform.position + new Vector2(.3f, 0);
        } else if (zoneLoc.Equals("right"))
        {
            particlesPosition = (Vector2)player.transform.position + new Vector2(-.3f, 0);
        } else if (zoneLoc.Equals("up"))
        {
            particlesPosition = (Vector2)player.transform.position + new Vector2(0, -.3f);
        } else if (zoneLoc.Equals("bottom"))
        {
            particlesPosition = (Vector2)player.transform.position + new Vector2(0f, .3f);
        }
        //arreglar rotacion en top y bottom
        lightning = Instantiate(dashParticles, particlesPosition, Quaternion.identity, player.transform);
        Destroy(lightning, .3f);
        magicBarrier = Instantiate(magicCircle, player.transform.position, Quaternion.identity, player.transform);
        Destroy(magicBarrier, .3f);
    }

    private IEnumerator boi(float boostTime)
    {
        abilitieActivation1();
        Debug.Log("Boi");
        playPowerUpAudio();
        player.incSpeed(1f);
        yield return new WaitForSeconds(boostTime);
        player.resetSpeed();
        endProcess();
    }

    private IEnumerator lifefull(float cuantity)
    {
        abilitieActivationHeal();
        GameObject shieldInstance = Instantiate(batteryFullParticles,player.transform.position,Quaternion.identity,player.transform);
        Destroy(shieldInstance, .5f);
        playPowerUpAudio();
        player.setLife(cuantity);
        yield return new WaitForSeconds(.5f);
        endProcess();
    }
    private IEnumerator lifeEmpty(float cuantity)
    {
        abilitieActivationHeal();
        playPowerUpAudio();
        player.setLife(-cuantity);
        yield return new WaitForSeconds(.5f);
        endProcess();
    }
    private IEnumerator awakenDash()
    {
        playPowerUpAudio();
        player.setDashing(true);
        dashInstance();
        Handheld.Vibrate();
        impulse();
        yield return new WaitForSeconds(.5f);
        player.setDashing(false);
    }
    private IEnumerator stopTime()
    {
        if (onCoolDown)
        {
            StopAllCoroutines();
        }
        else
        {
            //Caso especial - solo afecta a Arcaelum y a su animator.
            onCoolDown = true;
            Destroy(Instantiate(stopTimeEffect, player.transform.position, Quaternion.identity, player.transform), 1);
            Arcaelum arc = FindObjectOfType<Arcaelum>();
            Animator arcAnim = arc.GetComponent<Animator>();
            arc.movementSpeed = arc.movementSpeed / 2;
            arcAnim.speed = arcAnim.speed / 2;
            yield return new WaitForSeconds(5);
            arc.movementSpeed = arc.movementSpeed * 2;
            arcAnim.speed = arcAnim.speed * 2;
            yield return new WaitForSeconds(15);
            onCoolDown = false;
        }
        
    }

    private IEnumerator bulletAwaken()
    {
        if (!onCoolDown)
        {
            onCoolDown = true;
            GameObject bullet = Instantiate(bulletAwakenEffect, player.attackZone.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletAwaken>().triggerBullet();
            yield return new WaitForSeconds(1);
            onCoolDown = false;
        }
    }



    private IEnumerator kordDash(float time)
    {
        if(nDash < 3)
        {
            playPowerUpAudio();
            player.setDashing(true);
            dashInstance();
            Handheld.Vibrate();
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
        float thrust = player.getThrust() + 12.5f;
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

    private void playPowerUpAudio()
    {
        playerAudio.PlayOneShot(powerUpClip);
    }

    private IEnumerator kordDisuade(float time)
    {
        abilitieActivation1();
        playPowerUpAudio();
        player.setDisuade(true);
        yield return new WaitForSeconds(time);
        player.setDisuade(false);
        endProcess();
    }

    private IEnumerator kordFullDamage(float cuantity)
    {
        abilitieActivation1();
        //Ponemos sprite de particulas
        GameObject particleInstance = Instantiate(fullParticles, player.transform.position, Quaternion.identity, player.transform);
        Destroy(particleInstance, 1f);
        playPowerUpAudio();
        player.setAttack(cuantity);
        yield return new WaitForSeconds(15f);
        player.resetAttack();
        player.setAttack(-(player.getAttack()/2));
        endProcess();
    }

    private IEnumerator kordRevive(float cuantity)
    {
        abilitieActivationHeal();
        playPowerUpAudio();
        player.resetAttack();
        yield return new WaitForSeconds(.5f);
        endProcess();
    }

    private IEnumerator reviveFromDeath(float cuantity)
    {
        //Quizas debamos crear campos constantes de vida maxima?
        //El manager de partida se hará cargo de que al morir o revivir salga lo que haga falta.
        abilitieActivationHeal();
        playPowerUpAudio();
        player.setLife(cuantity);
        yield return new WaitForSeconds(.5f);
        endProcess();
    }

    private IEnumerator dashFullLife(float cuantity)
    {
        //Damos un margen de error en la vida
        if(player.getLife() >= cuantity)
        {
            abilitieActivationHeal();
            playPowerUpAudio();
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
        playPowerUpAudio();
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
