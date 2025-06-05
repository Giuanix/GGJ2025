using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerShoot playerShoot;

    [Header("Movement Settings")]
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    public FloatingObject floatingObject;
    public Animator particles;
    [SerializeField] private float moveSpeed;
    private float actualSpeed;

    private float horizontalMovement;
    public bool isFacingRight = true;

    [Header("Jump Settings")]
    public float jumpForce;

    [Header("GroundCheck Settings")]
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    [SerializeField] private LayerMask grondLayer;

    [Header("Gravity Settings")]
    [SerializeField] public float baseGravity = 2f;
    [SerializeField] private float maxFallSpeed = 18f;
    [SerializeField] public float fallSpeedMultiplier = 2f;
    [SerializeField] private float slipperySpeedMultiplier = 0.5f;
    [SerializeField] private float maxSlipperySpeed = 5f;
    [SerializeField] private float frictionSlippery = 1.5f;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    State currentState;
    [HideInInspector] public LocomotionState locomotionState;
    [HideInInspector] public JumpState jumpState;
    [HideInInspector] public WaterState waterState;
    [HideInInspector] public BubbleState bubbleState;
    [HideInInspector] public NullState nullState;
    public UI_Manager uiManager;
    [HideInInspector] public bool inSprint = false;
    private float accumulatedVelocity;
    [HideInInspector] public bool isStunned = false;
    private float stunDuration = 0.5f;

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
        playerShoot = GetComponent<PlayerShoot>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        if (IsGrounded() == true)
        {
            baseGravity = 3f;
            fallSpeedMultiplier = 3f;
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
                rb.velocity = new Vector2((Mathf.Abs(horizontalMovement) * actualSpeed * slipperySpeedMultiplier + (accumulatedVelocity / frictionSlippery)) * (isFacingRight ? -1 : 1), rb.velocity.y);
            }
        }
        else
        {
            accumulatedVelocity = 0f;
            if (!inSprint)
            {
                rb.velocity = new Vector2(horizontalMovement * actualSpeed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2((isFacingRight ? -1f : 1f) * actualSpeed, rb.velocity.y);
            }

            if (isStunned)
            {
                horizontalMovement = 0f;
                StartCoroutine(Stun());
                StartCoroutine(Blink());
            }
        }

        Flip();
    }
    public IEnumerator Stun()
    {
        playerShoot.enabled = false;
        jumpForce = 0f;
        yield return new WaitForSeconds(stunDuration);
        playerShoot.enabled = true;
        jumpForce = 16f;
        isStunned = false;
    }
    public void StopMovement()
    {
        horizontalMovement = 0f;
    }
    public void ResetSpeed()
    {
        actualSpeed = moveSpeed;
    }
    public float GetDefaultSpeed()
    {
        return moveSpeed;
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
        if (rb.velocity.y < 0)
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
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer); //<-- avoid IsGrounded instantly when jumping!
    }

    public bool IsSlippery()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer))
            return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer).gameObject.tag == "Slippery";
        return false;
    }
    public void Flip(bool force = false)
    {
        if (isFacingRight && horizontalMovement > 0 || !isFacingRight && horizontalMovement < 0 || force)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    private IEnumerator Blink()
    {
        SetAlpha(0.2f);
        yield return new WaitForSeconds(stunDuration);
        SetAlpha(1f);
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
