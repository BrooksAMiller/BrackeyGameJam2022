using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scare : MonoBehaviour
{
    public bool hasActivated = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasActivated)
        {
            GetComponent<Animator>().SetTrigger("activate");
            hasActivated = true;
        }
    }
}
