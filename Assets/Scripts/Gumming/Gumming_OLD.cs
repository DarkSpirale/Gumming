using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTerrain;

public class Gumming_OLD : MonoBehaviour
{
    [Range(0, 1)]
    public float timeSlower;

    //Variables visible from  the inspector
    [SerializeField]
    private bool debug = false;

    [Header("Movement properties")]
    [SerializeField]
    private float walkSpeed = 50.0f;

    [SerializeField]
    private float diggingSpeed = 500.0f;

    [SerializeField]
    private float diggingTime = 1.0f;   

    [SerializeField]
    private float gravityOnGround = 0.1f;

    [SerializeField]
    private float gravityInAir = 2.0f;

    [Header("Surrounding check properties")]
    [SerializeField]
    private float groundCheckDistance = 0.1f;

    [SerializeField]
    private float wallCheckDistance = 0.1f;

    [SerializeField]
    private float ceilingCheckDistance = 0.1f;

    [SerializeField]
    private float ceilingCheckCooldown = 0.2f;

    [SerializeField]
    private Transform[] groundCheck;

    [SerializeField]
    private Transform[] wallCheck;

    [SerializeField]
    private Transform ceilingCheck;

    [SerializeField]
    private LayerMask worldLayer;


    //--Private Variables
    private Vector2 workspace;
    private Vector2 currentVelocity;

    private Shape diggingShape;

    private float diggingStartTime;

    private int facingDirection = 1;

    private bool isGrounded = true;
    private bool shouldDetectCeiling = true;
    private bool isDigging;

    //--Component References
    private Rigidbody2D rb;
    private Animator animator;
    private CircleCollider2D col;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();

        currentVelocity = rb.velocity;
        isDigging = false;

        diggingShape = Shape.GenerateShapeCircle(15);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isDigging)
        {
            diggingStartTime = Time.time;
            col.enabled = false;
            isDigging = true;
        }
        Time.timeScale = timeSlower;
    }

    private void FixedUpdate()
    {
        currentVelocity = rb.velocity;
        CheckSurroundings();
        ApplyGravity();
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        if (isDigging)
        {
            if(Time.time >= diggingStartTime + diggingTime)
            {
                isDigging = false;
                col.enabled = true;
            }
            else
            {
                workspace.Set(diggingSpeed * facingDirection * Time.fixedDeltaTime, 0.0f);
                SetVelocity(workspace);
                WorldManager.instance.DestroyShape(diggingShape, transform.position);
            }
            
        }
        if (isGrounded && !isDigging)
        {
            SetVelocityX(walkSpeed * facingDirection * Time.fixedDeltaTime);
        }
        else if (!isGrounded && !isDigging)
        {
            SetVelocityX(0.0f);
        }
    }

    private void ApplyGravity()
    {
        if (isDigging)
        {
            rb.gravityScale = 0.0f;
        }
        else if (isGrounded)
        {
            rb.gravityScale = gravityOnGround;
        }
        else
        {
            rb.gravityScale = gravityInAir;
        }
    }

    #region Set Functions
    private void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, currentVelocity.y);
        rb.velocity = workspace;
    }

    private void SetVelocityY(float velocity)
    {
        workspace.Set(currentVelocity.x, velocity);
        rb.velocity = workspace;
    }

    private void SetVelocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
    #endregion

    #region Check Functions
    private void CheckSurroundings()
    {
        CheckGround();
        CheckWall();
        CheckCeiling();
    }

    private void CheckGround() 
    {
        isGrounded = false;

        for (int i = 0; i < groundCheck.Length; i++)
        {
            if(Physics2D.Raycast(groundCheck[i].position, Vector2.down, groundCheckDistance, worldLayer))
            {
                isGrounded = true;
                continue;
            }
        }

        if (debug)
        {
            for (int i = 0; i < groundCheck.Length; i++)
            {
                Debug.DrawRay(groundCheck[i].position, Vector2.down * groundCheckDistance, Color.blue);
            }
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    private void CheckWall()
    {
        if (!isDigging)
        {
            RaycastHit2D hitTop = Physics2D.Raycast(wallCheck[0].position, Vector2.right * facingDirection, wallCheckDistance, worldLayer);
            RaycastHit2D hitBottom = Physics2D.Raycast(wallCheck[1].position, Vector2.right * facingDirection, wallCheckDistance, worldLayer);

            if (hitTop && hitBottom)
            {
                ChangeDirection();
            }
        }

        if (debug)
        {
            for (int i = 0; i < wallCheck.Length; i++)
            {
                Debug.DrawRay(wallCheck[i].position, facingDirection * wallCheckDistance * Vector2.right, Color.blue);
            }
        }
    }

    private void CheckCeiling()
    {
        if (shouldDetectCeiling)
        {
            RaycastHit2D hit = Physics2D.Raycast(ceilingCheck.position, Vector2.up, ceilingCheckDistance, worldLayer);

            if (hit)
            {
                ChangeDirection();
                shouldDetectCeiling = false;
                StartCoroutine(CeilingCheckCooldown());
            }
        }

        if (debug) Debug.DrawRay(ceilingCheck.position, Vector2.up * ceilingCheckDistance, Color.blue);
    }

    IEnumerator CeilingCheckCooldown()
    {
        yield return new WaitForSeconds(ceilingCheckCooldown);
        shouldDetectCeiling = true;
    }
    #endregion

    private void ChangeDirection()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180f, 0.0f);
        SetVelocityY(0.0f); //Reset Y velocity when flipping in case it was going up
    }


}
