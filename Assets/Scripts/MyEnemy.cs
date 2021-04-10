using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    [SerializeField] GameObject frontSight;
    [SerializeField] GameObject[] waypoints = new GameObject[2];
    [Range(0f, 50f)] [SerializeField] float enemyMoveSpeed = 10f;

    bool isEngaged;
    bool isRunning;
    bool isMovingRight = true;
    bool isThrowing;
    bool isAttacking;
    float AttackTime;
    float AttackTimeInterval = 0.5f;
    PlayerController playerContoller;
    Animator EnemyAnim;

    // Start is called before the first frame update
    void Awake()
    {
        playerContoller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        EnemyAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        Flip();

        if (!isEngaged)
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

        if (isEngaged)
        {
            if (isAttacking)
            {
                isThrowing = false;
                EnemyAnim.SetBool("Attacking", isAttacking);
            }
            else if (isThrowing)
            {
                isAttacking = false;
                EnemyAnim.SetBool("Throwing", isThrowing);
            }
        }

        EnemySight();
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;

        if (!isEngaged)
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

        if (isEngaged)
        {
            if(Mathf.Abs(transform.position.x - playerContoller.transform.position.x) <= 2f)
            { 
                isAttacking = true;
            }
            else
            {
                isThrowing = true;
            }
        }

        //transform.localScale = localScale;
    }

    void EnemySight()
    {
        if (Mathf.Abs(frontSight.transform.position.x - playerContoller.transform.position.x) <= 0.5f)
        {
            isEngaged = true;
            isRunning = false;
            EnemyAnim.SetBool("Running", isRunning);
        }
    }
}
