using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Public Variables
    [SerializeField] float jumpForce = 50f;
    [SerializeField] float moveSpeed = 30f;
    public int healthPoint = 4;
    public GameObject Kunai;
    public GameObject KunaiSpawnLocation;
    
    public int KunaiHeld = 4;
    public bool hasDoubleJumpAbility;

    //Private Variables
    bool CanDoubleJump;
    bool isRunning;
    bool isJumping;
    bool isFacingRight = true;
    bool isDead;
    bool isTakingDamage;
    bool isThrowing;
    
    bool isAttacking = false;
    float AttackTime;
    float AttackTimeInterval = 0.5f;
    
    float horizontalValue;
    float verticalValue;

    Rigidbody2D playerRb;
    Animator playerAnim;
    SpriteRenderer playerSR;
    Color playerOriginalColor;
    GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerSR = GetComponent<SpriteRenderer>();
        playerOriginalColor = playerSR.color;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoint == 0)
        {
            isDead = true;
        }

        if(!isDead)
        {
            PlayerMainControls();
        }
        else if (isDead)
        {
            playerAnim.SetTrigger("Dead");
        }
        
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

    void PlayerMainControls()
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
        if (isRunning)
        {
            playerRb.velocity = new Vector2(horizontalValue * moveSpeed, verticalValue);
        }

        //Initiate Attack
        Attack();

        //Start Throwing
        Throwing();
    }

    //Do Jump
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
            isJumping = true;
            playerAnim.SetBool("Jumping", isJumping);

            if(CanDoubleJump)
            {
                isJumping = false;
                CanDoubleJump = false;
            }
        }
    }

    //Initiate Attack
    void Attack()
    {
        if ((Input.GetKeyDown(KeyCode.LeftControl) || Input.GetMouseButtonDown(0)) && !isAttacking)
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
        if ((Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(1)) && !isThrowing && KunaiHeld != 0)
        {
            isThrowing = true;
            playerAnim.SetBool("Throwing", isThrowing);
            AttackTime = Time.time + AttackTimeInterval;

            Instantiate(Kunai, KunaiSpawnLocation.transform.position, KunaiSpawnLocation.transform.rotation);
            KunaiHeld--;
            gameManager.UpdateKunaiGUI();
        }
        else if (isThrowing && (Time.time > AttackTime))
        {
            isThrowing = false;
            playerAnim.SetBool("Throwing", isThrowing);
        }
    }

    public void AppplyDamage()
    {
        if (!isTakingDamage && !isDead)
        {
            isTakingDamage = true;
            playerSR.color = Color.red;
            healthPoint--;
            gameManager.UpdateHealthGUI();
            StartCoroutine(ResetTakingDamage());
        }
    }

    IEnumerator ResetTakingDamage()
    {
        yield return new WaitForSeconds(0.5f);
        isTakingDamage = false;
        playerSR.color = playerOriginalColor;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<MyEnemy>().AppplyDamage();
        }
    }

    //Check if Player is in Air or not
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            if(hasDoubleJumpAbility)
            {
                CanDoubleJump = true;
            }
            playerAnim.SetBool("Jumping", isJumping);
        }
    }
}
