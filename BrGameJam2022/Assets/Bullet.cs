using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob")
        {
            PlayerController.pc.gameObject.GetComponent<AudioSource>().PlayOneShot(PlayerController.pc.pigeonDeathSound);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
