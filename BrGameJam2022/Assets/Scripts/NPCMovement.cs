using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    //Movement Global Vars
    public CharacterController2D controller2D;
    public float runSpeed = 40f;
    public bool isJumping = false;
    public bool isCrouching = false;
    public float horizontalMove = 0f;
    public Rigidbody2D rb;

    //Animation Global Vars
    private Animator anim;

    //Pathing Global Vars
    public Transform position1;
    public Transform position2;
    public Transform position3;

    private Transform target;
    //private Queue<Transform> targetQueue;
    private int targetHit;

    public Vector3 direction;

    //Player interaction
    private bool isInTalkRange = false;

    // Start is called before the first frame update
    void Start()
    {
        //anim = GetComponent<Animator>();
        target = position1;
    }

    // Update is called once per frame
    void Update()
    {
        direction = target.position - transform.position;
        direction.Normalize();
        
    }
    private void FixedUpdate()
    {
        if (!isInTalkRange)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //target = targetQueue.Peek();

            if (Mathf.Round(transform.position.y) == target.position.y)
            {
                rb.velocity = Vector3.zero;
                targetHit++;
                nextTarget();
            }
            else
            {
                if (targetHit == 4)
                    targetHit = 0;
                MoveCharacter(direction);
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void MoveCharacter(Vector3 dir)
    {
        rb.MovePosition((Vector3)transform.position + (dir * runSpeed * Time.deltaTime));
    }

    private void nextTarget()
    {

        if (targetHit == 1)
        {
            target = position2;
        }
        else if (targetHit == 2)
        {
            target = position3;
        }
        else if (targetHit == 3)
        {
            target = position2;
        }
        else
        {
            target = position1;
            //targetQueue.Enqueue(position1);
        }
        //target = targetQueue.Dequeue();
    }

}
