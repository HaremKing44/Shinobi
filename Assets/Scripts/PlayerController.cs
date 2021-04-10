using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    [SerializeField] float jumpForce = 50f;
    [SerializeField] float moveSpeed = 30f;
    public GameObject Kunai;
    public GameObject KunaiSpawnLocation;

    //Private Variables
    bool isRunning;
    bool isJumping;
    bool isFacingRight = true;
    bool isThrowing;
    
    bool isAttacking = false;
    float AttackTime;
    float AttackTimeInterval = 0.5f;
    
    float horizontalValue;
    float verticalValue;
    
    Rigidbody2D playerRb;
    Animator playerAnim;
    CircleCollider2D playerCC;

    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCC = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move Player horizontally
        horizontalValue = Input.GetAxis("Horizontal");
        if (horizontalValue != 0f && !isAttacking && !isThrowing)
        {
            isRunning = true;
            playerAnim.SetBool("Running", isRunning);
        }
        else
        {
            isRunning = false;
            playerAnim.SetBool("Running", isRunning);
        }

        //player vertical value.
        verticalValue = playerRb.velocity.y;

        //Do Jump
        Jump();

        //Move the Player
        if(isRunning)
        {
            playerRb.velocity = new Vector2(horizontalValue * moveSpeed, verticalValue);
        }

        //Initiate Attack
        Attack();

        //Start Throwing
        Throwing();
    }

    private void LateUpdate()
    {
        Vector3 localScale = transform.localScale;

        if(horizontalValue > 0f)
        {
            isFacingRight = true;
        }
        else if (horizontalValue < 0f)
        {
            isFacingRight = false;
        }

        if(((isFacingRight) && (localScale.x < 0)) || ((!isFacingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }

        transform.localScale = localScale;
    }

    //Do Jump
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
            isJumping = true;
            playerAnim.SetBool("Jumping", isJumping);
        }
    }

    //Initiate Attack
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isAttacking)
        {
            isAttacking = true;
            playerAnim.SetBool("Attacking", isAttacking);
            AttackTime = Time.time + AttackTimeInterval;
        }
        else if (isAttacking && (Time.time > AttackTime))
        {
            isAttacking = false;
            playerAnim.SetBool("Attacking", isAttacking);
        }
    }

    //Start Throwing
    void Throwing()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isThrowing)
        {
            isThrowing = true;
            playerAnim.SetBool("Throwing", isThrowing);
            AttackTime = Time.time + AttackTimeInterval;

            Instantiate(Kunai, KunaiSpawnLocation.transform.position, KunaiSpawnLocation.transform.rotation);
        }
        else if (isThrowing && (Time.time > AttackTime))
        {
            isThrowing = false;
            playerAnim.SetBool("Throwing", isThrowing);
        }
    }

    //Check if Player is in Air or not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            playerAnim.SetBool("Jumping", isJumping);
        }
    }
}
