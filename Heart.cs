using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D heart)
    {
        // Check if the object tagged Player comes in contact with the heart
        if (heart.CompareTag("Player"))
        {
            // Check if the player's current health is less than the maximum health
            if (PlayerController.currentHealth < PlayerController.maxHealth)
            {
                // Add health to the player
                PlayerController.currentHealth += 20;
                // Test: Print the updated health
                Debug.Log("You currently have " + PlayerController.currentHealth + " HP!");
                // Destroy the heart object
                Destroy(gameObject);
            }
        }
    }
}
Please ensure that the currentHealth and maxHealth variables in the PlayerController script are indeed declared as static for this code to work correctly.






