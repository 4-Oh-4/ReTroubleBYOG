using UnityEngine;

public class ArrowDestroy : MonoBehaviour
{
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
            
            DestroyArrow();
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
