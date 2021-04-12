using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    [SerializeField] GameObject frontSight;
    [SerializeField] GameObject[] waypoints = new GameObject[2];
    [Range(0f, 50f)] [SerializeField] float enemyMoveSpeed = 10f;
    [Range(1, 3)] [SerializeField] int healthPoint = 2;
    public GameObject Kunai;
    public GameObject KunaiSpawnLocation;
    public bool canMeleeAttack;
    public bool canThrowKunai;

    bool isRunning;
    bool isMovingRight = true;
    bool isDead;

    [HideInInspector]public bool isEngaged;
    bool isAttacking;
    bool isThrowing;
    bool isTakingDamage;

    float AttackTime;
    float AttackTimeInterval = 2f;
    
    PlayerController playerContoller;
    Animator EnemyAnim;
    SpriteRenderer enemySR;
    Color enemyOriginalColor;
    GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        playerContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        EnemyAnim = GetComponent<Animator>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyOriginalColor = enemySR.color;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isInGame)
            return;

        if (healthPoint == 0)
        {
            isDead = true;
        }

        if(!isDead)
        {
            EnemyMove();
        }
        else if(isDead)
        {
            EnemyAnim.SetTrigger("Dead");
            StartCoroutine(DestroyBody());
        }
    }

    IEnumerator DestroyBody()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void EnemyMove()
    {
        Flip();
        EnemySight();

        if (!isEngaged && canMeleeAttack)
        {
            if (isMovingRight)
            {
                transform.Translate(Vector2.right * Time.deltaTime * enemyMoveSpeed);
                isRunning = true;
                EnemyAnim.SetBool("Running", isRunning);
            }
            else if (!isMovingRight)
            {
                transform.Translate(Vector2.right * Time.deltaTime * -enemyMoveSpeed);
                isRunning = true;
                EnemyAnim.SetBool("Running", isRunning);
            }
        }

        if (isEngaged && canMeleeAttack)
        {
            if(Mathf.Abs(playerContoller.transform.position.x - transform.position.x) > 1.5f)
            {
                transform.Translate(GetDistance() * Time.deltaTime * enemyMoveSpeed);
                isRunning = true;
                EnemyAnim.SetBool("Running", isRunning);
            }
            else
            {
                isRunning = false;
                EnemyAnim.SetBool("Running", isRunning);
                if(!isAttacking && ((Time.time) >= AttackTime))
                {
                    AttackTime = Time.time + AttackTimeInterval;
                    Attack();
                }
            }
        }

        if(isEngaged && canThrowKunai)
        {
            if (!isThrowing && ((Time.time) >= AttackTime))
            {
                AttackTime = Time.time + AttackTimeInterval;
                ThrowKunai();
            }
        }
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;

        if (!isEngaged && canMeleeAttack)
        {
            if (isMovingRight && (Mathf.Abs(transform.position.x - waypoints[0].transform.position.x) <= 0.05f))
            {
                isMovingRight = false;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
            else if (!isMovingRight && (Mathf.Abs(transform.position.x - waypoints[1].transform.position.x) <= 0.05f))
            {
                isMovingRight = true;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
    }

    void EnemySight()
    {
        if ((Mathf.Abs(frontSight.transform.position.x - playerContoller.transform.position.x) <= 0.5f) && (Mathf.Abs(transform.position.y - playerContoller.transform.position.y)) <= 0.1f)
        {
            isEngaged = true;
        }
    }

    Vector3 GetDistance()
    {
        Vector3 newLocation = (playerContoller.transform.position - transform.position).normalized;
        return newLocation;
    }

    void Attack()
    {
        isAttacking = true;
        EnemyAnim.SetBool("Attacking", isAttacking);
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
        EnemyAnim.SetBool("Attacking", isAttacking);
    }

    void ThrowKunai()
    {
        isThrowing = true;
        EnemyAnim.SetBool("Throwing", isThrowing);
        Instantiate(Kunai, KunaiSpawnLocation.transform.position, KunaiSpawnLocation.transform.rotation);
        StartCoroutine(ResetThrow());
    }

    IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(0.5f);
        isThrowing = false;
        EnemyAnim.SetBool("Throwing", isThrowing);
    }

    public void AppplyDamage()
    {
        if(!isTakingDamage && !isDead)
        {
            isTakingDamage = true;
            enemySR.color = Color.red;
            healthPoint--;
            StartCoroutine(ResetTakingDamage());
        }
    }

    IEnumerator ResetTakingDamage()
    {
        yield return new WaitForSeconds(0.5f);
        isTakingDamage = false;
        enemySR.color = enemyOriginalColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isAttacking && collision.CompareTag("Player"))
        {
            playerContoller.AppplyDamage();
        }
    }
}
