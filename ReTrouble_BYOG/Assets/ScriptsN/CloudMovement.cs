using UnityEngine;

public class CloudMovement : MonoBehaviour {
    [Header("Movement Settings")]
    public float moveSpeed = 1.0f;          // How fast the cloud moves
    public float moveDistance = 5.0f;       // How far the cloud moves left/right from its start
    public float jitterAmount = 0.05f;      // How much random jitter
    public float jitterSpeed = 1.0f;        // How fast the jitter changes

    private Vector3 startPosition;
    private float currentJitterOffset;
    private float jitterTimer;
    private int moveDirection = 1;          // 1 for right, -1 for left

    void Start() {
        startPosition = transform.position; // Store the initial position of the cloud
        currentJitterOffset = Random.Range(-jitterAmount, jitterAmount);
        jitterTimer = Random.Range(0f, 100f); // Randomize start of jitter animation
    }

    void Update() {
        // --- Horizontal Back-and-Forth Movement ---
        float newX = transform.position.x + (moveDirection * moveSpeed * Time.deltaTime);

        // Check if it has moved past the right boundary
        if (moveDirection == 1 && newX > startPosition.x + moveDistance) {
            newX = startPosition.x + moveDistance; // Snap to boundary
            moveDirection = -1; // Change direction to left
        }
        // Check if it has moved past the left boundary
        else if (moveDirection == -1 && newX < startPosition.x - moveDistance) {
            newX = startPosition.x - moveDistance; // Snap to boundary
            moveDirection = 1; // Change direction to right
        }

        // --- Vertical Jitter ---
        // Update jitter timer
        jitterTimer += Time.deltaTime * jitterSpeed;
        // Use Sine wave for smooth, continuous jitter
        currentJitterOffset = Mathf.Sin(jitterTimer) * jitterAmount;

        // Apply movement and jitter
        transform.position = new Vector3(newX, startPosition.y + currentJitterOffset, transform.position.z);
    }

    // Optional: Call this if you want to restart a cloud's movement
    public void ResetCloud() {
        transform.position = startPosition;
        moveDirection = Random.Range(0, 2) * 2 - 1; // Randomly start left or right
        jitterTimer = Random.Range(0f, 100f);
        currentJitterOffset = Random.Range(-jitterAmount, jitterAmount);
    }
}