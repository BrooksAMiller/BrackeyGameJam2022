using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    public GameObject dialogueObject;

    public GameObject dialogueButtonPrefab;

    public GameObject flashBangPanel;

    public GameObject pauseMenu;

    void Awake()
    {
        gc = this;
    }

    public void Invoker(string func)
    {
        Invoke(func, 0.1f);
    }

    public void ActivateFlashbang()
    {
        flashBangPanel.SetActive(true);
    }
}
