using UnityEngine;

public class FusionBubble : MonoBehaviour {
    // Unused variable, can be removed.
    // private Color[] colorArray = { Color.yellow, Color.cyan, Color.magenta, Color.white };
    int index1;
    int index2;
    // public int colorIndex; // This is a duplicate, the one in DestroyBubbleFusionN is used.
    public float UpwardForce = 3f;
    [SerializeField] GameObject bubblePrefab;

    // The empty Start and Update methods can be removed for cleanliness.

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("FusionBubble")) return;

        index1 = gameObject.GetComponent<DestroyBubbleFusionN>().colorIndex;
        index2 = collision.gameObject.GetComponent<DestroyBubbleFusionN>().colorIndex;

        // --- SIMPLIFIED CHECK ---
        // We no longer need to check for canMerge. The trigger handles it.
        if (index1 == index2) return;

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

        destroyer.SetColor(colorIdx);

        // Make sure to pass 'true' so the new bubble knows to start its grace period.
        bubble.Initialize(stage, dir, true);

        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();
        float inheritedY = Mathf.Max(parentYVel * 0.5f, 3f);
        rb.linearVelocity = new Vector2(bubble.initialSpeed * dir, inheritedY);
        rb.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);
    }
}