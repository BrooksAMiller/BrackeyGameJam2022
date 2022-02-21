using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gc;

    public GameObject dialogueObject;

    void Awake()
    {
        gc = this;
    }


}
