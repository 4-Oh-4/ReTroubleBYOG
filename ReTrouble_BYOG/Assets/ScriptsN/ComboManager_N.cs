using UnityEngine;

public class ComboManager_N : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int combo = 0;
    public int score = 0;
    [SerializeField] SpawnArrow_N frenzySwitch;
    [SerializeField] int frenzyCondition = 5;
    public void ResetCombo() {
        score += combo;
        combo = 0;
        Debug.Log(" Reset score" + score.ToString());

    }
    public void IncreaseCombo() {
        combo ++;
        score += combo;
        Debug.Log("I score" + score.ToString());
        if (score >= frenzyCondition) frenzySwitch.EnableFrenzy();
    }

}
