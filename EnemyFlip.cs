using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private EnemyBehavior enemyBehavior;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    private void Update()
    {
        if (!enemyBehavior.IsDead())
        {
            // Flip the enemy sprite based on the player's position
            if (playerTransform.position.x > transform.position.x)
                spriteRenderer.flipX = false; // Face right
            else if (playerTransform.position.x < transform.position.x)
                spriteRenderer.flipX = true; // Face left
        }
    }
}
