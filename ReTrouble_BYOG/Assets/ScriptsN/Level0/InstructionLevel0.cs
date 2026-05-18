using UnityEngine;
using Unity.Collections;
using System.Collections;

public class InstructionLevel0 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject movementImg;
    [SerializeField] GameObject shootDone;
    [SerializeField] GameObject freezeDone;
    [SerializeField] GameObject shieldDone;
    [SerializeField] GameObject FreezePowerUp;
    [SerializeField] GameObject SheildPowerUp;
    [SerializeField] PowerManger_N powerManger;
    [SerializeField] GameObject Cage;
    bool once = true;
    bool onceShield = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (powerManger.freezeBool && once) {
            once = false;
            FreezeDone();
        }
        if (powerManger.ShieldBool && onceShield) {
            onceShield = false;
            ShieldDone();
        }
    }
    public void movementDone() {
        movementImg.SetActive(true);
        if (movementImg.activeInHierarchy && shootDone.activeInHierarchy) {
            StartCoroutine(SpawnFreeze(FreezePowerUp));
        }
    }
    public void ShootDone() {
        if (!once) return;
        shootDone.SetActive(true);
        if (movementImg.activeInHierarchy && shootDone.activeInHierarchy) {
            StartCoroutine(SpawnFreeze(FreezePowerUp));
        }
    }
    public void FreezeDone() {
        StartCoroutine(SpawnFreeze(SheildPowerUp,true));

        freezeDone.SetActive(true);
        
    }
    public void ShieldDone() {
        StartCoroutine(DeactivateCage());
    }
    IEnumerator SpawnFreeze(GameObject obj,bool shield=false) {
        if (!shield) yield return new WaitForSecondsRealtime(3f);
        else yield return new WaitForSecondsRealtime(6f);
        if(obj!=null)obj.SetActive(true);
    }
    IEnumerator DeactivateCage() {
        yield return new WaitForSecondsRealtime(1.5f);
        shieldDone.SetActive(true);
        Cage.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
