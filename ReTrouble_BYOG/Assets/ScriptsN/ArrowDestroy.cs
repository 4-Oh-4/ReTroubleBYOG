using UnityEngine;

public class ArrowDestroy : MonoBehaviour
{
    public int ColorIndex =-1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bubble")) {
            if (ColorIndex == collision.gameObject.GetComponent<DestroyBubbleN>().colorIndex) {
                GetComponent<BoxCollider2D>().enabled = false;
                collision.gameObject.GetComponent<DestroyBubbleN>().DestroyBubble();
                DestroyArrow();
            } else {
                int initialStage = collision.gameObject.GetComponent<BubbleHeightAdjustment_N>().Stage;
                if(initialStage>1)collision.gameObject.GetComponent<BubbleHeightAdjustment_N>().Stage = initialStage-1;
                collision.gameObject.GetComponent<BubbleHeightAdjustment_N>().SetupStageVisuals();
                DestroyArrow();
            }
        }
        if (collision.CompareTag("Ceiling")) {
            DestroyArrow();
        }
    }
    public void DestroyArrow() {
        GameObject.FindGameObjectWithTag("Player").GetComponent<SpawnArrow_N>().canSpawn=true;
        Destroy(gameObject);
    }
    
}
