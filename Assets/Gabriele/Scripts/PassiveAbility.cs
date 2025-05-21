using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassiveAbility : MonoBehaviour
{
    public enum Ability
    {
        Dash,
        None
    }

    [Header("General Ability Properties")]
    [SerializeField] private Ability ability;
    [SerializeField] private float abilityCooldown = 0.35f; 
    PlayerController player;
    [Header("Dash")]
    [SerializeField] private float sprintInstantSpeed = 25f;
    [SerializeField] private float sprintDuration = 0.35f;
    [SerializeField] private LayerMask projectileLayer;


    bool canAbility = true;
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void TriggerAbility(InputAction.CallbackContext context)
    {
        switch (ability)
        {
            case Ability.Dash:
                if (canAbility)
                {
                    StartSprint(true);
                    canAbility = false;
                }
                break;
        }
    }


    /// <summary>
    /// DASH
    /// </summary>
    Coroutine sprintCoroutine;

    public void StartSprint(bool ignoreProjectile)
    {
        sprintCoroutine = StartCoroutine(Sprint(ignoreProjectile));
    }


    private IEnumerator Sprint(bool ignoreProjectile)
    {
        player.SetActualSpeed(sprintInstantSpeed);
        player.inSprint = true;
       
        if(ignoreProjectile)
            player.rb.excludeLayers = projectileLayer;
        
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
        if (collision.gameObject.CompareTag("Player") && player.inSprint)
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
    /// <summary>
    /// END DASH
    /// </summary>
}
