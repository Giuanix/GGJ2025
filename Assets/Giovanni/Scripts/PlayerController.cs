using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    public FloatingObject floatingObject;
    public Animator particles;
    [SerializeField] private float moveSpeed;
    private float sprintInstantSpeed = 25f;
    private float sprintDuration = 0.35f;
    private float actualSpeed;

    private float horizontalMovement;
    public bool isFacingRight = true;

    [Header("Jump Settings")]
    public float jumpForce;

    [Header("GroundCheck Settings")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f,0.05f);
    [SerializeField] private LayerMask grondLayer;

    [Header("Gravity Settings")]
    [SerializeField] private float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] private float fallSpeedMultiplier = 2f;
    [SerializeField] private float slipperySpeedMultiplier = 0.5f;
    [SerializeField] private float maxSlipperySpeed = 5f;
    [SerializeField] private float frictionSlippery = 1.5f;


    State currentState;
    [HideInInspector] public LocomotionState locomotionState;
    [HideInInspector] public JumpState jumpState;
    [HideInInspector] public WaterState waterState;
    [HideInInspector] public BubbleState bubbleState;
    [HideInInspector] public NullState nullState;
    public UI_Manager uiManager;
    bool inSprint = false;
    private float accumulatedVelocity;
    //Enum for sprint state.
    enum SprintState
    {
        WaitForFirstSprint,
        FirstSprintStarted,
        WaitForSecondSprint,
        WaitForEndSprint //Questo serve per prevenire un doppio scatto continuo. Il giocatore deve prima fermarsi per fare un altro scatto
    }

    SprintState sprintState = SprintState.WaitForFirstSprint;

    private void OnDestroy()
    {
        uiManager.DeactivateUI();
        KO.instance.AnimateKO();
        GetComponent<PlayerShoot>().SliderHide();
    }


    public void ChangeState(State newState, bool callOnEnter = true)
    {
        currentState?.OnExit();
        State oldState = currentState;
        currentState = newState;

        if (callOnEnter)
            currentState?.OnEnter(oldState);
    }
    public bool IsInState<T>()
    {
        return currentState is T;
    }


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        locomotionState = new LocomotionState(this);
        jumpState = new JumpState(this);
        waterState = new WaterState(this);
        bubbleState = new BubbleState(this);

        //uiManager.targetPlayer = transform;

        actualSpeed = moveSpeed;

        ChangeState(nullState);

    }

    public void Update()
    {
        currentState?.Update();

        if (floatingObject.IsInWater() && !IsInState<WaterState>() && !IsInState<BubbleState>())
        {
            ChangeState(waterState);
        }
    }

    public void Movement()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovement));

        if (IsSlippery())
        {

            // Accumulate velocity when there is input
            if (Mathf.Abs(horizontalMovement) >= 0.1f)
            {
                accumulatedVelocity += Time.deltaTime * 5f;
                accumulatedVelocity = Mathf.Clamp(accumulatedVelocity, 0, maxSlipperySpeed);
               
                rb.velocity = new Vector2(horizontalMovement * actualSpeed, rb.velocity.y);
            }
            else
            {
                accumulatedVelocity -= Time.deltaTime; // Gradually reduce velocity without input
                accumulatedVelocity = Mathf.Clamp(accumulatedVelocity, 0, maxSlipperySpeed);
                rb.velocity = new Vector2(
                   (Mathf.Abs(horizontalMovement) * actualSpeed * slipperySpeedMultiplier + (accumulatedVelocity / frictionSlippery)) * (isFacingRight ? -1 : 1),
                   rb.velocity.y);
            
            }
   
        }
        else
        {
            accumulatedVelocity = 0f;
            if(!inSprint)
                rb.velocity = new Vector2(horizontalMovement * actualSpeed, rb.velocity.y);
            else
                rb.velocity = new Vector2((isFacingRight ? -1f : 1f) * actualSpeed, rb.velocity.y);
        }

        Flip();
    }

    public void ResetSpeed()
    {
        actualSpeed = moveSpeed;
    }
    public void ReduceActualSpeedBy(float d)
    {
        actualSpeed /= d;
    }
    public void SetActualSpeed(float s)
    {
        actualSpeed = s;
    }
    //Inizio di Input
    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }


    public void Jump(InputAction.CallbackContext context)
    {
        currentState?.JumpCall(context);
    }

    public void AnyKey(InputAction.CallbackContext context)
    {
        currentState?.AnyKeyCall(context);
    }


    //Funzioni di Controllo
    public void Gravity()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }

        animator.SetBool("IsGrounded", IsGrounded());
        animator.SetFloat("yVelocity", rb.velocity.y);
    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer) && Mathf.Abs(rb.velocity.y) < 0.1f; //<-- avoid IsGrounded instantly when jumping!
    }

    public bool IsSlippery()
    {
        
        if(Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer))
            return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer).gameObject.tag == "Slippery";
        return false;
    }
    private void Flip()
    {
        if(isFacingRight && horizontalMovement > 0 || !isFacingRight && horizontalMovement < 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }



    int maxFrame = 30; // 1~ second, if game to 60fps half second
    int currentFrame = 0;
    float startDirection = 0f;
    Coroutine sprintCoroutine;

    public void HandleSprint()
    {

        switch (sprintState)
        {
            case SprintState.WaitForFirstSprint:
                
                if (Mathf.Abs(horizontalMovement) >= 1 && !inSprint)
                {
                    startDirection = horizontalMovement;
                    sprintState = SprintState.FirstSprintStarted;
                    currentFrame = 0;
                }

                break;

            case SprintState.FirstSprintStarted:
               
                currentFrame++;
                
                if (Mathf.Abs(horizontalMovement) < 1)
                {
                    sprintState = SprintState.WaitForSecondSprint;
                }
                
                else if (currentFrame > maxFrame)
                {
                    ResetSprint();
                }

                break;

            case SprintState.WaitForSecondSprint:

                currentFrame++;

                if (Mathf.Abs(horizontalMovement) >= 1)
                {
                    if(horizontalMovement != startDirection)
                    {
                        ResetSprint();
                        break;
                    }


                    if (sprintCoroutine != null)
                        StopCoroutine(sprintCoroutine);
                    sprintCoroutine = StartCoroutine(Sprint());

                    ResetSprint();
                    sprintState = SprintState.WaitForEndSprint;
                }
                
                else if (currentFrame > maxFrame)
                {
                    ResetSprint();
                }
                break;

            case SprintState.WaitForEndSprint:
         
                break;
        }
    }

    private void ResetSprint()
    {
        currentFrame = 0;
        sprintState = SprintState.WaitForFirstSprint;
    }
  
    private IEnumerator Sprint()
    {
        actualSpeed = sprintInstantSpeed;
        inSprint = true;
        
        float timer = 0;
        float spawnInterval = 0.05f; // Adjust to control trail density
        float lastSpawnTime = 0f;

        while (timer < sprintDuration)
        {
            timer += Time.deltaTime;
            actualSpeed = Mathf.Lerp(sprintInstantSpeed, moveSpeed, timer / sprintDuration);

            if (timer - lastSpawnTime >= spawnInterval)
            {
                CreateAfterImage();
                lastSpawnTime = timer;
            }

            yield return null;
        }
        
        inSprint = false;
        sprintState = SprintState.WaitForFirstSprint;
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position,groundCheckSize);
    }


}
