using UnityEngine;
using System.Linq;

public class FusionBubble : MonoBehaviour {
    private int index1;
    private int index2;
    public float UpwardForce = 3f;
    [SerializeField] private GameObject bubblePrefab;

    // We need references to the colliders to easily manage them.
    private Collider2D physicalCollider;
    private Collider2D triggerCollider;

    void Awake() {
        // Find and assign the two colliders on this bubble.
        // This assumes you have one solid collider and one trigger collider.
        var colliders = GetComponents<Collider2D>();
        physicalCollider = colliders.FirstOrDefault(c => !c.isTrigger);
        triggerCollider = colliders.FirstOrDefault(c => c.isTrigger);

        if (physicalCollider == null || triggerCollider == null) {
            Debug.LogError("FusionBubble requires one physical Collider2D and one trigger Collider2D.", this);
        }
    }

    /// <summary>
    /// This method is called when another bubble enters our LARGER trigger zone,
    /// BEFORE the physical colliders have a chance to touch and bounce.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other) {
        // Ensure we are interacting with another bubble.
        if (!other.CompareTag("FusionBubble")) return;

        // Get components to check colors.
        var thisBubbleDestroyer = GetComponent<DestroyBubbleFusionN>();
        var otherBubbleDestroyer = other.GetComponent<DestroyBubbleFusionN>();

        if (thisBubbleDestroyer == null || otherBubbleDestroyer == null) return;

        int localIndex1 = thisBubbleDestroyer.colorIndex;
        int localIndex2 = otherBubbleDestroyer.colorIndex;

        // Check if the bubbles have the same color OR if either is a "fused" bubble.
        if (localIndex1 == localIndex2 || localIndex1 > 2 || localIndex2 > 2) {
            // Find the other bubble's physical collider.
            Collider2D otherPhysicalCollider = other.GetComponents<Collider2D>().FirstOrDefault(c => !c.isTrigger);

            // If both physical colliders exist, tell the physics engine to ignore them.
            // This PREVENTS the initial bounce from ever happening.
            if (physicalCollider != null && otherPhysicalCollider != null) {
                Physics2D.IgnoreCollision(physicalCollider, otherPhysicalCollider);
            }
        }
    }

    /// <summary>
    /// This method is now only responsible for handling the actual MERGE logic
    /// when two compatible bubbles make physical contact.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision) {
        // The logic for ignoring collisions is now in OnTriggerEnter2D.
        // This method will now only fire for bubbles that are allowed to collide and fuse.

        if (!collision.gameObject.CompareTag("FusionBubble")) return;

        var thisBubbleDestroyer = GetComponent<DestroyBubbleFusionN>();
        var otherBubbleDestroyer = collision.gameObject.GetComponent<DestroyBubbleFusionN>();

        if (thisBubbleDestroyer == null || otherBubbleDestroyer == null) return;

        index1 = thisBubbleDestroyer.colorIndex;
        index2 = otherBubbleDestroyer.colorIndex;

        // A simplified check is still good practice here as a fallback.
        if (index1 == index2 || index1 > 2 || index2 > 2) return;

        if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID()) {
            return;
        }

        Debug.Log("Fusion initiated by: " + gameObject.name);
        
        int newColorIndex = index1 + index2 + 2;
        float parentYVel = GetComponent<Rigidbody2D>().linearVelocity.y;
        int currentStage = GetComponent<HeightAdjustmentFusion_N>().Stage;
        Vector3 spawnPosition = transform.position;

        SpawnBubble(spawnPosition, currentStage, 1, parentYVel, newColorIndex);

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }

    private void SpawnBubble(Vector3 pos, int stage, int dir, float parentYVel, int colorIdx) {
        GameObject newBubble = Instantiate(bubblePrefab, pos, Quaternion.identity);

        var bubble = newBubble.GetComponent<HeightAdjustmentFusion_N>();
        var destroyer = newBubble.GetComponent<DestroyBubbleFusionN>();

        if (destroyer != null) destroyer.SetColor(colorIdx);
        if (bubble != null) bubble.Initialize(stage, dir, true);

        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
        if (rb != null) {
            float inheritedY = Mathf.Max(parentYVel * 0.5f, 3f);
            rb.linearVelocity = new Vector2(bubble.initialSpeed * dir, inheritedY);
            rb.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);
        }
    }
}

