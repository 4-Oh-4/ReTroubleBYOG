using UnityEngine;
using TMPro;
using System.Collections;

public class ComboCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboText;
    [Tooltip("How long the counter stays on screen after the last combo hit.")]
    [SerializeField] private float displayDuration = 4.0f; // You can set this to 4 or 5

    private Animator anim;
    private Coroutine disableCoroutine;
    private bool isDisplayed = false; // NEW: Flag to track if we are already visible

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

 
    public void ShowCombo(int comboCount)
    {
        // If the counter is not already on screen, activate it and play the animation.
        if (!isDisplayed)
        {
            gameObject.SetActive(true);
            isDisplayed = true;
            if (anim != null)
            {
                anim.Play("Combo_Appear_Anim", -1, 0f);
            }
        }

        // Always update the text
        comboText.text = "" + comboCount;

        // Always reset the disappear timer
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        disableCoroutine = StartCoroutine(DisableAfterTime());
    }

 
    public void Hide()
    {
        if (disableCoroutine != null)
        {
            StopCoroutine(disableCoroutine);
        }
        isDisplayed = false;
        gameObject.SetActive(false);
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(displayDuration);

        // Hide the counter and reset its state
        isDisplayed = false;
        gameObject.SetActive(false);
    }
}