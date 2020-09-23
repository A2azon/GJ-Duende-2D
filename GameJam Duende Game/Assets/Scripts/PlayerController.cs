using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float jumpForce;
    public float variableJumpHeightMultiplier = .5f;
    public float groundCheckRadius;
    public float wallCheckDistance;

    private float moveHorizontal;

    public bool isFacingRight = true;
    private bool isGrounded;
    public bool isTouchingWall;
    private bool checkJumpMultiplier;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsPushable;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckInput();
        UpdateAnimation();
        CheckMovementDirection();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckInput()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void UpdateAnimation()
    {
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(moveHorizontal * walkSpeed, rb.velocity.y);

        if (isTouchingWall)
        {
            walkSpeed = 0.8f;
        }
        else
        {
            walkSpeed = 5f;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        checkJumpMultiplier = true;
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsPushable);
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && moveHorizontal < 0)
        {
            Flip();
        }
        else if (!isFacingRight && moveHorizontal > 0)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        //transform.localScale = theScale;
        transform.Rotate(0.0f, 180.0f, 0.0f);//esto ayuda al isTouchingWall, ya que transform.right tambien funcionará si está mirando a la izquierda
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}