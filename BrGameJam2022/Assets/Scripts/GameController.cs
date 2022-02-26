using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    public GameObject dialogueObject;

    public GameObject dialogueButtonPrefab;

    public GameObject flashBangPanel;

    public GameObject pauseMenu;

    public Slider mainSlider;

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        if (mainSlider.value > 0.5)
            mainSlider.value = 0.5f;
        GetComponent<AudioSource>().volume = mainSlider.value;
        Debug.Log(mainSlider.value);
    }

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
