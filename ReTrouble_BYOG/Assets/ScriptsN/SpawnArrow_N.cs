using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnArrow_N : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    public bool canSpawn = true;
    [SerializeField] float arrowSpeed=4f;
    [SerializeField]private int index = 0;
    private Color[] colorArray = { Color.red ,Color.green, Color.blue,Color.white};
    public bool frenzy = false;

    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnArrow(InputAction.CallbackContext context) {
        
        if (canSpawn && context.phase==InputActionPhase.Performed) {
            
            GameObject arrow=Instantiate(arrowPrefab);
            arrow.transform.localPosition = transform.localPosition;
            arrow.GetComponentInChildren<SpriteRenderer>().color = colorArray[index];
            arrow.GetComponent<ArrowDestroy>().ColorIndex = index;
            arrow.GetComponent<Rigidbody2D>().linearVelocityY = arrowSpeed;
            canSpawn = false;
        }
    }
    public void ChangeColorPositive(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed && !frenzy) {
            index = (index + 1) % 3;
            gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
        }
    }
    public void ChangeColorNegative(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed && !frenzy) {
            index = (index - 1+3) % 3;
            gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
        }
    }
    public void EnableFrenzy() {
        frenzy = true;
        index = 3;
        gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
    }
    public void DisableFrenzy() {
        frenzy = false;
        index = Random.Range(0,3);
        gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
    }
}
