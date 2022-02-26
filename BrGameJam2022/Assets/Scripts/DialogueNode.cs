using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueNode : StateMachineBehaviour
{
    public enum Actions { next, back, option1, option2, option3 }
    [TextArea]
    public string text;

    private GameObject canvas;
    private GameObject textPanel;

    private GameController game;

    [System.Serializable]
    public struct Options
    {
        public string option;
        public Actions action;
    }

    public Options[] options;
    [Header("Extras")]
    public Sprite changeBG;

    public string extraMethod;

    private List<GameObject> activeOptions = new List<GameObject>();

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            
        foreach (Transform child in GameController.gc.dialogueObject.transform.GetChild(2).transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        textPanel = GameController.gc.dialogueObject.transform.GetChild(1).gameObject;
        Debug.Log("adding: " + text);
        textPanel.GetComponent<TextMeshProUGUI>().text = text;
        if(options.Length > 0)
        {
            for (int i = 0; i < options.Length; i++)
            {
                GameObject newButton = Instantiate(GameController.gc.dialogueButtonPrefab);
                newButton.transform.SetParent(GameController.gc.dialogueObject.transform.GetChild(2).transform, false);
                newButton.GetComponent<ButtonPress>().triggerString = options[i].action.ToString();
                newButton.GetComponent<TextMeshProUGUI>().text = "> " + options[i].option;
                activeOptions.Add(newButton);
            }
        }
        if(extraMethod != "")
        {
            animator.SendMessage(extraMethod, this);
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        //base.OnStateExit(animator, animatorStateInfo, layerIndex);
        for (int i = 0; i < activeOptions.Count; i++)
        {
            Destroy(activeOptions[i]);
        }
    }

}
