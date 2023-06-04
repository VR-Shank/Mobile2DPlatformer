using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // public GameManager gameManager;

    public LayerMask groundMask; // layers considered as ground
   // public LayerMask attackLayer; //Enemy Layers
    
    
    public float maxSpeed; // on ground speed
    public float limitedSpeed; // in air speed
    public float jumpDuration = 1.8f;
    public float jumpForce = 10f; // force applied to jump
    public float groundDistance = 0.1f; // distance of the ground check ray
    public Animator animator;

    public HealthBar healthBar;
    public static int maxHealth = 100;
    public static int currentHealth;

    public Transform groundCheck; // position of the ground check object
    private Rigidbody2D rb; // reference to the Rigidbody2D component
    private Vector3 facingDirection = Vector3.right; // current facing direction of the player
    private bool limitSpeed; //Toggle if speed from max to limited
    private bool canJump;
    private bool isGrounded; // whether the player is on the ground
    private bool beenHit;
    private float jumpTimer;
    private float speed; // movement speed of the player
    private float horizontalInput;
    private bool canTakeDamage = true; // Add a flag to control whether the player can take damage
    private bool playerHasDied=false;
    private int bulletsDeflected = 0;


    void Start()
    {
        bulletsDeflected = 0;
        rb = GetComponent<Rigidbody2D>();
        //Prevent character from falling over
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void Update()
    {
        // horizontal movement

        if(beenHit){
            PushBack(-0.2f, 0.2f);
            speed = 0f;
            horizontalInput = rb.velocity.x;
        } else  {
            if(!limitSpeed) {
            speed = maxSpeed;
            } else {
            speed = limitedSpeed;
            }
            horizontalInput = InputDirection.x;
        }
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // flip the character if it's moving in a different direction
        if (horizontalInput > 0 && facingDirection != Vector3.right)
        {
            facingDirection = Vector3.right;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalInput < 0 && facingDirection != Vector3.left)
        {
            facingDirection = Vector3.left;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // jump
        if (canJump && InputDirection.y > 0f && !TooTiredToJump())
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpTimer = jumpDuration;
            // reduce speed during jump
            limitSpeed=true;
        } else {
            limitSpeed = false;
        }

        if(playerHasDied == true || rb.position.y <= 400){
            playerHasDied = false;
            // FindObjectOfType <GameManager>().EndGame();
        }

        jumpTimer -= Time.fixedDeltaTime;

    
        // ground check
        // isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        // if (isGrounded)
        // {
        //     canJump = true;
        //     speed = 300f; // reset speed when the player touches the ground
        // }
    }

    // Update InputDirection from joystick
    public Vector3 InputDirection { get; private set; }

    public void SetInputDirection(Vector3 direction)
    {
        InputDirection = direction;
    }

    private bool TooTiredToJump()
    {
        return jumpTimer > 0;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {

        // allow jumping again
        if (col.collider.tag == "Ground")
        {
            canJump = true;
            limitSpeed = false;
        }

        if (col.collider.tag == "Slime")
        {
            EnemyBehavior slimeEnemy = col.collider.GetComponent<EnemyBehavior>();

            if (slimeEnemy != null && !slimeEnemy.IsDead() && canTakeDamage)
            {
                EnemyCollision(0.5f, 15, 1.6f);
            }
           
        }
        if (col.collider.tag == "Crocodilian")
        {
            EnemyBehavior crocodilian = col.collider.GetComponent<EnemyBehavior>();

            if (crocodilian != null && !crocodilian.IsDead() && canTakeDamage)
            {
                EnemyCollision(60f, 15, 1.6f);
            }
        }
        if (col.collider.tag == "Bandit")
        {
           EnemyBehavior bandit = col.collider.GetComponent<EnemyBehavior>();

            if (bandit != null && !bandit.IsDead() && canTakeDamage)
            {
                EnemyCollision(60f, 10, 1.6f);
            }
        }
        if (col.collider.tag == "Bullet")
        {
           Bullet bullet = col.collider.GetComponent<Bullet>();

            if (bullet != null && canTakeDamage)
            {
                EnemyCollision(60f, 10, 1.6f);
            }
        }
        
    }


    public bool SetCanTakeDamage(){
        return true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Ground"){
        canJump = false;
        //animator.SetBool("IsJumping", false);
        }

    }
    
     public void SetCanTakeDamage(bool value)
    {
        canTakeDamage = value;
    }

    private void EnemyCollision(float push, int damage, float time) {
            //PushBack(push, 0.2f);
            TakeDamage(damage);
            beenHit = true;
            StartCoroutine(EnableMovementAfterDelay(time)); // enable movement after a certain time
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("BeenHit");

        if(currentHealth <= 0) {
            //Die();
            playerHasDied= true;
        }
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
    yield return new WaitForSeconds(delay);
    speed = 0;
    beenHit = false; // restore horizontal movement
    }

//     void OnDrawGizmosSelected()
// {
//     if (attackPoint == null) return;
//     Gizmos.DrawWireSphere(attackPoint.position, attackRange);
// }

// Offset the player's position by a specified amount
public void PushBack(float offset, float timeOffset)
{
    if(facingDirection == Vector3.right)
    {
        transform.position += new Vector3(offset, 0, 0);
        DelayedOffset(new Vector3 (offset * 0.6f, 0, 0),timeOffset);
        DelayedOffset(new Vector3 (offset * 0.3f, 0, 0), timeOffset * 2);
        DelayedOffset(new Vector3 (offset * 0.1f, 0, 0), timeOffset * 3);
    } else { 
        transform.position += new Vector3(-offset, 0, 0);
        DelayedOffset(new Vector3 (-offset * 0.6f, 0, 0),timeOffset);
        DelayedOffset(new Vector3 (-offset * 0.3f, 0, 0), timeOffset * 2);
        DelayedOffset(new Vector3 (-offset * 0.1f, 0, 0),timeOffset * 3);
        
    }
    // beenHit = false;
    
}
//After a certain amount of time should offset the position
 private IEnumerator DelayedOffset(Vector3 offset, float delay)
{
    yield return new WaitForSeconds(delay);
     transform.position += offset;
}

public void BulletDeflected(){
    bulletsDeflected++;
}
    
// void Die(){
//     Debug.Log("You have Died!");
//     //Die animation
// }
}