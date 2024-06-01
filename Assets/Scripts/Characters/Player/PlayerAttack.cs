using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public Controls controls;
    Animator anim;
    public GameObject attackControllerL;
    public GameObject attackControllerM;
    public GameObject attackControllerH;
    public float attackLDamage;
    public float attackMDamage;
    public float attackHDamage;
    public float timeBetweenAttackL;
    public float timeBetweenAttackM;
    public float timeBetweenAttackH;
    private float timeNextAttackL;
    private float timeNextAttackM;
    private float timeNextAttackH;
    private bool isAttacking;
    private bool isGuarding;
    private bool isDead;
    private EnemyController enemyController;
    public float health;
    public float maxHealth;
    private PlayerController playerController;
    public HealthBar healthBar;
    private bool isGettingHit;


    private void Awake()
    {
        controls = new();
        anim = GetComponent<Animator>();


        health = maxHealth;

        playerController = GetComponent<PlayerController>();
        healthBar = GameObject.Find("LeftHealthBar").GetComponent<HealthBar>();

    }

    void Start()
    {
        enemyController = FindObjectOfType<EnemyController>();
        healthBar.Initialize(health);
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

        if (timeNextAttackL > 0)
        {
            timeNextAttackL -= Time.deltaTime;
        }

        if (timeNextAttackM > 0)
        {
            timeNextAttackM -= Time.deltaTime;
        }

        if (timeNextAttackH > 0)
        {
            timeNextAttackH -= Time.deltaTime;
        }

        controls.Movement.AttackL.started += _ => AttackL();
        controls.Movement.AttackM.started += _ => AttackM();
        controls.Movement.AttackH.started += _ => AttackH();
        controls.Movement.Guard.started += _ => Guard();
        controls.Movement.Guard.canceled += _ => Unguard();

        GameController.gameController.playerHealth = health;

        if (GameController.gameController.isPlayerWin)
        {
            controls.Disable();
            anim.SetTrigger("Win");
        }
        else if (GameController.gameController.isEnemyWin)
        {
            if (!isDead)
                anim.SetTrigger("Die");
        }
    }

    void FixedUpdate()
    {

    }

    private void AttackL()
    {

        if (timeNextAttackL <= 0 && !isGuarding && playerController.CheckIsGrounded())
        {
            DoAttackL();
            timeNextAttackL = timeBetweenAttackL;
        }
    }
    private void AttackM()
    {

        if (timeNextAttackM <= 0 && !isGuarding && playerController.CheckIsGrounded())
        {
            DoAttackM();
            timeNextAttackM = timeBetweenAttackM;
        }
    }
    private void AttackH()
    {

        if (timeNextAttackH <= 0 && !isGuarding && playerController.CheckIsGrounded())
        {
            DoAttackH();
            timeNextAttackH = timeBetweenAttackH;
        }
    }

    private void DoAttackL()
    {
        anim.SetTrigger("AttackL");
    }

    private void DoAttackM()
    {
        anim.SetTrigger("AttackM");
        timeNextAttackL = timeBetweenAttackL;
    }

    private void DoAttackH()
    {
        anim.SetTrigger("AttackH");
        timeNextAttackL = timeBetweenAttackL;
        timeNextAttackM = timeBetweenAttackM;
    }

    private void Guard()
    {
        if (!isAttacking)
        {
            isGuarding = true;
        }
    }

    private void Unguard()
    {
        isGuarding = false;
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

    public bool CheckIsAttacking()
    {
        return isAttacking;
    }

    public bool CheckIsGuarding()
    {
        return isGuarding;
    }

    public bool CheckIsDead()
    {
        return isDead;
    }

    public bool CheckIsGettingHit()
    {
        return isGettingHit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("AttackL") && !isDead && !isGuarding)
        {
            anim.SetTrigger("HitL");
            getHit(enemyController.attackLDamage);
        }

        if (collision.tag.Equals("AttackM") && !isDead && !isGuarding)
        {
            anim.SetTrigger("HitM");
            getHit(enemyController.attackMDamage);
        }

        if (collision.tag.Equals("AttackH") && !isDead && !isGuarding)
        {
            anim.SetTrigger("HitH");
            getHit(enemyController.attackHDamage);
        }

    }

    public void getHit(float damage)
    {
        health -= damage;
        healthBar.ChangeCurrentHealth(health);

        if (health <= 0)
        {
            anim.SetTrigger("Die");
            isDead = true;
            GameController.gameController.isPlayerDead = isDead;
        }
    }
}
