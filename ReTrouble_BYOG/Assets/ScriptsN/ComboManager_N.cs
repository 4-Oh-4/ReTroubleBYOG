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
    [SerializeField] bool canFrenzy = true;
    private bool isFrenzyActive = false;
    [SerializeField] GameObject[] frenzymeter;
    [Header("UI Reference")]
    [SerializeField] private ComboCounterUI comboCounterUI;
    private int frenzyIndex = 0;
    [Header("Combo Settings")]
    [Tooltip("How long (in seconds) the player has to hit another bubble before the combo resets.")]
    [SerializeField] private float comboResetDelay = 2.0f;
    private Coroutine resetCoroutine;



    private void Start()
    {
        // NEW: Ensure the timer text is hidden at the start
        if (frenzyTimerText != null)
        {
            frenzyTimerText.gameObject.SetActive(false);
        }

        if (comboCounterUI != null)
        {
            comboCounterUI.gameObject.SetActive(false);
        }
    }

    public void ResetCombo() {
        
        combo = 0;

        if (comboCounterUI != null)
        {
            comboCounterUI.gameObject.SetActive(false);
        }

        Debug.Log(" Reset score" + score.ToString());
        if (score >= frenzyCondition && !isFrenzyActive) {
            // Instead of just enabling, we now start the timer coroutine
            if (canFrenzy) StartCoroutine(FrenzyCoroutine());
            else return;
        }
    }
    public void IncreaseCombo() {

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }

        combo += 10;
        score += combo;

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AddScore(combo);
        }


        Debug.Log("Combo: " + combo + " | Score: " + score);
        Debug.Log("I score" + score.ToString());

        if (combo >= 2 && comboCounterUI != null) // Show and update the combo counter UI.
        {
            comboCounterUI.ShowCombo(combo);
            if(frenzyIndex<=4)frenzymeter[frenzyIndex].SetActive(true);
            frenzyIndex++;
        }

        resetCoroutine = StartCoroutine(ResetComboAfterDelay()); // // Start a new timer to reset the combo after a delay


        if (frenzyIndex >= 5 && !isFrenzyActive)
        {
            if (canFrenzy) StartCoroutine(FrenzyCoroutine());
            else return;
        }
    }


    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboResetDelay);

        // If we reach this point, the player waited too long. Reset everything.
        combo = 0;

        if (comboCounterUI != null)
        {
            comboCounterUI.Hide();
        }
        Debug.Log("Combo Reset due to timeout.");
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
        for (int i = 0; i< 5; i++) {
            frenzymeter[i].SetActive(false);
        }
        frenzyIndex = 0;
        // 4. Reset score and frenzy state for game balance
        score = 0; // Reset score so player has to earn it again
        isFrenzyActive = false;
    }

}
