using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController.pc.playerHealth++;
            Instantiate(PlayerController.pc.playerHealthIcon, PlayerController.pc.playerHealthBar.transform);
            Destroy(gameObject);
        }
    }

}
