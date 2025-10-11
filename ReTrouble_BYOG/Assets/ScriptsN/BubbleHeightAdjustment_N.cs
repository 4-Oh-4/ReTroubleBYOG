using UnityEngine;

public class BubbleHeightAdjustment_N : MonoBehaviour {
    [Header("Component References")]
    [SerializeField] private Rigidbody2D rb;

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

    private Vector2 savedVelocity;
    private float savedAngularVelocity;
    private bool isFrozen = false;

    // ? NEW: A flag to prevent initializing more than once.
    private bool hasBeenInitialized = false;

    // ? MODIFIED: Start() now handles the VERY FIRST bubble.
    private void Start() {
        // If Initialize() has not been called externally (i.e., this is the first bubble)
        // then set it up with default values.
        if (!hasBeenInitialized) {
            Initialize(Stage, 1); // Default to stage 1, moving right
        }
    }

    private void FixedUpdate() {

        if (isFrozen)
        {
            return;
        }


        rb.linearVelocity = new Vector2(currentVelocityX, rb.linearVelocity.y);
        float dampingForce = -rb.linearVelocity.y * yAxisDamping;
        rb.AddForce(new Vector2(0, dampingForce));
        CheckForDeactivation();
    }

    // This method is now used for ALL bubbles (initial and spawned).
    public void Initialize(int newStage, int direction,bool spawnedAfterCollision=false) {
        if (hasBeenInitialized) return;
        if (spawnedAfterCollision) combo = true;
        Stage = newStage;
        currentVelocityX = initialSpeed * direction;
        SetupStageVisuals();

        // Directly set horizontal velocity immediately
        if (rb != null)
            rb.linearVelocity = new Vector2(currentVelocityX, 0f);

        hasBeenInitialized = true;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        combo = false;
        if (collision.gameObject.CompareTag("Floor")) return;

        if (collision.gameObject.CompareTag("Player")) {

            //New Sheild Check
            PlayerController_D player = collision.gameObject.GetComponent<PlayerController_D>();
            if (player != null && player.isShielded)
            {
                player.DisableShield();
                //Destroy(gameObject);
                return;
            }


            collision.gameObject.GetComponent<HealthManager_A>().DecreaseHealth();

            Destroy(gameObject);
            Debug.Log("health--");
            return;
        }
        ChangeDirection();
    }

    // --- Powerup Methods ---
    public void Freeze()
    {
        if (rb == null) return;
        isFrozen = true;
        savedVelocity = rb.linearVelocity;
        savedAngularVelocity = rb.angularVelocity;
        rb.bodyType = RigidbodyType2D.Static; // This completely stops all physics movement
                                              // Optional: Change color to show it's frozen
        //GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void Unfreeze()
    {
        if (rb == null) return;
        isFrozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = savedVelocity;
        rb.angularVelocity = savedAngularVelocity;
        // Restore original color - relies on DestroyBubbleN to have the color
       // GetComponent<DestroyBubbleN>().SetColor(GetComponent<DestroyBubbleN>().colorIndex);
    }
}