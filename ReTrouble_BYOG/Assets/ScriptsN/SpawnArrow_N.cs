using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnArrow_N : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    public bool canSpawn = true;
    [SerializeField] float arrowSpeed=4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnArrow(InputAction.CallbackContext context) {
        
        if (canSpawn && context.phase==InputActionPhase.Performed) {
            GameObject arrow=Instantiate(arrowPrefab);
            arrow.transform.localPosition = transform.localPosition;
            arrow.GetComponent<Rigidbody2D>().linearVelocityY = arrowSpeed;
            canSpawn = false;
        }
    }
}
