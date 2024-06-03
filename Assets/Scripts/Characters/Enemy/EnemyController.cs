using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rb2d;
    public float movementSpeed;
    public float jumpSpeed;
    private Transform playerTransform;
    private bool followPlayer;
    private bool attackPlayer;
    private bool lookingLeft = true;
    private bool isAttacking;
    public GameObject attackControllerL;
    public GameObject attackControllerM;
    public GameObject attackControllerH;
    public float attackLDamage;
    public float attackMDamage;
    public float attackHDamage;
    public float attackDistance;
    public float chasePlayerAfterAttack;
    private float currentAttackTimer;
    public float deffaultAttackTimer;
    public float health;
    public float maxHealth;
    private bool isDead;
    private PlayerAttack playerAttack;
    public HealthBar healthBar;
    private bool isGettingHit;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();

        followPlayer = true;

        currentAttackTimer = deffaultAttackTimer;

        health = maxHealth;

        if (GameState.gameNumber == 2)
        {
            health += maxHealth / 2;
            deffaultAttackTimer /= 1.5f;
        }
        if (GameState.gameNumber == 3)
        {
            health += maxHealth;
            deffaultAttackTimer /= 2;
        }

        healthBar = GameObject.Find("RightHealthBar").GetComponent<HealthBar>();

    }

    void Start()
    {
        healthBar.Initialize(health);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.gameController.isGameFinished)
        {
            if (transform.position.x < playerTransform.position.x && lookingLeft)
            {
                Rotate();
            }
            else if (transform.position.x > playerTransform.position.x && !lookingLeft)
            {
                Rotate();
            }

            if (isAttacking || isGettingHit)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }

            if (!isDead)
            {
                if (!isAttacking && !isGettingHit)
                {
                    FollowPlayer();
                }

                AttackPlayer();
            }

            GameController.gameController.enemyHealth = health;
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }

        if (GameController.gameController.isEnemyWin)
        {
            anim.SetTrigger("Win");
        }
        else if (GameController.gameController.isPlayerWin)
        {
            if (!isDead)
                anim.SetTrigger("Die");
        }

    }

    void FixedUpdate()
    {
        Animate();
    }

    void Animate()
    {
        if (rb2d.velocity.x != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }

    void FollowPlayer()
    {
        if (!followPlayer)
        {
            return;
        }

        if (Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance)
        {
            if (playerTransform.transform.position.x < transform.position.x)
            {
                rb2d.velocity = new Vector2(-1 * movementSpeed, rb2d.velocity.y);
            }
            else
            {
                rb2d.velocity = new Vector2(1 * movementSpeed, rb2d.velocity.y);
            }
        }
        else if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= attackDistance)
        {
            rb2d.velocity = Vector2.zero;
            followPlayer = false;
            attackPlayer = true;
        }
    }

    void AttackPlayer()
    {
        if (!attackPlayer)
        {
            return;
        }

        currentAttackTimer -= Time.deltaTime;

        if (currentAttackTimer <= 0)
        {
            Attack(Random.Range(0, 4));
            currentAttackTimer = deffaultAttackTimer;
        }

        if (Mathf.Abs(transform.position.x - playerTransform.position.x) > attackDistance + chasePlayerAfterAttack)
        {
            attackPlayer = false;
            followPlayer = true;
        }
    }

    private void Rotate()
    {
        lookingLeft = !lookingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    IEnumerator AttackL(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("AttackL");
    }

    IEnumerator AttackM(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("AttackM");
    }

    IEnumerator AttackH(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("AttackH");
    }

    void Attack(int i)
    {
        switch (i)
        {
            case 0:
                StartCoroutine(AttackL(0.1f));
                break;
            case 1:
                StartCoroutine(AttackL(0.1f));
                StartCoroutine(AttackM(0.3f));
                break;
            case 2:
                StartCoroutine(AttackL(0.1f));
                StartCoroutine(AttackM(0.3f));
                StartCoroutine(AttackH(0.6f));
                break;
            case 3:
                StartCoroutine(AttackH(0.1f));
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameController.gameController.isGameFinished)
        {
            if (collision.tag.Equals("AttackL") && !isDead)
            {
                anim.SetTrigger("HitL");
                getHit(playerAttack.attackLDamage);
            }

            if (collision.tag.Equals("AttackM") && !isDead)
            {
                anim.SetTrigger("HitM");
                getHit(playerAttack.attackMDamage);
            }

            if (collision.tag.Equals("AttackH") && !isDead)
            {
                anim.SetTrigger("HitH");
                getHit(playerAttack.attackHDamage);
            }
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }

    public void StartGetHit()
    {
        isGettingHit = true;
    }

    public void FinishGetHit()
    {
        isGettingHit = false;
    }

    public void ActivateAttackL()
    {
        attackControllerL.SetActive(true);
    }

    public void ActivateAttackM()
    {
        attackControllerM.SetActive(true);
    }

    public void ActivateAttackH()
    {
        attackControllerH.SetActive(true);
    }

    public void DeactivateAttackL()
    {
        attackControllerL.SetActive(false);
    }

    public void DeactivateAttackM()
    {
        attackControllerM.SetActive(false);
    }

    public void DeactivateAttackH()
    {
        attackControllerH.SetActive(false);
    }

    public void DeactivateAllAttacks()
    {
        attackControllerL.SetActive(false);
        attackControllerM.SetActive(false);
        attackControllerH.SetActive(false);
    }

    public void getHit(float damage)
    {
        health -= damage;
        healthBar.ChangeCurrentHealth(health);

        if (health <= 0)
        {
            anim.SetTrigger("Die");
            isDead = true;
            GameController.gameController.isEnemyDead = isDead;
        }
    }
}
