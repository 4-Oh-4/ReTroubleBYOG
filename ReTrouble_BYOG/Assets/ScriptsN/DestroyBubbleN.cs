using System.Collections;
using UnityEngine;

public class DestroyBubbleN : MonoBehaviour {
    [SerializeField] private BubbleHeightAdjustment_N bubbleHeight;
    [SerializeField] private GameObject bubblePrefab; // Renamed for clarity
    [SerializeField] private float UpwardForce = 3f;
    public int colorIndex = -1;
    private Color[] colorArray = { Color.red, Color.green, Color.cyan };
    GameObject GM;
    private void Start() {
        if (colorIndex == -1) {
            colorIndex = Random.Range(0, 3);
            GetComponent<SpriteRenderer>().color = colorArray[colorIndex];
        }
        GM = GameObject.FindGameObjectWithTag("GM");
    }

    public void DestroyBubble() {
        int currentStage = bubbleHeight.Stage;

        if (currentStage >= 4) {
            Destroy(gameObject);
            return;
        }

        int nextStage = currentStage + 1;
        Vector3 spawnPos = transform.position;

        // Get upward motion from current bubble
        float parentYVel = bubbleHeight.GetComponent<Rigidbody2D>().linearVelocity.y;
        if (bubbleHeight.combo) {
            GM.GetComponent<ComboManager_N>().IncreaseCombo();
            //Debug.Log("Combo");
        } else {
            GM.GetComponent<ComboManager_N>().ResetCombo();
        }
        int i = Random.Range(1, 4);
        if (i == 1 && PowerManger_N.Instance != null)
        {
            PowerManger_N.Instance.SpawnRandomPowerup(transform.position);
            Debug.Log("PowerUP drop");
        }

        // --- TEMPORARY DEBUGGING CODE ---
        //Debug.Log("Attempting to spawn a powerup now.");
        //if (PowerManger_N.Instance != null)
        //{
        //    PowerManger_N.Instance.SpawnRandomPowerup(transform.position);
        //}
        //else
        //{
        //    Debug.LogError("PowerManger_N.Instance is NULL. Cannot spawn powerup!");
        //}
        // --- END OF TEMP CODE ---


        // Spawn the two smaller bubbles
        SpawnBubble(spawnPos, nextStage, 1, parentYVel);   // right
        SpawnBubble(spawnPos, nextStage, -1, parentYVel);  // left

        Destroy(gameObject);
    }

    private void SpawnBubble(Vector3 pos, int nextStage, int dir, float parentYVel) {
        GameObject newBubble = Instantiate(bubblePrefab, pos, Quaternion.identity);

        var bubble = newBubble.GetComponent<BubbleHeightAdjustment_N>();
        var destroyer = newBubble.GetComponent<DestroyBubbleN>();

        // Color
        destroyer.SetColor(colorIndex);

        // Initialize stage & visuals
        bubble.Initialize(nextStage, dir,true);

        // Get the Rigidbody
        Rigidbody2D rb = newBubble.GetComponent<Rigidbody2D>();

        // Give the new bubble an initial diagonal velocity
        // upward velocity = fraction of parent's + some boost
        float inheritedY = Mathf.Max(parentYVel * 0.5f, 3f); // tweakable values
        rb.linearVelocity = new Vector2(bubble.initialSpeed * dir, inheritedY);

        // Optional: short upward force pulse to make it feel springy
        rb.AddForce(Vector2.up * UpwardForce, ForceMode2D.Impulse);
    }


    


    public void SetColor(int i) {
        colorIndex = i;
        GetComponent<SpriteRenderer>().color = colorArray[colorIndex];
    }

}