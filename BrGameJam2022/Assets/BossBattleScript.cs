using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleScript : MonoBehaviour
{
    public float speed; //speed of gameObject
    public float checkRadius;
    public float attackRadius;

    private bool shouldRotate;
    public SpriteRenderer MySpriteRenderer;
    public LayerMask player; //Select layer of player 
    public Transform target1;
    public Transform target2;
    public Transform target3;

    private Transform target; // determine what to follow
    public Rigidbody2D rb;
    private Animator anim; // reference to animation component
    private Vector3 movement; // determine where to go
    public Vector3 dir;

    private bool isInAttackRange;
    private bool inPlayerRange;
    private int targetHit = 0;
    Queue<Transform> targetQueue = new Queue<Transform>();
    void Start()
    {
        anim = GetComponent<Animator>();
        targetQueue.Enqueue(target1);
    }


    void Update()
    {

        //Where to start, the radius to check, what layer mask to search for
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, player);
        inPlayerRange = Physics2D.OverlapCircle(transform.position, checkRadius, player);


        //dir is which direction to go, the target's position is subtracted from the current position of the gameObject
        dir = target.position - transform.position;
        // the angle of the target
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;



    }

    private void FixedUpdate()
    {



        if (inPlayerRange)
        {
            MoveCharacter(movement);
            target = GameObject.FindWithTag("Player").transform;
            Debug.Log("Player is target");
            if (Mathf.Round(transform.position.x) == (target.position.x) && Mathf.Round(transform.position.y) == (target.position.y))
            {
                Debug.Log("Attack Target");

            }

            if (isInAttackRange)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            }

        }
        else
        {

            target = targetQueue.Peek();
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (Mathf.Round(transform.position.x) == Mathf.Round(target.position.x) && Mathf.Round(transform.position.y) == Mathf.Round(target.position.y))
            {
                targetHit++;
                rb.velocity = Vector3.zero;
                nextTarget();
            }
            else
            {
                if (targetHit == 4)
                    targetHit = 0;
                MoveCharacter(movement);
            }
        }





    }

    private void MoveCharacter(Vector3 dir)
    {
        if (target.position.x > transform.position.x)
        {
            MySpriteRenderer.flipX = false;
        }
        else if (target.position.x < transform.position.x)
        {
            MySpriteRenderer.flipX = true;
        }
        if (target == GameObject.FindWithTag("Player").transform)
        {
            rb.MovePosition((Vector3)transform.position + (dir * speed * Time.deltaTime));
        }
        else
        {
            rb.MovePosition((Vector3)transform.position + (dir * speed * Time.deltaTime));
        }

    }

    private void nextTarget()
    {
        if (targetHit == 1)
        {
            targetQueue.Enqueue(target2);
        }
        else if (targetHit == 2)
        {
            targetQueue.Enqueue(target3);
        }
        else if (targetHit == 3)
        {
            targetQueue.Enqueue(target2);
        }
        else
        {
            targetQueue.Enqueue(target1);
        }
        target = targetQueue.Dequeue();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.pc.TakeDamage(1);
        }
    }
}
