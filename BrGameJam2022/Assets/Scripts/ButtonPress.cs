using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPress : MonoBehaviour
{
    Button butt;
    GameController gc;
    PlayerController pc;

    public string triggerString;

    private void Start()
    {
        pc = PlayerController.pc;
        gc = GameController.gc;
        butt = gameObject.GetComponent<Button>();
        butt.onClick.AddListener(delegate { ButtonPressed(triggerString); });
    }

    public void ButtonPressed(string trigger)
    {
        switch (trigger)
        {
            case "next":
                pc.interactableNPC.GetComponent<Animator>().SetTrigger("next");
                break;
            case "back":
                pc.interactableNPC.GetComponent<Animator>().SetTrigger("back");
                break;
            case "option1":
                pc.interactableNPC.GetComponent<Animator>().SetTrigger("option1");
                break;
            case "option2":
                pc.interactableNPC.GetComponent<Animator>().SetTrigger("option2");
                break;
            case "option3":
                pc.interactableNPC.GetComponent<Animator>().SetTrigger("option3");
                break;
        }
    }
}
