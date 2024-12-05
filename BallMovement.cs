using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float moveSpeed = 10f;  // Speed multiplier for movement
    public float rotationSpeed = 5f; // Speed multiplier for rotation

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody attached to the ball
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Apply force to the Rigidbody
        rb.AddForce(movement * moveSpeed);

        // Rotate the ball for a rolling effect
        if (movement.magnitude > 0.1f)
        {
            Vector3 rotation = new Vector3(-moveVertical, 0.0f, moveHorizontal);
            rb.AddTorque(rotation * rotationSpeed);
        }
    }
}
