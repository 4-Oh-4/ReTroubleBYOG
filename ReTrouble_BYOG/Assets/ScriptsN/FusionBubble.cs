using UnityEngine;

public class FusionBubble : MonoBehaviour
{
    private Color[] colorArray = { Color.yellow, Color.cyan, Color.magenta, Color.white };
    int index1;
    int index2;
    public int colorIndex;
    public float UpwardForce = 3f;
    [SerializeField] GameObject bubblePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        index1 = gameObject.GetComponent<DestroyBubbleFusionN>().colorIndex;
        if (collision.gameObject.CompareTag("FusionBubble")) {
            Debug.Log("Collision 1");
            index2 = collision.gameObject.GetComponent<DestroyBubbleFusionN>().colorIndex;
            if (index1 == index2) return;
            colorIndex = index1 + index2;
            Debug.Log("Collision 2 "+ colorIndex.ToString());
            Destroy(collision.gameObject);
            float parentYVel = GetComponent<Rigidbody2D>().linearVelocity.y;
            int nextStage = GetComponent<HeightAdjustmentFusion_N>().Stage;
            SpawnBubble(transform.position, nextStage, 1, parentYVel);
            Destroy(gameObject);

        }
    }
    private void SpawnBubble(Vector3 pos, int nextStage, int dir, float parentYVel) {
        GameObject newBubble = Instantiate(bubblePrefab, pos, Quaternion.identity);

        var bubble = newBubble.GetComponent<HeightAdjustmentFusion_N>();
        var destroyer = newBubble.GetComponent<DestroyBubbleFusionN>();

        // Color
        destroyer.SetColor(colorIndex+2);

        // Initialize stage & visuals
        bubble.Initialize(nextStage, dir, true);

        // Get the Rigidbody
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();

        // Give the new bubble an initial diagonal velocity
        // upward velocity = fraction of parent's + some boost
        float inheritedY = Mathf.Max(parentYVel * 0.5f, 3f); // tweakable values
        rb.linearVelocity = new Vector2(bubble.initialSpeed * dir, inheritedY);

        // Optional: short upward force pulse to make it feel springy
        rb.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);
    }


}
