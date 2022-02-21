using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public void CloseDialogue()
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
}
