using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!PlayerController.pc.hasDiscoveredAlien && gameObject.tag == "AlienTruck")
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(true);
                PlayerController.pc.canInteractWithNPC = true;
                PlayerController.pc.interactableNPC = this.gameObject;
            }

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!PlayerController.pc.hasDiscoveredAlien && gameObject.tag == "AlienTruck")
            {
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                GetComponent<Animator>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                PlayerController.pc.canInteractWithNPC = false;
                PlayerController.pc.interactableNPC = null;
                if (GameController.gc.dialogueObject.activeSelf)
                    GameController.gc.dialogueObject.SetActive(false);
            }
        }
    }

    public void ActivateFlashbang()
    {
        GameController.gc.flashBangPanel.SetActive(true);
    }

    public void HasDiscoveredAlien()
    {
        PlayerController.pc.hasDiscoveredAlien = true;
    }
}