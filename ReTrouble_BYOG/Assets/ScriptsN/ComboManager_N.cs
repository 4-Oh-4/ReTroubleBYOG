using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboManager_N : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int combo = 0;
    public int score = 0;


    [Header("Frenzy Settings")]
    [SerializeField] private SpawnArrow_N frenzySwitch;
    [SerializeField] private int frenzyCondition = 5;
    [SerializeField] private float frenzyDuration = 10f; // NEW: How long frenzy lasts
    [SerializeField] private TextMeshProUGUI frenzyTimerText; // NEW: UI text for the timer
    
    private bool isFrenzyActive = false;



    private void Start()
    {
        // NEW: Ensure the timer text is hidden at the start
        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(false);
        }
    }

    public void ResetCombo() {
        score += combo;
        combo = 1;
        Debug.Log(" Reset score" + score.ToString());
        if (score >= frenzyCondition && !isFrenzyActive) {
            // Instead of just enabling, we now start the timer coroutine
            StartCoroutine(FrenzyCoroutine());
        }
    }
    public void IncreaseCombo() {
        combo ++;
        score += combo;
        Debug.Log("I score" + score.ToString());
        if (score >= frenzyCondition && !isFrenzyActive)
        {
            // Instead of just enabling, we now start the timer coroutine
            StartCoroutine(FrenzyCoroutine());
        }
    }


    private IEnumerator FrenzyCoroutine()
    {
        isFrenzyActive = true;

        // 1. Activate Frenzy Mode on the player's spawner
        frenzySwitch.EnableFrenzy();

        // 2. Start and display the UI timer (reusing the freeze timer pattern)
        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(true);
        }

        float timeLeft = frenzyDuration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (frenzyTimerText != null)
            {
                frenzyTimerText.text = "FRENZY: " + timeLeft.ToString("F1");
            }
            yield return null; // Wait for the next frame
        }

        // 3. Timer is up, deactivate everything
        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(false);
        }

        frenzySwitch.DisableFrenzy();

        // 4. Reset score and frenzy state for game balance
        score = 0; // Reset score so player has to earn it again
        isFrenzyActive = false;
    }

}
