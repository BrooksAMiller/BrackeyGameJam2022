using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController pc;

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

    public bool canInteractWithNPC;
    public GameObject interactableNPC;
    
    //temp for gun/neuralizer
    public int ammo = 99;
    public bool isShooting = false;
    public GameObject barrelEnd;
    public GameObject barrelEndFlipped;
    public GameObject bulletPrefab;
    public GameObject bulletContainer;
    private float timeStamp;
    public float coolDownTimer = 0.25f;

    public bool levelSwitchOptional = false;
    public bool onSurfaceLevel = true;

    public int playerHealth = 5;
    public GameObject playerHealthIcon;
    public GameObject playerHealthBar;

    public bool canClimbDownManhole = false;
    public bool hasDiscoveredAlien = false;

    public bool playerAlive = true;

    public AudioClip[] sounds;
    private AudioSource src;
    private AudioSource footStepSrc;
    public AudioClip pigeonDeathSound;

    private void Awake()
    {
        pc = this;
    }

    void Start()
    {
        footStepSrc = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        src = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curSpeed = walkSpeed;

        for (int i = 0; i < playerHealth; i++)
        {
            Instantiate(playerHealthIcon, playerHealthBar.transform);
        }
    } 

    public void TakeDamage(int dmg)
    {
        src.PlayOneShot(sounds[1]);

        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("shakeIt");
        playerHealth = playerHealth - dmg;
        Destroy(playerHealthBar.transform.GetChild(0).gameObject);
        if(playerHealth <= 0)
        {
            src.PlayOneShot(sounds[2]);
            playerAlive = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if(playerHealth > 0)
        {
            Debug.Log("player health: " + playerHealth + " ACTIVATE SCREEN SHAKE");
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameController.gc.pauseMenu.activeSelf)
                GameController.gc.pauseMenu.SetActive(false);
            else
            {
                GameController.gc.pauseMenu.SetActive(true);
            }
        }

        if(levelSwitchOptional && Input.GetKeyDown(KeyCode.E))
        {
            if (onSurfaceLevel)
                SceneManager.LoadScene(2);
            else
                SceneManager.LoadScene(1);
        }

        if (Input.GetMouseButtonDown(0) && ammo > 0 && !isShooting && !GameController.gc.dialogueObject.activeSelf && timeStamp <= Time.time)
        {
            src.PlayOneShot(sounds[0]);
            anim.SetTrigger("Shoot");
            isShooting = true;
            anim.SetBool("isShooting", true);

            if (lookingRight)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelEnd.transform);
                bullet.transform.parent = bulletContainer.transform;
                bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 7, ForceMode2D.Impulse);
                timeStamp = Time.time + coolDownTimer;

            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelEndFlipped.transform);
                bullet.transform.parent = bulletContainer.transform;
                bullet.GetComponent<Rigidbody2D>().AddForce(-transform.right * 7, ForceMode2D.Impulse);
                timeStamp = Time.time + coolDownTimer;

            }

        }
        
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

        if (canInteractWithNPC && Input.GetKeyDown(KeyCode.E))
        {
            GameController.gc.dialogueObject.SetActive(true);
            interactableNPC.GetComponent<Animator>().enabled = true;

            //GameController.gc.dialogueObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = interactableNPC.name;

            //GameController.gc.dialogueObject.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = interactableNPC.GetComponent<NPCDialogue>().answers[Random.Range(0, interactableNPC.GetComponent<NPCDialogue>().answers.Length)];
            //for (int i = 0; i < interactableNPC.GetComponent<NPCDialogue>().questions.Length; i++)
            //{
            //    GameController.gc.dialogueObject.transform.GetChild(2).transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>().text = interactableNPC.GetComponent<NPCDialogue>().questions[i];
            //    GameController.gc.dialogueObject.transform.GetChild(2).transform.GetChild(i).gameObject.SetActive(true);
            //}
        }


            if (!footStepSrc.isPlaying)
            {

                if ((rb.velocity.x > 0.1f || rb.velocity.x < -0.1f) && isGrounded)
                {
                    footStepSrc.Play();
                }
                else
                    footStepSrc.Stop();
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
        Debug.Log("landing");
        //transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
    }

    //called by shoot animation trigger
    public void stopShootAnimationCallback()
    {
        isShooting = false;
        anim.SetBool("isShooting", false);
    }
}
