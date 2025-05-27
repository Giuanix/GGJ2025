using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassiveAbility : MonoBehaviour
{
    #region Variables
    public enum Ability
    {
        Dash,
        Dodge,
        Glide,
        Granate,
        None
    }

    [Header("General Ability Properties")]
    [SerializeField] private Ability ability;
    [SerializeField] private float abilityCooldown = 0.35f;
    PlayerController player;
    AudioManager audioManager;

    [Header("Dash and Dodge: Pina e Rita")]
    [SerializeField] private float sprintInstantSpeed = 25f;
    [SerializeField] private float sprintDuration = 0.35f;
    [SerializeField] private LayerMask ignoreLayerMask;
    bool canAbility = true;

    [Header("Glide: Mr. Gooseway")]
    [SerializeField] private float glideGravity = 0.4f;
    [SerializeField] private float glideFallSpeedMultiplier = 0.6f;

    [Header("Granate: Whalla")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float force = 7f;
    [SerializeField] private float upwardModifier = 0.7f;
    [SerializeField] private Transform whallaTransform;

    #endregion
    private void Start()
    {
        player = GetComponent<PlayerController>();
        audioManager = AudioManager.instance;
    }

    public void TriggerAbility(InputAction.CallbackContext context)
    {
        switch (ability)
        {
            case Ability.Dash:
                if (context.performed && canAbility)
                {
                    Debug.Log("DASH");
                    audioManager.PlayDashAndDodge();
                    StartSprint(false);
                    canAbility = false;
                }
                break;

            case Ability.Dodge:
                if (context.performed && canAbility)
                {
                    Debug.Log("DODGE");
                    audioManager.PlayDashAndDodge();
                    StartSprint(true);
                    canAbility = false;
                }
                break;

            case Ability.Glide:
                if (context.started)
                {
                    Debug.Log("GLIDE");
                    StartGlide();
                }
                else if (context.canceled)
                {
                    Debug.Log("STOP GLIDE");
                    StopGlide();
                }
                break;

            case Ability.Granate:
                if (context.performed && canAbility)
                {
                    Debug.Log("GRANATE");
                    StartCoroutine(LaunchGrenade());
                    canAbility = false;
                }
                break;
        }
    }


    /// <summary>
    /// DASH
    /// </summary>

    #region Dash and Dodge
    Coroutine sprintCoroutine;
    public void StartSprint(bool ignoreProjectile)
    {
        sprintCoroutine = StartCoroutine(Sprint(ignoreProjectile));
    }

    private IEnumerator Sprint(bool ignoreProjectile)
    {
        player.SetActualSpeed(sprintInstantSpeed);
        player.inSprint = true;

        if (ignoreProjectile)
            player.rb.excludeLayers = ignoreLayerMask;

        float timer = 0;
        float spawnInterval = 0.05f; // Adjust to control trail density
        float lastSpawnTime = 0f;

        while (timer < sprintDuration)
        {
            timer += Time.deltaTime;
            player.SetActualSpeed(Mathf.Lerp(sprintInstantSpeed, player.GetDefaultSpeed(), timer / sprintDuration));

            if (timer - lastSpawnTime >= spawnInterval)
            {
                StartCoroutine(FadeAndDestroy());
                lastSpawnTime = timer;
            }

            yield return null;
        }

        player.inSprint = false;

        player.rb.excludeLayers = 0;

        yield return new WaitForSeconds(abilityCooldown);
        canAbility = true;
    }

    #endregion

    #region Glide
    public void StartGlide()
    {
        player.baseGravity = glideGravity;
        player.fallSpeedMultiplier = glideFallSpeedMultiplier;
    }

    public void StopGlide()
    {
        player.baseGravity = 3f;
        player.fallSpeedMultiplier = 3f;
    }
    #endregion

    #region Granata
    IEnumerator LaunchGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, spawnPoint.position, Quaternion.identity);

        float directionX = whallaTransform.localScale.x > 0 ? -1f : 1f;
        Vector2 direction = new Vector2(directionX, upwardModifier).normalized;

        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(abilityCooldown);
        canAbility = true;
    }

    #endregion

    #region Fade and Destroy
    private IEnumerator FadeAndDestroy()
    {
        //Creating Image
        GameObject afterImage = new GameObject("AfterImage");
        afterImage.transform.position = transform.position;
        afterImage.transform.localScale = transform.localScale;

        SpriteRenderer spriteRenderer = afterImage.AddComponent<SpriteRenderer>();
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = playerSprite.sprite;
        spriteRenderer.sortingLayerID = playerSprite.sortingLayerID;
        spriteRenderer.sortingOrder = playerSprite.sortingOrder - 1;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.8f); // Initial alpha


        //Fade
        float fadeDuration = 0.3f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(afterImage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.inSprint && ability == Ability.Dash)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 4f, ForceMode2D.Impulse);
            if (collision.gameObject.GetComponent<PlayerController>().isFacingRight != player.isFacingRight)
            {
                collision.gameObject.GetComponent<PlayerController>().Flip(true);
            }
            //Stop sprint

            if (sprintCoroutine != null)
            {
                StopCoroutine(sprintCoroutine);
            }
            player.ResetSpeed();
            player.StopMovement();
            player.inSprint = false;
            canAbility = true;

            collision.gameObject.GetComponent<PassiveAbility>().StartSprint(false);
        }
    }
        
    #endregion

    /// <summary>
    /// END DASH
    /// </summary>
}
