using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[RequireComponent(typeof(MoveTo))]
public class NPC : MonoBehaviour
{
    private MoveTo move;

    [Range(1,100)]
    public int Panic = 1;

    public GameObject[] PlacesToAttend;
    public int Index_PlacesToAttend = 0;

    //Panic Meter mockup
    public GameObject Mask;

    public void Start()
    {
        move = GetComponent<MoveTo>();
  
        StartCoroutine(ExecuteActionAfterDelay(() =>
        {
            var aux = new Vector3[3];

            for(int i =0; i < 3; i++)
            {
                aux[i] = PlacesToAttend[i].transform.position;
            }
            move.SetDestinations(aux);
            move.method = MoveTo.Method.Speed;
            move.On_FinalDestinationReach += RepeatRoutine;

        }, 1f));
    }
    public void Update()
    {
        Mask.transform.localPosition = new Vector3((Panic / 100f) * -6f, 0f, 0f);
    }
    public void FixedUpdate()
    {
        Flip();
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
    public void RepeatRoutine(GameObject sender)
    {
        //Debug.Log("Day ended lets go home" + gameObject.name);
        var aux = new Vector3[3];

        for (int i = 0; i < 3; i++)
        {
            aux[i] = PlacesToAttend[i].transform.position;
        }
        move.SetDestinations(aux);
    }

    //added this because of NPC doesnt have a ground check right now, this is a temporary fix
    public IEnumerator ExecuteActionAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
