using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PassiveAbility : MonoBehaviour
{
    public enum Ability
    {
        Dash
    }

    [SerializeField] private Ability ability;
    PlayerController player;
    private float sprintInstantSpeed = 25f;
    private float sprintDuration = 0.35f;
    private float cooldown = 0.35f;

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
                    StartSprint();
                break;
        }
    }


    /// <summary>
    /// DASH
    /// </summary>
    Coroutine sprintCoroutine;

    public void StartSprint()
    {
        if (sprintCoroutine != null)
            StopCoroutine(sprintCoroutine);
        sprintCoroutine = StartCoroutine(Sprint());
        canAbility = false;
    }

    public void StopSprint()
    {
        if (sprintCoroutine != null)
        {
            StopCoroutine(sprintCoroutine);
        }
        player.ResetSpeed();
        player.StopMovement();
        player.inSprint = false;
    }

    private IEnumerator Sprint()
    {
        player.SetActualSpeed(sprintInstantSpeed);
        player.inSprint = true;

        float timer = 0;
        float spawnInterval = 0.05f; // Adjust to control trail density
        float lastSpawnTime = 0f;

        while (timer < sprintDuration)
        {
            timer += Time.deltaTime;
            player.SetActualSpeed(Mathf.Lerp(sprintInstantSpeed, player.GetDefaultSpeed(), timer / sprintDuration));

            if (timer - lastSpawnTime >= spawnInterval)
            {
                CreateAfterImage();
                lastSpawnTime = timer;
            }

            yield return null;
        }
        timer = 0;
        player.inSprint = false;
        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        canAbility = true;
    }

    private void CreateAfterImage()
    {
        GameObject afterImage = new GameObject("AfterImage");
        afterImage.transform.position = transform.position;
        afterImage.transform.localScale = transform.localScale;

        SpriteRenderer spriteRenderer = afterImage.AddComponent<SpriteRenderer>();
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = playerSprite.sprite;
        spriteRenderer.sortingLayerID = playerSprite.sortingLayerID;
        spriteRenderer.sortingOrder = playerSprite.sortingOrder - 1;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.8f); // Initial alpha

        StartCoroutine(FadeAndDestroy(afterImage, spriteRenderer));
    }

    private IEnumerator FadeAndDestroy(GameObject afterImage, SpriteRenderer sr)
    {
        float fadeDuration = 0.3f;
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
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
            StopSprint();
            collision.gameObject.GetComponent<PassiveAbility>().StartSprint();
        }
    }
    /// <summary>
    /// END DASH
    /// </summary>
}
