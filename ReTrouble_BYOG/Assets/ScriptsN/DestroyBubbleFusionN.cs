using UnityEngine;

public class DestroyBubbleFusionN : MonoBehaviour {
    [Header("Component References")]
    [SerializeField] private HeightAdjustmentFusion_N bubbleHeight;
    [SerializeField] private GameObject bubblePrefab;

    [Header("Settings")]
    [SerializeField] private float UpwardForce = 3f;
    public int colorIndex = -1;

    // Color Array: 0:Red, 1:Green, 2:Blue, 3:Yellow, 4:Cyan, 5:Magenta
    private Color[] colorArray = { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta };
    private GameObject GM;

    private void Start() {
        // If this bubble is being created for the first time, assign a random primary color.
        if (colorIndex == -1) {
            colorIndex = Random.Range(0, 3); // 0, 1, or 2 (Red, Green, or Blue)
            GetComponent<SpriteRenderer>().color = colorArray[colorIndex];
        }
        GM = GameObject.FindGameObjectWithTag("GM");
    }

    public void DestroyBubble() {
        int currentStage = bubbleHeight.Stage;

        // If the bubble is too small, just destroy it.
        if (currentStage >= 4) {
            Destroy(gameObject);
            return;
        }

        int nextStage = currentStage + 1;
        Vector3 spawnPos = transform.position;
        float parentYVel = bubbleHeight.GetComponent<Rigidbody2D>().linearVelocity.y;

        // --- NEW MODIFIED LOGIC STARTS HERE ---

        int newColorIndex1;
        int newColorIndex2;

        // Check if the current bubble is a FUSED color (index > 2).
        if (colorIndex > 2) {
            // This is a FUSED bubble. Split it into its primary components.
            switch (colorIndex) {
                case 3: // Yellow splits into Red (0) and Green (1)
                    newColorIndex1 = 0;
                    newColorIndex2 = 1;
                    break;
                case 4: // Cyan splits into Green (1) and Blue (2)
                    newColorIndex1 = 1;
                    newColorIndex2 = 2;
                    break;
                case 5: // Magenta splits into Red (0) and Blue (2)
                    newColorIndex1 = 0;
                    newColorIndex2 = 2;
                    break;
                default: // Fallback case
                    newColorIndex1 = 0;
                    newColorIndex2 = 1;
                    break;
            }
        } else {
            // This is a PRIMARY color bubble. Split it into two of the SAME color.
            newColorIndex1 = colorIndex;
            newColorIndex2 = colorIndex;
        }

        // --- END OF MODIFIED LOGIC ---

        // Combo and Power-up logic (unchanged)
        if (bubbleHeight.combo) {
            GM.GetComponent<ComboManager_N>().IncreaseCombo();
        } else {
            GM.GetComponent<ComboManager_N>().ResetCombo();
        }
        int i = Random.Range(1, 8);
        if (i == 5 && GM.GetComponent<PowerManger_N>().hasPowerUp == false) {
            Debug.Log("PowerUP drop");
        }
        if (newColorIndex1 != newColorIndex2) nextStage--;
        Debug.Log("breaking");
        // Spawn the two smaller bubbles with their determined colors.
        SpawnBubble(spawnPos+new Vector3(1,0,0), nextStage, 1, parentYVel, newColorIndex1);  // right bubble
        SpawnBubble(spawnPos+ new Vector3(-1, 0, 0), nextStage, -1, parentYVel, newColorIndex2); // left bubble

        Destroy(gameObject);
    }

    // This method accepts a 'newColorIndex' to assign to the new bubble.
    private void SpawnBubble(Vector3 pos, int nextStage, int dir, float parentYVel, int newColorIndex) {
        GameObject newBubble = Instantiate(bubblePrefab, pos, Quaternion.identity);
        newBubble.GetComponent<Collider2D>().isTrigger = true;
        var bubble = newBubble.GetComponent<HeightAdjustmentFusion_N>();
        var destroyer = newBubble.GetComponent<DestroyBubbleFusionN>();

        // Assign the new color passed into this method.
        destroyer.SetColor(newColorIndex);

        // Initialize stage & visuals
        bubble.Initialize(nextStage, dir, true);

        // Get the Rigidbody
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();

        // Give the new bubble an initial diagonal velocity
        float inheritedY = Mathf.Max(parentYVel * 0.5f, 3f);
        rb.linearVelocity = new Vector2(bubble.initialSpeed * dir, inheritedY);

        // Optional: short upward force pulse
        rb.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);
    }

    public void SetColor(int i) {
        // Ensure the index is within the bounds of the color array
        if (i >= 0 && i < colorArray.Length) {
            colorIndex = i;
            GetComponent<SpriteRenderer>().color = colorArray[colorIndex];
        }
    }
}