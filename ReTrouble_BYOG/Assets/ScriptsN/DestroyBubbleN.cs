using UnityEngine;

public class DestroyBubbleN : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] BubbleHeightAdjustment_N bubbleHeight;
    [SerializeField] GameObject bubble;
    private int stage;
    private float offset;
    private void Start() {
        stage = bubbleHeight.getStage();
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

            GameObject bubble2 = Instantiate(bubble);
            bubble2.GetComponent<BubbleHeightAdjustment_N>().Stage=(stage + 1); 
            bubble2.transform.localPosition = transform.localPosition + new Vector3(0, 0, 0);
            bubble1.GetComponent<BubbleHeightAdjustment_N>().ChangeDirection();
            Destroy(gameObject);
        }
    }
}
