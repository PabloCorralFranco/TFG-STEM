using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public int blue, red, green;
    public AudioClip dropClip;
    private AudioSource playerSource;
    private void Start()
    {
        playerSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerSource.PlayOneShot(dropClip);
            Inventory playerInv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            playerInv.addEssences(blue, red, green);
            Destroy(this.gameObject);
        }
    }
}
