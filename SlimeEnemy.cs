using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : MonoBehaviour
{
    [SerializeField] Animator slimeAnimator;

    [SerializeField] float minJumpForce = 100;
    [SerializeField] float maxJumpForce = 300;
    [SerializeField] float minWaitTime = 3f;
    [SerializeField] float maxWaitTime = 7f;
    [SerializeField] EnemyBehavior eb;
   
    private bool canJump;
    private bool jump;


    private float jumpForce;
    private Rigidbody2D rb; // reference to the Rigidbody2D component

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canJump && jump)
        {
            Jump();
            jump = false; // Reset jump flag
        } else if (!canJump && rb.velocity.y < 0)
        {
            slimeAnimator.SetBool("Falling", true);
        }
    }

    IEnumerator JumpLogic()
    {
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        if (eb.IsDead()) yield break;

        
        jump = true; // Set jump flag to true
    }

    void Jump()
    {
        jumpForce = Random.Range(minJumpForce, maxJumpForce);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        slimeAnimator.SetTrigger("Jump");
        canJump = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            slimeAnimator.SetBool("Falling", false);
            slimeAnimator.SetTrigger("Landing");
            canJump = true;
            StartCoroutine(JumpLogic()); // Start JumpLogic coroutine again
        }
    }
}
