using UnityEngine;
using System.Collections;
public class instructionLevel1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject cage;
    [SerializeField] GameObject img;
    bool once = true;
    void Start()
    {
        StartCoroutine(spawnImg());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void coloredChagned() {
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
