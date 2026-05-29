using UnityEngine;

public class BombMovement : MonoBehaviour
{
    [SerializeField] private float dropSpeed = 5f; // Adjust this value to control the speed of dropping
    [SerializeField] private float dropDistance = 5f; // Adjust this value to control how far the object drops
    private Rigidbody2D rb;
    private bool isDropping = false;
  
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
  
    private void Update()
    {
        if (isDropping)
        {
            // Calculate the new position with downward movement
            Vector3 newPosition = transform.position + Vector3.down * dropSpeed * Time.deltaTime;

            // Clamp the dropping to the desired distance
            if (newPosition.y <= transform.position.y - dropDistance)
            {
                newPosition.y = transform.position.y - dropDistance;
                isDropping = false; // Stop dropping once desired distance is reached
            }

            // Update the object's position
            transform.position = newPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check the direction of the collision (relative velocity)
            Vector2 relativeVelocity = collision.relativeVelocity;
            if (relativeVelocity.y > 0) // The player has hit the bomb from above
            {
                Debug.Log("Boom!");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDropping = true; // Start dropping when triggered by the player
        }
    }

   
}
