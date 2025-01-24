using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
public class PlayerShoot : MonoBehaviour
{
    [Header("Shared Player Shoot var")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform BulletSpawnPoint;
    [SerializeField] private float shotDelay;
    [Header("Only Duck")]
    [SerializeField] private int shotMaxCounter = 4;

    [Header("Only Whale")]
    [SerializeField] private float singleShotDelay = 0.25f;

    [Space(5)]
    [Header("Reload Bar")]
    private Transform canvas;

    [SerializeField] private Vector3 offset = new Vector3(0,1,0);
    [SerializeField] private Color fillColor;
    [SerializeField] private ShotType type;
   
    private float shotTimeCounter;
    private float extraDamage = 0f;
    private float chargedShotExtraDamage = 0f;
    private float extraSize = 0f;


    private float chargeTime = 0f;
    [SerializeField] private float maxChargeTime = 2f; 
    private bool isCharging = false;


    [SerializeField] private GameObject sliderPrefab;
    Slider slider;
    private PlayerController pl;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public AudioManager managerAudio;
    enum ShotType
    {
        MultipleShot,
        SingleShot
    }

    private void Start()
    {
        managerAudio = AudioManager.instance;
        shotTimeCounter = shotDelay;
        canvas = FindObjectOfType<Canvas>().transform;
        pl = GetComponent<PlayerController>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isCharging)
        {
            chargeTime += Time.deltaTime;

            if(chargeTime > maxChargeTime)
            {
                spriteRenderer.color = Color.yellow;
            }
        }
 

        shotTimeCounter += Time.deltaTime;
        if(shotTimeCounter < shotDelay)
        {
            if (!slider) {
                slider = Instantiate(sliderPrefab,canvas).GetComponent<Slider>();
                slider.maxValue = shotDelay;
                slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = fillColor;
            }
            slider.value = shotTimeCounter;

           
        }
        
        else if (slider && shotTimeCounter > shotDelay + 0.5f)
        {
            Destroy(slider.gameObject);
            slider = null;
        }

        if (slider)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
            slider.transform.position = screenPos;
        }
    }
    public void Shoot(InputAction.CallbackContext context)
    {

        if (context.started && Time.timeScale > 0)
        {
            isCharging = true;
            chargeTime = 0f;
        }

        if (context.performed && shotTimeCounter > shotDelay && Time.timeScale > 0)
        {
            if (!pl.IsInState<BubbleState>())
            {
                shotTimeCounter = 0.0f;
             
                pl.animator.SetTrigger("Shoot");

                if (chargeTime >= maxChargeTime)
                {
                    extraDamage += maxChargeTime*2;
                    extraSize += maxChargeTime;
                    spriteRenderer.color = Color.white;
                }
               
                    // Perform Normal Shot
                switch (type)
                {
                    case ShotType.MultipleShot:
                        StartCoroutine("Raffica");
                        break;
                    case ShotType.SingleShot:
                        StartCoroutine("SingleShot");
                        break;
                }
                
                isCharging = false;
                chargeTime = 0f;
            }
        }

        if (context.canceled)
        {
            isCharging = false;
            chargeTime = 0f;
        }
    }
    
    public void SpawnBubble()
    {
        Bubble b = Instantiate(bullet, BulletSpawnPoint.position, transform.rotation).GetComponent<Bubble>();
        b.SetupProjectileOwner(gameObject);
        b.SetupDirection(pl.isFacingRight);
        b.damage += Vector2.one * extraDamage + Vector2.one * chargedShotExtraDamage;
        b.scale += extraSize;
    }
    
    IEnumerator Raffica()
    {
        managerAudio.PlaySparoPapera();
        for (int i = 0; i < shotMaxCounter; i++)
        {
            SpawnBubble();
            yield return new WaitForSeconds(0.1f);
        }
        chargedShotExtraDamage = 0;
        extraSize = 0;
    }

    IEnumerator SingleShot()
    {
        managerAudio.PlaySparoBalena();
        yield return new WaitForSeconds(singleShotDelay);
        SpawnBubble();
        chargedShotExtraDamage = 0;
        extraSize = 0;
    }

    IEnumerator PowerUp(float duration,float delayReducer)
    {
        float elapsedTime = 0f;
        float colorTime = 0f;


        if (spriteRenderer)
        {
            singleShotDelay /= delayReducer;
           
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.HSVToRGB(colorTime, 1f, 1f);
                
                colorTime += Time.deltaTime*2f;
                if (colorTime >= 1) colorTime = 0;


                elapsedTime += Time.deltaTime;

                yield return null;
            }
            spriteRenderer.color = Color.white;
            pl.uiManager.SetupPowerUpImage(null);

            singleShotDelay *= delayReducer;
        }

    }
    IEnumerator PowerUpDamage(float duration)
    {
        float elapsedTime = 0f;
        if (spriteRenderer)
        {
            while (elapsedTime < duration)
            {
                spriteRenderer.color = Color.red;

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            spriteRenderer.color = Color.white;
            pl.uiManager.SetupPowerUpImage(null);
            extraDamage = 0f;
        }

    }

    public void StarPowerup(float duration, float delayReducer,Sprite icon)
    {
        StartCoroutine(PowerUp(duration, delayReducer));
        pl.uiManager.SetupPowerUpImage(icon);
    }
    public void SetDamage(float duration, int damage, Sprite icon)
    {
        StartCoroutine(PowerUpDamage(duration));
        pl.uiManager.SetupPowerUpImage(icon);
        extraDamage = damage;
    }


}
