using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check the direction of the collision (relative velocity)
            Vector2 relativeVelocity = collision.relativeVelocity;
            if (relativeVelocity.y > 0) // The player has hit the enemy from above
            {
                Debug.Log("Player has hit the enemy from above");
                //Destroy(gameObject);
            }
        }
    }
}
