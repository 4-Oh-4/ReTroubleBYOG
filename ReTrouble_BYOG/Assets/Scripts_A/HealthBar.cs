using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
    [SerializeField] private HealthManager_A playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private Sprite[] heart;
    private int index;

    void Start() {
        index = hearts.Length - 1;
    }

    public void descreaseHealth() {
        if (index < 0) return;

        // Change sprite
        hearts[index].GetComponent<Image>().sprite = heart[1];

        // Add jerk effect
        StartCoroutine(HeartJerk(hearts[index].transform));

        index--;
    }

    private IEnumerator HeartJerk(Transform heart) {
        Vector3 originalScale = heart.localScale;
        Vector3 targetScale = originalScale * 1.2f;  // slightly larger
        float speed = 10f; // jerk speed
        float time = 0f;

        // Scale up quickly
        while (time < 1f) {
            time += Time.deltaTime * speed;
            heart.localScale = Vector3.Lerp(originalScale, targetScale, time);
            yield return null;
        }

        time = 0f;

        // Scale back down
        while (time < 1f) {
            time += Time.deltaTime * speed;
            heart.localScale = Vector3.Lerp(targetScale, originalScale, time);
            yield return null;
        }

        heart.localScale = originalScale;
    }
}
