using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Bubble : MonoBehaviour
{
    [Header("Bubble")]
    
    [SerializeField] private float horizontalAmplitude = 1f;
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalSpeed = 0.65f;
    [SerializeField] private float livingTime = 10f;

    [Space(5)]

    [SerializeField] private float scale = 5f;
    [SerializeField] private bool scaleIncreasing = false;

    [Space(5)]
    [Header("Random")]

    [SerializeField] private Vector2 floatingSpeed = new Vector2(1, 2);
    [SerializeField] private Vector2 increasingScaleSpeed = new Vector2(1, 2);

    [Space(3)]
    [SerializeField] private Vector2 damage = new Vector2(1, 2);


    [Header("Projectile")]
    [SerializeField] private bool isProjectile = false;
    [SerializeField] private float airFriction = 1f;
    [SerializeField] private float minSpeedStartFloating = 7f;

    private float startHorizontalOffset;
    private bool isBlowUp = false;
    private SpriteRenderer spriteRenderer;
    PlayerController manager;
    Rigidbody2D rb;

    bool isFacingRight;

    private void Start()
    {
        startHorizontalOffset = Random.Range(-5, 5);
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (spriteRenderer.drawMode != SpriteDrawMode.Sliced)
        {
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        // Set the initial texture size
        spriteRenderer.size = Vector2.one * scale;
        GetComponent<CircleCollider2D>().radius = 0.22f * scale;
    }

    public void SetupDirection(bool right)
    {
        isFacingRight = right;
    }

    private void Update()
    {
        Move();


        if (scaleIncreasing)
            transform.localScale += Vector3.one * Time.deltaTime * Random.Range(increasingScaleSpeed.x, increasingScaleSpeed.y);
        
        if (livingTime <= 0 && !isBlowUp)
            BubbleBlowUp();
    }

    private void Move()
    {
        float verticalMovement = 0f;
        float horizontalMovement = 0f;
        if (isBlowUp)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        if (isProjectile)
        {
            horizontalSpeed -= Time.deltaTime * airFriction;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, 0, 100);
        }
        else
        {
            livingTime -= Time.deltaTime;
            horizontalMovement = horizontalAmplitude * Mathf.Sin((Time.time + startHorizontalOffset));
        }

        if ((isProjectile && Mathf.Abs(horizontalSpeed) < minSpeedStartFloating) || !isProjectile)
        {
            if(isProjectile)
                livingTime -= Time.deltaTime;

            verticalMovement = Random.Range(floatingSpeed.x, floatingSpeed.y);
        }
        if (isFacingRight == true)
        {
            rb.velocity = -transform.right  * horizontalSpeed;

        }
        else
        {
            rb.velocity = transform.right   * horizontalSpeed;
        }
        transform.position += new Vector3(0, verticalMovement, 0) * Time.deltaTime * verticalSpeed;  

    }

    public void BubbleBlowUp()
    {
        if (!isBlowUp)
        {
            isBlowUp = true;

            Animator anim = GetComponent<Animator>();
            anim.Play("BubbleBlowUp");

            if(transform.childCount > 0)
                if (transform.GetChild(0).TryGetComponent<PlayerController>(out PlayerController pl))
                    pl.ChangeState(pl.locomotionState);
            
            transform.DetachChildren();
            Destroy(gameObject, 1f);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            BubbleBlowUp();
            return;
        }

        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.TakeDamage(Mathf.FloorToInt(Random.Range(damage.x, damage.y)),isProjectile);
            BubbleBlowUp();
            return;
        }
    }
}
