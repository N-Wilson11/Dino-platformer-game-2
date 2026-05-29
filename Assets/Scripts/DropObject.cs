using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObject : MonoBehaviour
{
    [SerializeField] private float dropSpeed = 5f; // Adjust this value to control the speed of dropping
    [SerializeField] private float dropDistance = 5f; // Adjust this value to control how far the object drops

    private bool isDropping = false;
    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (isDropping)
        {
            // Calculate the new position with downward movement
        Vector3 newPosition = transform.position + Vector3.down * dropSpeed * Time.deltaTime;

        // Update the object's position
        transform.position = newPosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            isDropping = true;
        }
    }
}
