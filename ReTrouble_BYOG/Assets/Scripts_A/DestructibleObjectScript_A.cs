using System.Reflection.Emit;
using UnityEngine;

public class DestructibleObjectScript_A : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D objectCollider;
    private bool isDestroyed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        objectCollider = GetComponent<Collider2D>();
    }

    public void HitByArrow()
    {
        if (isDestroyed == true) return;
        isDestroyed = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        objectCollider.enabled = false;
        Destroy(gameObject, 3f);
    }
}
