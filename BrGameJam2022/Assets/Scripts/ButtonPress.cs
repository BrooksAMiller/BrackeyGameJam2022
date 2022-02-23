using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonPress : MonoBehaviour
{
    Button butt;
    GameController gc;
    public string triggerString;

    private void Start()
    {
        gc = GameController.gc;
        butt = gameObject.GetComponent<Button>();
        butt.onClick.AddListener(delegate { ButtonPressed(triggerString); });
    }

    public void ButtonPressed(string trigger)
    {
        switch (trigger)
        {
            case "next":
                gc.GetComponent<Animator>().SetTrigger("next");
                break;
            case "back":
                gc.GetComponent<Animator>().SetTrigger("back");
                break;
            case "option1":
                gc.GetComponent<Animator>().SetTrigger("option1");
                break;
            case "option2":
                gc.GetComponent<Animator>().SetTrigger("option2");
                break;
            case "option3":
                gc.GetComponent<Animator>().SetTrigger("option3");
                break;
        }
    }
}
