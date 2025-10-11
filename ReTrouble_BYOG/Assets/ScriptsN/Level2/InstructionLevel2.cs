using System.Collections;
using UnityEngine;

public class InstructionLevel2 : MonoBehaviour {
    [SerializeField] GameObject cage;
    [SerializeField] GameObject img;
    bool once = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(spawnImg());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ReadInstructions() {
        if (!once) return;
        once = false;
        StartCoroutine(despawn());
    }
    IEnumerator despawn() {

        yield return new WaitForSecondsRealtime(3f);
        img.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        cage.SetActive(false);

        //gameObject.SetActive(false);
    }
    IEnumerator spawnImg() {
        yield return new WaitForSecondsRealtime(1f);
        img.SetActive(true);
    }
}
