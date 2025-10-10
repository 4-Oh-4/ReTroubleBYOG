using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_D : MonoBehaviour
{

    private float movementX;
    [SerializeField] private float speed = 0.0f;


    private Rigidbody2D playerRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();

    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 movement = new Vector2 (movementX, 0);
        playerRb.AddForce (movement * speed);
    }
}
