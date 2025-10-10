using UnityEngine;

public class BubbleHeightAdjustment_N : MonoBehaviour // Renamed for clarity
{
    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField] private float velocityX = 3f;
    [SerializeField] private float yAxisDamping = 0.5f; // Our new custom damping for the Y-axis

    [Header("Stage Settings")]
    [SerializeField] private int Stage = 1;
    [SerializeField] private float maxYStage1;
    [SerializeField] private float maxYStage2;
    [SerializeField] private float maxYStage3;

    [Header("Deactivation Logic")]
    [SerializeField] private float YVelocityDeactivationThreshold = 0.1f;

    private float maxY;

    private void Start() {
        // Set the size and max height based on the stage
        SetupStage();

        // Set the initial velocity once
        rb.linearVelocity = new Vector2(velocityX, 0);
    }

    // All physics calculations should happen in FixedUpdate
    private void FixedUpdate() {
        // 1. Enforce a constant X velocity while preserving the Y velocity
        //rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        // 2. Apply our custom linear damping only on the Y-axis
        // This force opposes the current Y velocity, slowing it down.
        float dampingForce = -rb.linearVelocity.y * yAxisDamping;
        rb.AddForce(new Vector2(0, dampingForce));

        // 3. Check the condition to disable this script
        CheckForDeactivation();
    }

    private void CheckForDeactivation() {
        // If the bubble has slowed down enough and is below its max height...
        if (transform.position.y < maxY && rb.linearVelocity.y > 0 && rb.linearVelocity.y < YVelocityDeactivationThreshold) {
            Debug.Log("Bubble has stabilized, disabling height adjustment.");
            // Instead of disabling the component, you might want to just stop applying damping
            // but disabling the whole component works if that's your goal.
            this.enabled = false;
        }
    }

    private void SetupStage() {
        switch (Stage) {
            case 1:
                transform.localScale = new Vector2(2.1f, 2.1f);
                maxY = maxYStage1;
                break;
            case 2:
                transform.localScale = new Vector2(1.4f, 1.4f);
                maxY = maxYStage2;
                break;
            case 3:
                transform.localScale = new Vector2(0.7f, 0.7f);
                maxY = maxYStage3;
                break;
        }
    }
}