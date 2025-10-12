using UnityEngine;

public class FusionBubbleBound_N : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private Collider2D col; // --- ADD THIS LINE --- Reference to the collider

    void Awake()
    {
        
    }
    private void Update() {

        if (rb.transform.position.y >= 11 || rb.transform.position.y < 2 || rb.transform.position.x < -9 || rb.transform.position.x > 9) Destroy(gameObject);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (rb.linearVelocityX >=0) rb.linearVelocityX = 3;
        if (rb.linearVelocityX < 0) rb.linearVelocityX = -3;
        if (Mathf.Abs(rb.linearVelocityY) > 10) rb.linearVelocityY = 7;
        if (rb.linearVelocity.y >= 0 && rb.linearVelocity.y < 0.1f) {
            if (transform.position.y < 4) {
                rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
            }
        }
        if (Mathf.Abs(transform.position.x) > 7.5 && col.isTrigger) BoundX();
        if (col.isTrigger && (transform.position.y < 3.71 || transform.position.y > 9.6)) BoundY();
    }
    
    public void BoundX() {
        rb.linearVelocityX = -rb.linearVelocityX;
    }
    public void BoundY() {
        rb.linearVelocityY = 10;
    }
}
