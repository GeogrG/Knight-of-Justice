using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Horse : MonoBehaviour
{
    [Header("Hit Effect settings")]
    [Tooltip("Material to switch to during the flash.")]
    [SerializeField] private Material flashMaterial;
    [Tooltip("Duration of the flash.")]
    [SerializeField] private float duration;

    // The SpriteRenderer that should flash.
    private SpriteRenderer spriteRenderer;

    // The material that was in use, when the script started.
    private Material originalMaterial;

    // The currently running coroutine.
    private Coroutine flashRoutine;

    [Header("Main Mechanic settings")]
    [Tooltip("Main Mechanic settings")]
    [SerializeField] Slider slider;
    [SerializeField] Image img;
    [SerializeField] SliderAnim sliderAnim;
    [SerializeField] float period;
    [SerializeField] Animator anim;
    [SerializeField] float minSpeed = 0.19f;
    [SerializeField] float maxSpeed = 0.81f;
    [SerializeField] float offsetForSinusoicMovement;
    [SerializeField] float imageOffsetOnXAxis;
    public Instantiate[] spawners;
    public float sliderValue;

    //Slider for the Shield's properties
    [Header("Shield Slider Properties")]
    [Tooltip("Slider prefab, Speed of wasting necklace's resouses")]
    [SerializeField] Slider shieldSlider;
    [SerializeField] float shieldWastingSpeed;

    //Private fields important for the gameplay
    private bool sliderActive = true;
    private bool shielded;
    private Vector2 imgPos; 

    [Header("Heart")]
    [Tooltip("Referent UI Object.")]
    [SerializeField] private GameObject hpSprite;
    int numberOfHits = 0;

    [Header("Shield")]
    [Tooltip("Referent GameObject.")]
    [SerializeField] private GameObject shield;

    [Header("Death Menu")]
    [Tooltip("Referent UI Oject for Death Menu.")]
    [SerializeField] private GameObject deathMenu;

    [Header("Attacking things")]
    [Tooltip("Referent object for a spear attack.")]
    [SerializeField] private GameObject attackPoint;
    [Tooltip("Attacking Radius.")]
    [SerializeField] private float attackingRadius; 

    void Start()
    {
        slider.value = 0;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        shielded = false;
    }

    void Update()
    {
        Attacking();
        FlipFloping();
        SliderMeth();
        Jumping();
        Defending();
    }

    private void Defending()
    {
        if (Input.GetMouseButton(1))
        {

            if (!sliderActive && shieldSlider.value > 0)
            {
                shield.SetActive(true);
                shieldSlider.gameObject.SetActive(true);
                shieldSlider.value -= shieldWastingSpeed * Time.deltaTime;
                shielded = true;
            }
            else if(sliderActive)
            {
                shield.SetActive(true);
            }
            else
            {
                shield.SetActive(false);
                shielded = false;
            }

        }
        else
        {
            shield.SetActive(false);
            shieldSlider.gameObject.SetActive(false);
            shielded = false;
            shieldSlider.value = 1;
        }
    }

    private void Attacking()
    {
        if (Input.GetMouseButtonDown(0) && !sliderActive)
        {
            anim.SetTrigger("Attacking");
            Collider2D enemyCol = Physics2D.OverlapCircle(attackPoint.transform.position, attackingRadius);
            if (enemyCol != null && enemyCol.tag == "Mage")
            {
                Debug.Log("Mage is Dead");
                enemyCol.GetComponent<Animator>().SetTrigger("MageDeath");
            }
        }
    }

    private void Jumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("Jumping");
        }
    }

    private void SliderMeth()
    {
        const float tau = Mathf.PI * 2;
        float cycles = Time.time / period;
        float rawSinCycle = Mathf.Sin(cycles * tau);
        slider.value = (rawSinCycle + 1) / 2f;
        //Debug.Log(slider.value);
        sliderAnim.ChangePictureInUISlider(slider.value);
        imgPos = new Vector2(this.gameObject.transform.position.x + imageOffsetOnXAxis,
            this.transform.position.y + offsetForSinusoicMovement * rawSinCycle);
        img.transform.position = imgPos;
        if (Input.GetMouseButtonDown(0) && sliderActive)
        {
            sliderAnim.HideSlider();
            FindObjectOfType<BackgroundMover>().moveSpeed = slider.value * maxSpeed + minSpeed;
            anim.SetFloat("Speed", 1 + slider.value * 0.3f); //если больше 0.5 то скорость анимки уменьшается, иначе - увеличивается
            Debug.Log(1 + ((slider.value - 0.5f) * 1.5f));
            FindObjectOfType<Timer>().sliderValue = slider.value;
            FindObjectOfType<Timer>().CalculateTime();
            FindObjectOfType<Timer>().isLevelStarted = true;
            FindObjectOfType<Instantiate>().howMuchToWait = slider.value;
            FindObjectOfType<Instantiate>().isCoroutineReady = true;
            FindObjectOfType<Instantiate>().CoroutineStarter();
            slider.gameObject.SetActive(false);
            sliderActive = false;
            //Debug.Log("Slider value is = " + slider.value);
        }

    }

    private void FlipFloping()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && transform.position.y == -1.7f)
        {
            transform.position = new Vector2(transform.position.x, -4);
        }
        if (Input.GetAxisRaw("Vertical") > 0 && transform.position.y == -4f)
        {
            transform.position = new Vector2(transform.position.x, -1.7f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // If the flashRoutine is not null, then it is currently running.
        if (flashRoutine != null)
        {
            // In this case, we should stop it first.
            // Multiple FlashRoutines the same time would cause bugs.
            StopCoroutine(flashRoutine);
        }
        if (!shielded || col.tag != "ShootingItems")
        {
            // Start the Coroutine, and store the reference for it.
            flashRoutine = StartCoroutine(FlashRoutine());
            // Decrease the HP and start Heart animation. 

            numberOfHits++;
            ChangeHeartAnim(numberOfHits);
        }
        if (col.GetComponent<ObstacleMover>() == null) { return; }
        col.GetComponent<ObstacleMover>().Die();
    }

    private IEnumerator FlashRoutine()
    {
        // Swap to the flashMaterial.
        spriteRenderer.material = flashMaterial;

        // Pause the execution of this function for "duration" seconds.
        yield return new WaitForSeconds(duration);

        // After the pause, swap back to the original material.
        spriteRenderer.material = originalMaterial;

        // Set the routine to null, signaling that it's finished.
        flashRoutine = null;
    }

    private void ChangeHeartAnim(int howManyHits)
    {
        Animator anim;
        if (howManyHits <= 3)
        {
            if (howManyHits == 1)
            {   //Helmet Anim is playing
                anim = hpSprite.transform.GetChild(3 - howManyHits).gameObject.GetComponent<Animator>();
                anim.SetTrigger("IsDamageGiven");
            }
            else if (howManyHits == 2)
            {   //Shield&Sword Anim is playing
                anim = hpSprite.transform.GetChild(3 - howManyHits).gameObject.GetComponent<Animator>();
                anim.SetTrigger("IsDamageGiven");
            }
            else
            {   //Heart Anim is playing and player is dead. 
                anim = hpSprite.transform.GetChild(3 - howManyHits).gameObject.GetComponent<Animator>();
                anim.SetTrigger("IsDamageGiven");
                //0.7 is the lenght of "Heart's" animation to play till the end.
                Invoke("OpenDeathMenu", .7f);
            }
        }
    }
    private void OpenDeathMenu()
    {
        deathMenu.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackingRadius);
    }
}