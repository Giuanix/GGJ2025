using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class IncapsulateBubble : MonoBehaviour
{
    [Header("Bubble")]
    public AudioManager managerAudio;
    [SerializeField] private float horizontalAmplitude = 1f;
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalSpeed = 0.65f;
    [SerializeField] private float livingTime = 10f;
    [SerializeField] private float pressKeyAmount = 0.4f;

    [Space(5)]

    [SerializeField] private float scale = 5f;

    [Space(5)]
    [Header("Random")]

    [SerializeField] private Vector2 floatingSpeed = new Vector2(1, 2);


    private float startHorizontalOffset;
    private bool isBlowUp = false;
    private SpriteRenderer spriteRenderer;
    PlayerController incapsulatedPlayer;
    Rigidbody2D rb;


    private void Start()
    {
        managerAudio = AudioManager.instance;
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

    private void Update()
    {
        Move();

        if (livingTime <= 0 && !isBlowUp)
            BubbleBlowUp(false);
    }

    public void TryToBeFree()
    {
        livingTime -= pressKeyAmount;
    }

    private void Move()
    {
        float verticalMovement = 0f;
        float horizontalMovement = 0f;

  
        livingTime -= Time.deltaTime;
        horizontalMovement = horizontalAmplitude * Mathf.Sin((Time.time + startHorizontalOffset));

        

        verticalMovement = Random.Range(floatingSpeed.x, floatingSpeed.y);
        
        
        rb.velocity = transform.right * horizontalSpeed;
        
        transform.position += new Vector3(0, verticalMovement, 0) * Time.deltaTime * verticalSpeed;

    }

    public void BubbleBlowUp(bool kill)
    {
        if (!isBlowUp)
        {
            isBlowUp = true;

            Animator anim = GetComponent<Animator>();
            anim.Play("BubbleBlowUp");
           
            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).TryGetComponent<PlayerController>(out PlayerController pl))
                {
                    if (kill)
                    {
                        managerAudio.PlayPlayerSconfitto();
                        Destroy(pl.gameObject);
                    }
                    else
                    {
                        pl.ChangeState(pl.locomotionState);
                        if(pl.TryGetComponent<BubbleCounter>(out BubbleCounter b))
                        {
                            b.OverridePercentage(75);
                        }
                        else
                        {
                            Destroy(pl.gameObject);
                        }
                    }
                }
            }


            transform.DetachChildren();
            Destroy(gameObject, 1f);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController pl))
        {
            if(pl != incapsulatedPlayer)
                BubbleBlowUp(true);
        }
    }
}
