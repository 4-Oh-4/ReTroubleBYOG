// PlayerMovement_Events.cs
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController_D : MonoBehaviour {
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public bool useSmoothing = true;
    public float acceleration = 10f;

    [Header("Powerup Settings")]
    [SerializeField] private GameObject shieldVisual;
    public bool isShielded { get; private set; } = false; // Public to read, private to set

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentVelocityX;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>(); // Get the Animator component
    }



    // This function name MUST match the action name prefixed with "On" (e.g., OnMove)
    public void OnMove(InputAction.CallbackContext context) {
        // Read the Vector2 from input
        moveInput = context.ReadValue<Vector2>();
    }

    

    void FixedUpdate() {
        // Only use X axis for horizontal movement
        float targetX = moveInput.x * moveSpeed;
        float newVX = useSmoothing
            ? Mathf.SmoothDamp(rb.linearVelocity.x, targetX, ref currentVelocityX, 1f / acceleration)
            : targetX;

        rb.linearVelocity = new Vector2(newVX, rb.linearVelocity.y);

        // Set the isMoving boolean in the animator
        anim.SetBool("IsMoving", Mathf.Abs(moveInput.x) > 0.1f);

        // Optional: flip sprite based on direction
        if (Mathf.Abs(moveInput.x) > 0.01f) {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveInput.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }


    public void EnableShield()
    {
        isShielded = true;
        shieldVisual.SetActive(true);
    }

    public void DisableShield()
    {
        isShielded = false;
        shieldVisual.SetActive(false);
    }
}
