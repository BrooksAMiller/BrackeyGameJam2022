using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manhole : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(true);
            PlayerController.pc.levelSwitchOptional = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            PlayerController.pc.levelSwitchOptional = false;
        }
    }
}
