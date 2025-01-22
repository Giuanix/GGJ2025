using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Bubble : MonoBehaviour
{
    [Header("Bubble")]
    
    [SerializeField] private float horizontalAmplitude = 1f;
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalSpeed = 0.65f;
    [SerializeField] private float livingTime = 10f;
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

    private float startHorizontalOffset;
    private float decreaseSpeed = 1f;
    private bool isBlowUp = false;
    private SpriteRenderer spriteRenderer;

    public void SetupDirection(bool right)
    {
        horizontalSpeed *= (right ? 1 : -1);
    }

    private void Start()
    {
        startHorizontalOffset = Random.Range(-5, 5);
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.drawMode != SpriteDrawMode.Sliced)
        {
            spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        }

        // Set the initial texture size
        spriteRenderer.size = Vector2.one * scale;
        GetComponent<CircleCollider2D>().radius = 0.22f * scale;
    }
    private void Update()
    {

        Move();

        decreaseSpeed += Time.deltaTime * airFriction;
        livingTime -= Time.deltaTime;
        if (scaleIncreasing)
            transform.localScale += Vector3.one * Time.deltaTime * Random.Range(increasingScaleSpeed.x, increasingScaleSpeed.y);
     
        
        if (livingTime <= 0 && !isBlowUp)
        {
            BubbleBlowUp();
        }
    }

    private void Move()
    {
        float verticalMovement = 0f;
        float horizontalMovement = 0f;

        if (isProjectile)
        {
            horizontalMovement = horizontalSpeed * Time.deltaTime;  // Applying speed over time
        }
        else
        {
            horizontalMovement = horizontalAmplitude * Mathf.Sin((Time.time + startHorizontalOffset));
        }

        if ((isProjectile && Mathf.Abs(horizontalMovement) < 1f) || !isProjectile)
        {
            verticalMovement = Random.Range(floatingSpeed.x, floatingSpeed.y);
        }

        transform.position += new Vector3(horizontalMovement, 0, 0) * Time.deltaTime * horizontalSpeed;  
        transform.position += new Vector3(0, verticalMovement, 0) * Time.deltaTime * verticalSpeed;  

    }

    public void BubbleBlowUp()
    {
        if (!isBlowUp)
        {
            isBlowUp = true;

            Animator anim = GetComponent<Animator>();
            anim.Play("BubbleBlowUp");


            if (transform.GetChild(0).TryGetComponent<PlayerController>(out PlayerController pl))
                pl.ChangeState(pl.locomotionState);
            transform.DetachChildren();

            Destroy(gameObject, 1f);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (MonoBehaviour script in collision.GetComponents<MonoBehaviour>())
        {
            if (script is IDamageable damageable)
            {
                damageable.TakeDamage(Mathf.FloorToInt(Random.Range(damage.x,damage.y)));
                BubbleBlowUp();
                break;
            }
        }
    }
}
