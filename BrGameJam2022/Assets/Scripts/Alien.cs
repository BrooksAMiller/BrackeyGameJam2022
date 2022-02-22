using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [NonSerialized]
    public GameObject Player;
    private MoveTo move;
    public GameObject CurrentTarget;

    public enum States { Pizza, Run, Caught, CarriedByPlayer }
    public States state;

    [Range(1, 100)]
    public int GettingCaught = 1;
    public float TickTime_GettingCaught = 0.25f;
    public float LastTick_Timer_Increase;
    public float LastTick_Timer_Decrease;
    bool Player_inside_Proxy = false;

    //GettinCaught Meter mockup
    public GameObject Mask;

    public OnTrigger CloseProxy;
    public OnTrigger FarProxy;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<MoveTo>();

        StartCoroutine(ExecuteActionAfterDelay(() =>
        {
            move.SetDestination(CurrentTarget.transform.position);
            move.method = MoveTo.Method.Speed;
            state = States.Pizza;
            move.On_FinalDestinationReach += On_Pizza_Reach;

        }, 1f));

        state = States.Pizza;

        CloseProxy.AddEvent("Enter", "Player", On_Alien_CloseProxyEnter);
        CloseProxy.AddEvent("Stay", "Player", On_Alien_CloseProxyStay);
        CloseProxy.AddEvent("Exit", "Player", On_Alien_CloseProxyExit);

    }

    // Update is called once per frame
    void Update()
    {
        Mask.transform.localPosition = new Vector3((GettingCaught / 100f) * -6f, 0f, 0f);
    }
    public void FixedUpdate()
    {
        Flip();

        switch (state)
        {
            case States.Pizza:
                break;
            case States.Run:
                if(!Player_inside_Proxy) if (LastTick_Timer_Decrease + TickTime_GettingCaught < Time.time && GettingCaught > 1)
                {
                    GettingCaught--;
                    LastTick_Timer_Decrease = Time.time;
                }
                if(CurrentTarget == null)
                {
                    var farTarget = GameManagerMockup.instance.FarthestNPC(transform.position);
                    var closeTarget = GameManagerMockup.instance.ClosestNPC(transform.position);
                    var newTarget = new GameObject("Current_Alien_Target");
                    newTarget.transform.position = (farTarget.transform.position + closeTarget.transform.position) / 2;
                    newTarget.transform.localScale /= 4;
                    newTarget.AddComponent<SpriteRenderer>().sprite = CloseProxy.GetComponent<SpriteRenderer>().sprite;
                    newTarget.GetComponent<SpriteRenderer>().color = Color.blue;

                    if (newTarget == null) Debug.LogError("jeez how dafq did this happen????");

                    CurrentTarget = newTarget;
                    move.SetDestination(CurrentTarget.transform.position);
                    move.On_FinalDestinationReach += On_Target_Reach;
                }
                break;
            case States.Caught:
                move.enabled = false;
                break;
            case States.CarriedByPlayer:
                break;
        }
    }
    public void Flip()
    {
        var Delta = move.Destination - transform.position;

        if (Delta.x > 0)
        {
            Vector3 Scale = transform.localEulerAngles;
            Scale.y = 180;
            transform.localEulerAngles = Scale;
        }
        else
        {
            Vector3 Scale = transform.localEulerAngles;
            Scale.y = 0;
            transform.localEulerAngles = Scale;
        }
    }

    public void On_Alien_CloseProxyEnter(GameObject sender, Collider2D other)
    {
        Player_inside_Proxy = true;
        switch (state)
        {
            case States.Pizza:
                Debug.Log("Alien SAW THE PLAYER");
                Player.GetComponentInChildren<TextMeshPro>().text = "Press E to capture the alien";
                break;
            case States.Run:
                GettingCaught++;
                LastTick_Timer_Increase = Time.time;
                break;
            case States.Caught:
                break;
            case States.CarriedByPlayer:
                break;
        }
    }

    public void On_Alien_CloseProxyStay(GameObject sender, Collider2D other)
    {
        switch (state)
        {
            case States.Pizza:
                if (Input.GetKey(KeyCode.E))
                {
                    state = States.Run;
                    Player.GetComponentInChildren<TextMeshPro>().text = "The Alien is running! Try and catch him!";
                }
                break;
            case States.Run:
                if(LastTick_Timer_Increase + TickTime_GettingCaught < Time.time && GettingCaught < 100)
                {
                    GettingCaught++;
                    LastTick_Timer_Increase = Time.time;
                }
                if(GettingCaught == 100)
                {
                    state = States.Caught;
                }
                break;
            case States.Caught:
                Player.GetComponentInChildren<TextMeshPro>().text = "The Alien was caught! Press E to pick him up!";
                if (Input.GetKey(KeyCode.E))
                {
                    state = States.CarriedByPlayer;
                    transform.parent = Player.transform;
                    Destroy(move);
                    Destroy(GetComponent<Rigidbody2D>());
                }
                break;
            case States.CarriedByPlayer:
                Player.GetComponentInChildren<TextMeshPro>().text = "Take the Alien to the closest disposal facility";
                break;
        }
    }

    public void On_Alien_CloseProxyExit(GameObject sender, Collider2D other)
    {
        Player_inside_Proxy = false;
        switch (state)
        {
            case States.Pizza:
                Debug.Log("Alien THE PLAYER LEFT");
                Player.GetComponentInChildren<TextMeshPro>().text = "";
                break;
            case States.Run:
                GettingCaught--;
                LastTick_Timer_Decrease = Time.time;
                break;
            case States.Caught:
                break;
            case States.CarriedByPlayer:
                break;
        }
    }
    public void On_Pizza_Reach(GameObject sender)
    {
        CurrentTarget = null;
        move.On_FinalDestinationReach -= On_Pizza_Reach;
    }
    public void On_Target_Reach(GameObject sender)
    {
        var farTarget = GameManagerMockup.instance.FarthestNPC(transform.position);
        var closeTarget = GameManagerMockup.instance.ClosestNPC(transform.position);
        var newTarget = new GameObject("Current_Alien_Target");
        newTarget.transform.position = (farTarget.transform.position + closeTarget.transform.position) / 2;
        newTarget.transform.localScale /= 4;
        newTarget.AddComponent<SpriteRenderer>().sprite = CloseProxy.GetComponent<SpriteRenderer>().sprite;
        newTarget.GetComponent<SpriteRenderer>().color = Color.blue;

        if (newTarget == null) Debug.LogError("jeez how dafq did this happen????");
        var oldTarget = CurrentTarget;

        CurrentTarget = newTarget;
        move.SetDestination(CurrentTarget.transform.position);

        Destroy(oldTarget);
    }
    //added this because of NPC doesnt have a ground check right now, this is a temporary fix
    public IEnumerator ExecuteActionAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
