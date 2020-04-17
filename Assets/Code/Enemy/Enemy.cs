﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Clase enemigo. Primera implementación Dummy.
    public float enemyLife, normalDeffense, movementSpeed, attackSpeed;
    public bool isInmune;

    private Rigidbody2D rb;
    private Player player;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Just in case we need it
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void takeDamage(float amount, string zoneLoc, float thrust)
    {
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
        if (enemyLife <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}