using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(FloatingObject))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement Settings")]
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public FloatingObject floatingObject;

    [SerializeField] private float moveSpeed;
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



    State currentState;
    [HideInInspector] public LocomotionState locomotionState;
    [HideInInspector] public JumpState jumpState;
    [HideInInspector] public WaterState waterState;
    [HideInInspector] public BubbleState bubbleState;



    public void ChangeState(State newState, bool callOnEnter = true)
    {
        currentState?.OnExit();
        State oldState = currentState;
        currentState = newState;

        if (callOnEnter)
            currentState.OnEnter(oldState);
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
        floatingObject = GetComponent<FloatingObject>();

        locomotionState = new LocomotionState(this);
        jumpState = new JumpState(this);
        waterState = new WaterState(this);
        bubbleState = new BubbleState(this);

        actualSpeed = moveSpeed;

        ChangeState(locomotionState);
    }

    public void Update()
    {
        currentState.Update();

        if (floatingObject.IsInWater() && !IsInState<WaterState>() && !IsInState<BubbleState>())
        {
            ChangeState(waterState);
        }
    }

    public void Movement()
    {
        rb.velocity = new Vector2(horizontalMovement * actualSpeed,rb.velocity.y);
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
        currentState.JumpCall(context);
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
    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, grondLayer) && Mathf.Abs(rb.velocity.y) <0.1f; //<-- avoid IsGrounded instantly when jumping!
    }


    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
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
        Gizmos.DrawCube(groundCheckPos.position,groundCheckSize);
    }


}
