using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float destructionDelay = 1.5f; // Delay before destroying the bullet after collision
    public bool beenDeflected = false;
    private Rigidbody2D rb;
    private Bandit bandit; // Reference to the Bandit instance that shot the bullet

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Method to set the Bandit reference when the bullet is shot
    public void SetShooter(Bandit shooter)
    {
        bandit = shooter;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !beenDeflected)
        {
            // Schedule destruction after the specified delay
            StartCoroutine(DestroyBulletAfterDelay(destructionDelay));
        }
        else if (collision.gameObject.CompareTag("Player") && beenDeflected)
        {
            ReturnToStartPosition();
        }
    }

    private IEnumerator DestroyBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public bool BeenDeflected()
    {
        return true;
    }

    public void ReturnToStartPosition()
    {
        Transform deflectionPoint = FindObjectOfType<PlayerController>().transform;
        Transform returnPoint = bandit.transform;

        if (bandit != null)
        {
            rb.velocity = Vector2.zero;
            float shotPower = bandit.ShotPower();
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y); // Use the bullet's current position
            Vector2 returnPos = new Vector2(returnPoint.position.x, returnPoint.position.y);
            Vector2 direction = returnPos - myPos; // Reverse the direction calculation
            rb.velocity = direction.normalized * shotPower;
        }
    }
}
