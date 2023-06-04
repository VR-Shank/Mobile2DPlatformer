using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehavior : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    [SerializeField] private Animator playerAnimator;

    public void Attack()
    {
        PlayerController pc = GetComponent<PlayerController>();
        pc.SetCanTakeDamage(false); // Disable player's ability to take damage during the attack
        playerAnimator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("We hit a " + enemy.name);
            if (enemy.name == "Slime")
            {
                enemy.GetComponent<EnemyBehavior>().TakeDamage(attackDamage);
            }
            else if (enemy.name == "Crocodilian")
            {
                enemy.GetComponent<EnemyBehavior>().TakeDamage(attackDamage);
            }
            else if (enemy.name == "Bandit")
            {
                enemy.GetComponent<EnemyBehavior>().TakeDamage(attackDamage);
            } 
            else if (enemy.name == "Bullet")
            {
                enemy.GetComponent<Bullet>().ReturnToStartPosition();
                pc.BulletDeflected();
            }
        }

        StartCoroutine(DelayedCanTakeDamage(1f, pc)); // Re-enable the player's ability to take damage after a delay
    }

     void OnDrawGizmosSelected()
{
    if (attackPoint == null) return;
    Gizmos.DrawWireSphere(attackPoint.position, attackRange);
}

    private IEnumerator DelayedCanTakeDamage(float delay, PlayerController pc)
    {
        yield return new WaitForSeconds(delay);
        pc.SetCanTakeDamage(true); // Enable player's ability to take damage
    }
}
