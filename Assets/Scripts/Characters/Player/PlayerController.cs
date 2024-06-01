using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Controls controls;
    private Vector2 directionX;
    private Vector2 directionY;
    private Rigidbody2D rb2d;
    public float movementSpeed;
    public float jumpSpeed;
    Animator anim;
    private bool lookingRight = true;
    private bool isGrounded;
    private PlayerAttack playerAttack;
    private Transform enemyTransform;
    private EnemyController enemyController;

    private void Awake()
    {
        controls = new();
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Start()
    {
        enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void OnEnable()
    {
        controls.Enable();

    }

    private void OnDisable()
    {
        controls.Disable();

    }

    // Update is called once per frame
    void Update()
    {
        directionX = controls.Movement.Move.ReadValue<Vector2>();
        directionY = controls.Movement.Jump.ReadValue<Vector2>();

        if (!GameController.gameController.isGameFinished)
        {
            if (transform.position.x < enemyTransform.position.x && !lookingRight)
            {
                Rotate();
            }
            else if (transform.position.x > enemyTransform.position.x && lookingRight)
            {
                Rotate();
            }

            if (playerAttack.CheckIsAttacking() || playerAttack.CheckIsGuarding() || playerAttack.CheckIsGettingHit())
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (!playerAttack.CheckIsAttacking() && !playerAttack.CheckIsGuarding() && !playerAttack.CheckIsGettingHit() && !playerAttack.CheckIsDead())
            {
                Move();
                Jump();
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

    }

    void FixedUpdate()
    {
        Animate();
    }

    void Move()
    {
        rb2d.velocity = new Vector2(directionX.x * movementSpeed, rb2d.velocity.y);
    }

    void Animate()
    {
        anim.SetBool("IsWalking", rb2d.velocity.x != 0);
        anim.SetBool("IsJumping", rb2d.velocity.y > 0);
        anim.SetBool("IsLanding", rb2d.velocity.y < 0 && !isGrounded);
        anim.SetBool("IsGuarding", playerAttack.CheckIsGuarding());
    }

    private void Rotate()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Jump()
    {
        if (isGrounded && rb2d.velocity.y == 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, directionY.y * jumpSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameController.gameController.isGameFinished)
        {
            return;
        }

        if (collision.tag.Equals("Ground")) isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ground")) isGrounded = false;
    }

    public bool CheckIsGrounded()
    {
        return isGrounded;
    }
}
