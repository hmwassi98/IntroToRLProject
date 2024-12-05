using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class SoccerBallAgent : Agent
{
    private Rigidbody rb;

    public Transform goal; // Reference to the goal
    public Rigidbody ballRigidbody; // Reference to the ball's Rigidbody

    // Initialize Rigidbody
    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>(); // Initialize the Rigidbody component
        ballRigidbody = GetComponent<Rigidbody>(); // Assign ball's Rigidbody if needed
    }

    // Reset the ball at the beginning of each episode
    public override void OnEpisodeBegin()
    {
        // Reset the ball's position and velocity
        transform.localPosition = new Vector3(0, 1, 0); // Adjust starting position
        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Optionally reset obstacles if needed
    }

    // Collect observations from the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the ball's position and velocity
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(ballRigidbody.linearVelocity);

        // Add goal's position
        sensor.AddObservation(goal.localPosition);
    }

    // Handle the actions received from the neural network
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Extract continuous actions for X and Z directions
        float moveX = actions.ContinuousActions[0];  // Move in X direction
        float moveZ = actions.ContinuousActions[1];  // Move in Z direction

        // Move the ball using forces
        rb.AddForce(new Vector3(moveX, 0f, moveZ), ForceMode.VelocityChange);

        // Reward logic (optional for additional goals or behaviors)
        if (transform.localPosition.x >= goal.localPosition.x - 1f && transform.localPosition.x <= goal.localPosition.x + 1f)
        {
            SetReward(1f); // Reward for reaching the goal area
            EndEpisode(); // End the episode when goal is reached
        }
    }

    // Handle user input for the Heuristic (manual control for testing)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Set the continuous actions in the output parameter
        var continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = 0f;
        continuousActions[1] = 0f;

        // Map key presses to movement actions
        if (Input.GetKey(KeyCode.W)) continuousActions[1] = 1f;
        if (Input.GetKey(KeyCode.S)) continuousActions[1] = -1f;
        if (Input.GetKey(KeyCode.A)) continuousActions[0] = -1f;
        if (Input.GetKey(KeyCode.D)) continuousActions[0] = 1f;
    }

    // Detect collisions with obstacles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            SetReward(-0.5f); // Negative reward for hitting obstacles
            EndEpisode(); // End the episode after hitting an obstacle
        }
    }
}
