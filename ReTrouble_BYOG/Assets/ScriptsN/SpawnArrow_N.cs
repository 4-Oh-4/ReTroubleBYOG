using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnArrow_N : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject shootEffectPrefab; // FOOSH EFFECT
    public bool canSpawn = true;
    [SerializeField] float arrowSpeed=8f;
    [SerializeField]private int index = 0;
    private Color[] colorArray = { Color.red ,Color.yellow, Color.blue,Color.white};
    public bool frenzy = false;
    [SerializeField] PlayerController_D playerController;
    [SerializeField] GameObject picture;
    // --- Reference to other components ---
    private Animator anim;

    [SerializeField] Material[] materials;
    [SerializeField] Sprite[] arrowsprites;

    AudioManager_A audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager_A>();

        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController_D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        gameObject.GetComponent<SpriteRenderer>().material = materials[index];
        picture.GetComponent<SpriteRenderer>().material = materials[index];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnArrow(InputAction.CallbackContext context) {
        
        if (canSpawn && context.phase==InputActionPhase.Performed) {

            playerController.LockMovement();
            audioManager.PlaySFX(audioManager.shoot);

            anim.SetTrigger("Shoot");

        }
    }

    // NEW: This public function will be called by an Animation Event
    public void FireArrowFromAnimation()
    {
        if (canSpawn)
        {
            canSpawn = false;



            GameObject arrow = Instantiate(arrowPrefab);

            arrow.transform.position = transform.position; // Adjusted spawn point
            arrow.transform.localScale = new Vector3(1, 1, 1);
            arrow.GetComponentInChildren<SpriteRenderer>().sprite = arrowsprites[index];
            arrow.GetComponent<ArrowDestroy>().ColorIndex = index;
            arrow.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, arrowSpeed);

            if (shootEffectPrefab != null)
            {
                // Position it slightly above the player, or wherever you want it to appear
                Vector3 effectSpawnPosition = transform.position + new Vector3(0, 1f, 0);
                Instantiate(shootEffectPrefab, effectSpawnPosition, Quaternion.identity);
            }
        }
    }
    public void ChangeColorPositive(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed && !frenzy) {
            index = (index + 1) % 3;
            //gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
            gameObject.GetComponent<SpriteRenderer>().material = materials[index];
            picture.GetComponent<SpriteRenderer>().material = materials[index];

        }
    }
    public void ChangeColorNegative(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed && !frenzy) {
            index = (index - 1+3) % 3;
            //gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
            gameObject.GetComponent<SpriteRenderer>().material = materials[index];
            picture.GetComponent<SpriteRenderer>().material = materials[index];

        }
    }
    public void EnableFrenzy() {
        frenzy = true;
        index = 3;
        //gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
        gameObject.GetComponent<SpriteRenderer>().material = materials[index];
        picture.GetComponent<SpriteRenderer>().material = materials[index];

    }
    public void DisableFrenzy() {
        frenzy = false;
        index = Random.Range(0,3);
        //gameObject.GetComponent<SpriteRenderer>().color = colorArray[index];
        gameObject.GetComponent<SpriteRenderer>().material = materials[index];
        picture.GetComponent<SpriteRenderer>().material = materials[index];

    }
}
