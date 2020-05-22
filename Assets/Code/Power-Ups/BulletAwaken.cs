using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAwaken : MonoBehaviour
{
    public LayerMask arcLayer;
    private Player player;
    private Rigidbody2D myRb;

    private void Start()
    {
        Destroy(gameObject, 5);
    }
    public void triggerBullet()
    {
        myRb = GetComponent<Rigidbody2D>();
        string zoneLoc = FindObjectOfType<Player>().getZoneLoc();
        switch (zoneLoc)
        {
            case "bottom":
                myRb.velocity = Vector3.zero;
                myRb.AddForce(Vector2.down * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "up":
                myRb.velocity = Vector3.zero;
                myRb.AddForce(Vector2.up * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "right":
                myRb.velocity = Vector3.zero;
                myRb.AddForce(Vector2.right * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            case "left":
                myRb.velocity = Vector3.zero;
                myRb.AddForce(Vector2.left * Time.deltaTime * 50, ForceMode2D.Impulse);
                break;
            default:
                Debug.Log("error");
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Arcaelum arc = FindObjectOfType<Arcaelum>();
        if(arc != null && collision.name.Equals("Arcaelum(Clone)"))
        {
            Debug.Log("Chocamos");
            arc.drainLife(50);
            Destroy(gameObject);
        }
        
    }
}
