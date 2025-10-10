using UnityEngine;

public class DestroyBubbleN : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] BubbleHeightAdjustment_N bubbleHeight;
    [SerializeField] GameObject bubble;
    private int stage;
    private float offset;
    public int colorIndex=-1;
    private Color[] colorArray = { Color.red, Color.green, Color.blue };

    private void Start() {
        stage = bubbleHeight.getStage();
        if (colorIndex == -1) {
            colorIndex = Random.Range(0, 3);
            gameObject.GetComponent<SpriteRenderer>().material.color = colorArray[colorIndex];
        }
    }
    public void DestroyBubble() {
        if (stage >= 3) Destroy(gameObject);
        else {
            if (stage == 1) {
                offset = 1.5f;
            } else {
                offset = 1f;
            }
            GameObject bubble1 = Instantiate(bubble);
            bubble1.transform.localPosition = transform.localPosition - new Vector3(0, 0,0);
            bubble1.GetComponent<BubbleHeightAdjustment_N>().Stage=(stage + 1);
            bubble1.GetComponent<DestroyBubbleN>().SetColor(colorIndex);
            bubble1.GetComponent<BubbleHeightAdjustment_N>().SetupStage(stage+1);
            GameObject bubble2 = Instantiate(bubble);
            bubble2.transform.localPosition = transform.localPosition + new Vector3(0, 0, 0);
            bubble2.GetComponent<BubbleHeightAdjustment_N>().Stage=(stage + 1);
            bubble2.GetComponent<BubbleHeightAdjustment_N>().SetupStage(stage + 1);
            bubble2.GetComponent<DestroyBubbleN>().SetColor(colorIndex);
            bubble1.GetComponent<BubbleHeightAdjustment_N>().ChangeDirection();
            Destroy(gameObject);
        }
    }
    public void SetColor(int i) {
        colorIndex = i;
        gameObject.GetComponent<SpriteRenderer>().material.color = colorArray[colorIndex];

    }
}
