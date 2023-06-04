using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    //[SerializeField] GameObject coinPrefab;
    [SerializeField] int enemyMaxHealth = 100;
    private int enemyCurrentHealth;

    private bool isDead = false;
    private Rigidbody2D rb;
    [SerializeField] Animator enemyAnimator;

    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        rb = GetComponent<Rigidbody2D>();
        //enemyAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (isDead)
        {
            LockPositionY();
            // Disable collision between player and enemy
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // If already dead, ignore further damage

        Debug.Log("Enemy took " + damage + " damage.");
        enemyCurrentHealth -= damage;
        enemyAnimator.SetTrigger("BeenHit");

        if (enemyCurrentHealth <= 0)
            Die();
    }

    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        Debug.Log("Enemy has been defeated!");

        // Die animation
        enemyAnimator.SetBool("IsDead", true);

        // Disable player's ability to take damage
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.SetCanTakeDamage(false);
        }

        // Disable enemy
        isDead = true;

        //Instantiate(coinPrefab, new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0), Quaternion.identity);

        int layerDead = LayerMask.NameToLayer("DeadEnemy");
        gameObject.layer = layerDead;



        StartCoroutine(StopCollision());

    }

    IEnumerator StopCollision()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(GetComponent<BoxCollider2D>());
    }

    void LockPositionY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }
}
