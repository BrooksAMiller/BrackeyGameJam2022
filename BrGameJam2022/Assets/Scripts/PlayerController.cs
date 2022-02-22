using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    private float curSpeed;
    public float jumpSpeed;
    public int maxJumps;
    public float gravityScaler;
    public float checkRadius;
    public LayerMask groundCheckLayer;
    public LayerMask wallCheckLayer;
    public bool CloseToVertWall;
    public float wallCheckRadius;
    public Transform wallChecker;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private float moveInput;
    private float ladderInput;
    [HideInInspector]
    public bool isGrounded;
    private bool isRunning = false;
    private bool lookingRight = true;

    public Transform groundChecker;
    private int extraJumps;

    private bool isClimbing = false;
    public LayerMask ladderMask;
    public float distance;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curSpeed = walkSpeed;
    } 
    private void Update()
    {
        
        moveInput = Input.GetAxisRaw("Horizontal");
       
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            curSpeed = runSpeed;
            anim.SetBool("IsRunning", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            anim.SetBool("IsRunning", false);
            curSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("rollTrigger");
        }
        if (isRunning && !CloseToVertWall)
        {
            anim.SetFloat("moveSpeed", (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))));
        }
        else if (!isRunning && !CloseToVertWall)
        {
            anim.SetFloat("moveSpeed", (Mathf.Abs(Input.GetAxisRaw("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"))) * 0.5f);
        }
        else if (CloseToVertWall)
            anim.SetFloat("moveSpeed", 0f);

        anim.SetBool("IsGrounded", isGrounded);
        if (Input.GetAxisRaw("Horizontal") > 0 && !lookingRight || Input.GetAxisRaw("Horizontal") < 0 && lookingRight)
        {
            Flip();
        }
        if (isGrounded)
        {
            extraJumps = maxJumps;
            anim.SetBool("IsJumping", false);

        }
        if ((Input.GetKeyDown(KeyCode.Space) && (extraJumps > 0)) || (Input.GetKeyDown(KeyCode.Space) && CloseToVertWall))
        {
            rb.velocity = Vector2.up * jumpSpeed;
            extraJumps--;
            anim.SetTrigger("takeOff");
            if (CloseToVertWall)
                anim.SetTrigger("wallJump");
            else if (!isGrounded)
                anim.SetTrigger("rollTrigger");

        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpSpeed;
            anim.SetTrigger("takeOff");

            if (CloseToVertWall)
                anim.SetTrigger("wallJump");
        }
        if (!isGrounded)
        {

            anim.SetBool("IsJumping", true);
        }
    }

    
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, checkRadius, groundCheckLayer);
        CloseToVertWall = Physics2D.OverlapCircle(wallChecker.position, wallCheckRadius, wallCheckLayer);

        rb.velocity = new Vector2(moveInput * curSpeed, rb.velocity.y);
        if (rb.velocity.y < -0.1)
        {
            anim.SetTrigger("FallTrigger");
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, ladderMask);

        if (hitInfo.collider != null)
        {
            Debug.Log("Climbable");
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
            }
        }
        else
            isClimbing = false;

        if (isClimbing)
        {
            ladderInput = Input.GetAxisRaw("Vertical");
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0f, ladderInput * curSpeed);

        }
        else
            rb.gravityScale = gravityScaler;
    }

    void Flip()
    {
        lookingRight = !lookingRight;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        GetComponent<CapsuleCollider2D>().offset = new Vector2(GetComponent<CapsuleCollider2D>().offset.x * -1, GetComponent<CapsuleCollider2D>().offset.y);
        Vector3 Scaler = transform.GetChild(1).transform.localPosition;
        Scaler.x *= -1;
        transform.GetChild(1).transform.localPosition = Scaler;
    }

    public void OnLanding()
    {
        transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
    }
}
