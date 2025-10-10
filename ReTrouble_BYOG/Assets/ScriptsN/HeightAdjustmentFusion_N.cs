using UnityEngine;
using System.Collections;

public class HeightAdjustmentFusion_N : MonoBehaviour {
    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col; // --- ADD THIS LINE --- Reference to the collider

    [Header("Movement Settings")]
    [SerializeField] public float initialSpeed = 3f;
    [SerializeField] private float yAxisDamping = 0.5f;

    [Header("Stage Settings")]
    public int Stage = 1;
    [SerializeField] private float maxYStage1;
    [SerializeField] private float maxYStage2;
    [SerializeField] private float maxYStage3;

    [Header("Deactivation Logic")]
    [SerializeField] private float YVelocityDeactivationThreshold = 0.1f;
    public bool combo = false;
    private float maxY;
    private float currentVelocityX;

    // We no longer need canMerge. Using the collider state is more reliable.
    // public bool canMerge = false; 

    private bool hasBeenInitialized = false;

    private void Start() {
        if (!hasBeenInitialized) {
            Initialize(Stage, 1);
        }
    }

    private void FixedUpdate() {
        if (Mathf.Abs(transform.position.x) > 7.5 && col.isTrigger) BoundX();
        if (col.isTrigger&&(transform.position.y < 3.71||transform.position.y>9.6)) BoundY();
        rb.linearVelocity = new Vector2(currentVelocityX, rb.linearVelocity.y);
        if (Mathf.Abs(rb.linearVelocityX) < 0.5f) {
            if (transform.position.x >= 0) rb.linearVelocityX = -3f;
            else rb.linearVelocityX = 3f;
        }
        float dampingForce = -rb.linearVelocity.y * yAxisDamping;
        rb.AddForce(new Vector2(0, dampingForce));
        CheckForDeactivation();
    }

    public void Initialize(int newStage, int direction, bool spawnedAfterCollision = false) {
        if (hasBeenInitialized) return;
        if (spawnedAfterCollision) combo = true;

        Stage = newStage;
        currentVelocityX = initialSpeed * direction;
        SetupStageVisuals();

        if (rb != null)
            rb.linearVelocity = new Vector2(currentVelocityX, 0f);

        hasBeenInitialized = true;

        // --- MODIFIED LOGIC HERE ---
        // If this bubble was spawned from a collision, start the grace period.
        if (spawnedAfterCollision) {
            //col.isTrigger = false;
            StartCoroutine(GracePeriodCoroutine());
        }
    }

    // This coroutine creates a 1-second "grace period" where the bubble cannot physically collide with other bubbles.
    IEnumerator GracePeriodCoroutine() {
        // 1. Make the collider a trigger so it passes through other bubbles.
        if (col != null) col.isTrigger = true;

        // 2. Wait for 1 second. (Using WaitForSeconds is generally better for gameplay timers).
        yield return new WaitForSeconds(0.3f);

        // 3. Revert the collider back to a normal, solid collider.
        if (col != null) col.isTrigger = false;
    }

    private void CheckForDeactivation() {
        if (transform.position.y < maxY && rb.linearVelocity.y > 0 && rb.linearVelocity.y < YVelocityDeactivationThreshold) {
            this.enabled = false;
        }
    }

    public void SetupStageVisuals() {
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
                transform.localScale = new Vector2(1f, 1f);
                maxY = maxYStage3;
                break;
            case 4:
                transform.localScale = new Vector2(0.7f, 0.7f);
                maxY = maxYStage3;
                break;
        }
    }

    public void ChangeDirection() {
        currentVelocityX = -currentVelocityX;
    }
    public void BoundX() {
        ChangeDirection();
    }
    public void BoundY() {
        rb.linearVelocityY = -rb.linearVelocityY;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        combo = false;
        if (collision.gameObject.CompareTag("Floor")) return;

        if (collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
            Debug.Log("health--");
            return;
        }
        ChangeDirection();
    }
}