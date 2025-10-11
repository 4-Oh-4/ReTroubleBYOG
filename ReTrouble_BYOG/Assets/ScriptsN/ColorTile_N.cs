using UnityEngine;

public class ColorTile_N : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] int index = -1;
    private Color[] colorArray = { Color.red, Color.yellow, Color.cyan };
    private void Start() {
        if (index == -1) {
            index = Random.Range(0, 3);
        }
        gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bubble")) {
            collision.gameObject.GetComponent<DestroyBubbleN>().SetColor(index);
            resetColor();
        }
        if (collision.CompareTag("FusionBubble")) {
            if (collision.gameObject.GetComponent<DestroyBubbleFusionN>().colorIndex > 2) return;
            else collision.gameObject.GetComponent<DestroyBubbleFusionN>().SetColor(index);
            resetColor();
        }
    }
    void resetColor() {
        index = Random.Range(0, 3);
        gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
    }
}
